using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Dto
{
    public class DynamicMetadataDto
    {
        public IList<DynamicPropertyDto> Properties { get; set; } = new List<DynamicPropertyDto>();

        public string DefinitionId { get; set; }
        public string Id           { get; set; }

        public bool IsValid { get; set; }
    }
}
