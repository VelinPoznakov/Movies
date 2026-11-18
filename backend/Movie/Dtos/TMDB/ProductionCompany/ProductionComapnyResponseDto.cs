using System.Text.Json.Serialization;

namespace Movie.Dtos.TMDB.ProductionCompany;

public class ProductionComapnyResponseDto
{
    [JsonPropertyName("id")] 
    public long TmdbId { get; set; }

    [JsonPropertyName("logo_path")] 
    public string? LogoPath { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}