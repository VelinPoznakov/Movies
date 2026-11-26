using Movie.Dtos.Comments;
using Movie.Dtos.Director;
using Movie.Dtos.Genre;
using Movie.Dtos.ProducerComapny;

namespace Movie.Dtos.Movies;

public class MovieDetailsResponseDto
{
    public long Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public int TimeLong { get; set; }
    
    public double Budget { get; set; }
    
    public string MovieStatus { get; set; } = null!;
    
    public string? BackdropUrl { get; set; } = null!;
    
    public string ReleaseDate { get; set; } = null!;
    
    public bool IsAdult { get; set; }
    
    public double Rating { get; set; }
    
    public ICollection<GenreDto> Genres { get; set; } 
        = new List<GenreDto>();
    
    public ICollection<ProducerCompanyDetails> ProducerCompanies { get; set; } 
        = new List<ProducerCompanyDetails>();
    
    public ICollection<DirectorDto> Directors { get; set; }
        = new List<DirectorDto>();
    
    public ICollection<CommentDto> Comments { get; set; }
        = new List<CommentDto>();
}