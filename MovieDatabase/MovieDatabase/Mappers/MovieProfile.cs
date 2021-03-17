using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Dto;
using MovieDatabase.Dto.Movies;
using MovieDatabase.Model;

namespace MovieDatabase.Mappers
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<CreateMovieDto, Movie>()
                .ForMember(x => x.Metadata, x => x.MapFrom(src => new List<DynamicMetadata>()))
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Version, x => x.MapFrom(src => 0))
                .ForMember(x => x.IsValid, x => x.MapFrom(src => true));

            CreateMap<Movie, MovieDto>();
        }
    }
}
