using System.Text.Json.Serialization;

namespace Movie.Dtos.TMDB.Movie;

public class MovieResponseDto
{
    [JsonPropertyName("adult")]
    public bool IsAdult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("genre_ids")]
    public List<long> GenresIds { get; set; }
        = new List<long>();
    
    [JsonPropertyName("id")]
    public long TmdbId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("overview")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
    
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }
}