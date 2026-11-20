using System.Globalization;
using Microsoft.AspNetCore.WebUtilities;
using Movie.Dtos.TMDB.Movie;
using Movie.Dtos.TMDB.Person;
using Movie.Dtos.TMDB.ProductionCompany;
using Movie.ExternalSystems.Contracts;

namespace Movie.ExternalSystems;

public class TMDBHandler: ITMDBHandler
{
    private readonly ILogger<TMDBHandler> _logger;
    private readonly IConfigurationSection _endpoints;
    private readonly IConfigurationSection _query;
    private readonly HttpClient _apiClient;
    private readonly HttpClient _imageClient;

    public TMDBHandler(ILogger<TMDBHandler> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _apiClient = httpClientFactory.CreateClient("TMDB");
        _imageClient = httpClientFactory.CreateClient("TMDBImages");
        _endpoints = configuration.GetSection("TMDBSettings:Endpoints");
        _query = configuration.GetSection("TMDBSettings:Query");
    }

    public async Task<MovieCollectionResponseDto?> DiscoverMoviesAsync(DateOnly fromDate, DateOnly toDate, int page = 1)
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
                ["primary_release_date.lte"] = toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            }
        );
        
        MovieCollectionResponseDto? result = await GetAsync<MovieCollectionResponseDto>(url);

        if (result == null || result.Movies.Count == 0)
            return null;

        return result;
    }
    
    public async Task<MovieDetailsResponseDto?> GetMovieDetailsAsync(long tmdbId, bool includeCast = false)
    {
        string formatedMovieDetailsUri = string.Format(_endpoints["MovieDetails"]!, tmdbId);

        Dictionary<string, string?> queryParams = new Dictionary<string, string?>()
        {
            ["language"] = _query["Language"]
        };

        if (includeCast)
            queryParams["append_to_response"] = _query["AppendToResponse"];

        string url = QueryHelpers.AddQueryString(formatedMovieDetailsUri, queryParams);
        
        MovieDetailsResponseDto? responseDto = await GetAsync<MovieDetailsResponseDto>(url);
        
        if (responseDto == null)
            return null;
        
        return responseDto;
    }

    // GET person/{id} → profile_path of a director/actor
    public Task<PersonResponseDto?> GetPersonAsync(long tmdbId) =>
        GetAsync<PersonResponseDto>(string.Format(_endpoints["Person"]!, tmdbId));

    // GET company/{id} → logo_path of a production company
    public Task<CompanyDetailsResponseDto?> GetCompanyAsync(long tmdbId) =>
        GetAsync<CompanyDetailsResponseDto>(string.Format(_endpoints["Company"]!, tmdbId));

    public async Task<Stream?> GetImageStreamAsync(string imagePath)
    {
        HttpResponseMessage response = await _imageClient.GetAsync(imagePath, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Image {Path} returned {Status}", imagePath, (int)response.StatusCode);

            response.Dispose();

            return null;
        }

        return await response.Content.ReadAsStreamAsync();
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