using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Converters;
using MovieDatabase.Dto;
using MovieDatabase.Model;

namespace MovieDatabase.Mappers
{
    public class MetadataProfile : Profile
    {
        public MetadataProfile()
        { 
            CreateMap<DynamicMetadata, DynamicMetadataDto>();

            CreateMap<DynamicMetadataProperty, DynamicPropertyDto>()
                .ForMember(x => x.Value, x => x.MapFrom<PropertyConverter>());
        }
    }
}
