using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Dto;
using MovieDatabase.Model;

namespace MovieDatabase.Mappers
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonDto>();
        }
    }
}
