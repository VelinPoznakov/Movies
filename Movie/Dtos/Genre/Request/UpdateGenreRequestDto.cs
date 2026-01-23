using AutoMapper;
using Movie.Dtos.Genre.Request;
using Movie.Entities;

namespace Movie.Dtos.Genre.Request
{
    public class UpdateGenreRequestDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
