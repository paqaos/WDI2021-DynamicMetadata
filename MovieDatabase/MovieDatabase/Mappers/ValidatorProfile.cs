using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Dto.Validators;
using MovieDatabase.Model;

namespace MovieDatabase.Mappers
{
    public class ValidatorProfile : Profile
    {
        public ValidatorProfile()
        {
            CreateMap<AddValidatorDto, ValidatorConfiguration>()
                .ForMember(x => x.ValidatorId, x => x.Ignore());
        }
    }
}
