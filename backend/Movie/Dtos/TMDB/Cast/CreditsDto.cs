using System.Text.Json.Serialization;

namespace Movie.Dtos.TMDB.Cast;

public class CreditsDto
{
    [JsonPropertyName("crew")]
    public List<CrewDto> Crew { get; set; }
        = new List<CrewDto>();
}