using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Dto.MovieMetadatas
{
    public class CreateMovieMetadataDto
    {
        public string MetadataDefinitionId { get; set; }

        public List<DynamicPropertyDto> DynamicProperties { get; set; }
    }
}
