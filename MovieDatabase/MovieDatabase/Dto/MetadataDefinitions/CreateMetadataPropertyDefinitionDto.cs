using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Dto.MetadataDefinitions
{
    public class CreateMetadataPropertyDefinitionDto
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public object DefaultValue { get; set; }
    }
}
