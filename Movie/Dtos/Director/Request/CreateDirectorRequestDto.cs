using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Director.Request;
using Movie.Entities;

namespace Movie.Dtos.Director.Request
{
    public class CreateDirectorRequestDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
