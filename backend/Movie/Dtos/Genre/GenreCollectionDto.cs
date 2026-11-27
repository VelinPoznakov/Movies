namespace Movie.Dtos.Genre;

public class GenreCollectionDto
{
    public ICollection<GenreDto> Genres { get; set; } 
        = new List<GenreDto>();
    
    public int Page { get; set; }
    
    public int TotalPages { get; set; }
}