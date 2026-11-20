using System.Text.Json.Serialization;

namespace Movie.Dtos.TMDB.Person;

public class PersonResponseDto
{
    [JsonPropertyName("id")]
    public long TmdbId { get; set; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }
}
