using System.Text.Json.Serialization;

namespace Movie.Dtos.TMDB.ProductionCompany;

public class CompanyDetailsResponseDto
{
    [JsonPropertyName("id")]
    public long TmdbId { get; set; }

    [JsonPropertyName("logo_path")]
    public string? LogoPath { get; set; }
}
