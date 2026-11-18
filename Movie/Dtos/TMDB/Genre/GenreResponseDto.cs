using System.Text.Json.Serialization;

namespace Movie.Dtos.TMDB.Genre;

public class GenreResponseDto
{
    [JsonPropertyName("id")]
    public long TmdbId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}