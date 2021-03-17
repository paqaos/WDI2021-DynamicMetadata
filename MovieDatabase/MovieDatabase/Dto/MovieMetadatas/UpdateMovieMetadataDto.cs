using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Dto.MovieMetadatas
{
    public class UpdateMovieMetadataDto
    {
        public List<DynamicPropertyDto> DynamicProperties { get; set; }
    }
}
