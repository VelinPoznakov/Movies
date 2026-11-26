using Microsoft.AspNetCore.Components.Web;

namespace Movie.Dtos.Movies;

public class MovieFromCollectionDto
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;
    
    public int TimeLong { get; set; }

    public string MovieStatus { get; set; } = null!;

    public string? PosterUrl { get; set; }
    
    public double Rating { get; set; }
    
    public long Views { get; set; }
}