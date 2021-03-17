using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Dto.MetadataDefinitions
{
    public class UpdateMetadataPropertyDefinitionDto
    {
        public string Type         { get; set; }
        public object DefaultValue { get; set; }
    }
}
