using System.Text.Json.Serialization;
using Movie.Dtos.TMDB.Cast;
using Movie.Dtos.TMDB.Genre;
using Movie.Dtos.TMDB.ProductionCompany;

namespace Movie.Dtos.TMDB.Movie;

public class MovieDetailsResponseDto: MovieResponseDto
{
    [JsonPropertyName("budget")]
    public long Budget { get; set; }

    [JsonPropertyName("genres")]
    public List<GenreResponseDto> GenreResponseDtos { get; set; }
        = new List<GenreResponseDto>();

    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; set; }

    [JsonPropertyName("production_companies")]
    public List<ProductionComapnyResponseDto> ProductionCompanies { get; set; }
        = new List<ProductionComapnyResponseDto>();

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;

    [JsonPropertyName("credits")] 
    public CreditsDto CreditsDto { get; set; } = null!;
}