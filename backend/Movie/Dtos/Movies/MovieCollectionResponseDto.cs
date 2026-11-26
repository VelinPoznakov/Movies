namespace Movie.Dtos.Movies;

public class MovieCollectionResponseDto
{
    public ICollection<MovieFromCollectionDto> Movies { get; set; }
        = new List<MovieFromCollectionDto>();
    
    public int MoviesPerPage { get; set; }
    
    public int TotalPages { get; set; }
}