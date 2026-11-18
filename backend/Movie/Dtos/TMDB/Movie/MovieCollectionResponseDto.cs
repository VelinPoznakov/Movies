using System.Text.Json.Serialization;

namespace Movie.Dtos.TMDB.Movie;

public class MovieCollectionResponseDto
{
    [JsonPropertyName("page")]
    public int Page { get; set; }
    
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
    
    [JsonPropertyName("total_results")]
    public long TotalResult { get; set; }

    [JsonPropertyName("results")]
    public List<MovieResponseDto> Movies { get; set; }
        = new List<MovieResponseDto>();
}