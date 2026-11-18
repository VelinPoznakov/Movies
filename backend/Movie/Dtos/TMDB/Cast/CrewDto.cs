using System.Text.Json.Serialization;

namespace Movie.Dtos.TMDB.Cast;

public class CrewDto
{
    [JsonPropertyName("id")]
    public long TmdbId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("job")]
    public string Job { get; set; } = null!;

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }
}