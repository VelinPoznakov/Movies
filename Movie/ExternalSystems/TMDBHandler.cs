using System.Globalization;
using Microsoft.AspNetCore.WebUtilities;
using Movie.BackgroundServices.JobServices;
using Movie.Dtos.TMDB.Movie;
using Movie.ExternalSystems.Contracts;

namespace Movie.ExternalSystems;

public class TMDBHandler: ITMDBHandler
{
    private readonly ILogger<MovieSeederJobService> _logger;
    private readonly IConfigurationSection _endpoints;
    private readonly IConfigurationSection _query;
    private readonly HttpClient _apiClient;
    private readonly HttpClient _imageClient;
    private readonly DateOnly _startDate;

    public TMDBHandler(ILogger<MovieSeederJobService> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _apiClient = httpClientFactory.CreateClient("TMDB");
        _imageClient = httpClientFactory.CreateClient("TMDBImages");
        _endpoints = configuration.GetSection("Endpoints");
        _query = configuration.GetSection("Query");
        _startDate = configuration.GetValue<DateOnly>("FromDate");
    }

    public async Task<MovieCollectionResponseDto> DiscoverMoviesAsync(DateOnly fromDate, int page = 1)
    {
        string url = QueryHelpers.AddQueryString(_endpoints["DiscoverMovies"]!,
            new Dictionary<string, string?>
            {
                ["page"] = page.ToString(),
                ["language"] = _query["Language"],
                ["include_adult"] = _query.GetValue<bool>("IncludeAdult").ToString().ToLowerInvariant(),
                ["sort_by"] = _query["SortBy"],
                ["vote_count.gte"] = _query["MinVoteCount"],
                ["with_runtime.gte"] = _query["MinRuntime"],
                ["primary_release_date.gte"] = fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                ["primary_release_date.lte"] = _startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            }
        );
        
        MovieCollectionResponseDto? result = await GetAsync<MovieCollectionResponseDto>(url);
        
        if(result == null)
            throw new Exception("Could not find movie collection response");

        return result;
    }
    
    public async Task<MovieDetailsResponseDto> GetMovieDetailsAsync(long tmdbId, bool includeCast = false)
    {
        string formatedMovieDetailsUri = string.Format(_query["MovieDetails"]!, tmdbId);

        Dictionary<string, string?> queryParams = new Dictionary<string, string?>()
        {
            ["language"] = _query["Language"]
        };

        if (includeCast)
            queryParams["append_to_response"] = _query["AppendToResponse"];

        string url = QueryHelpers.AddQueryString(formatedMovieDetailsUri, queryParams);
        
        MovieDetailsResponseDto? responseDto = await GetAsync<MovieDetailsResponseDto>(url);
        
        if (responseDto == null)
            throw new Exception("Could not find movie details response");
        
        return responseDto;
    }

    public async Task<byte[]?> GetImageAsync(string imageUrl)
    {
        using HttpResponseMessage response = await _imageClient.GetAsync(imageUrl);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            
            _logger.LogError($"Error on {_imageClient.BaseAddress}{imageUrl}: Status {response.StatusCode}: Error: {error}");

            return default;
        }
        
        return await response.Content.ReadFromJsonAsync<byte[]>() ?? Array.Empty<byte>();
    }
 
    private async Task<T?> GetAsync<T>(string url)
    {
        using HttpResponseMessage response = await _apiClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            
            _logger.LogError($"Error on {url}: Status {response.StatusCode}: Error: {error}");

            return default;
        }
        
        return await response.Content.ReadFromJsonAsync<T>();
    }
    
}