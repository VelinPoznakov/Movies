using AutoMapper;
using Movie.Dtos.Director.Response;
using Movie.Entities;

namespace Movie.Dtos.Director.Response
{
    public class DirectorReasponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}

