using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.Validators;
using MovieDatabase.Model;

namespace MovieDatabase.Dto.MetadataDefinitions
{
    public class MetadataDefinitionDto
    {
        public string Name { get; set; }
        public string Id { get; set; }


        public IList<DynamicPropertyDto> Properties { get; set; } = new List<DynamicPropertyDto>();
        public List<ValidatorConfigurationDto> Validators { get; set; } = new List<ValidatorConfigurationDto>();
    }
}
