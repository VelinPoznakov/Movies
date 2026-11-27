using Movie.Dtos.Movies;

namespace Movie.Dtos.Genre;

public class GenreWithMoviesDto: GenreDto
{
    public ICollection<MovieFromCollectionDto> Movies { get; set; }
        = new List<MovieFromCollectionDto>();

    public int Page { get; set; }

    public int TotalPages { get; set; }
}