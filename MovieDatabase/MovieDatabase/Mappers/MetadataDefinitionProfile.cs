using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Constants;
using MovieDatabase.Dto.MetadataDefinitions;
using MovieDatabase.Dto.Validators;
using MovieDatabase.Model;

namespace MovieDatabase.Mappers
{
    public class MetadataDefinitionProfile : Profile
    {
        public MetadataDefinitionProfile()
        {
            CreateMap<DynamicMetadataDefinition, MetadataDefinitionDto>();

            CreateMap<CreateMetadataPropertyDefinitionDto, DynamicMetadataProperty>()
                .ForMember(x => x.Id, x => x.MapFrom(src => src.Key))
                .ForMember(x => x.State, x => x.MapFrom(src => PropertyState.Active))
                .ForMember(x => x.HasDefaultValue, x => x.MapFrom(src => true))
                .ForMember(x => x.Value, x => x.Ignore());

            CreateMap<ValidatorConfiguration, ValidatorConfigurationDto>();
        }
    }
}
