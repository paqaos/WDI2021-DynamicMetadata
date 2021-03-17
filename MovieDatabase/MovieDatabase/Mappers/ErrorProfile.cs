using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Dto;
using MovieDatabase.Model;

namespace MovieDatabase.Mappers
{
    public class ErrorProfile : Profile
    {
        public ErrorProfile()
        {
            CreateMap<Error, ErrorDto>();
        }
    }
}
