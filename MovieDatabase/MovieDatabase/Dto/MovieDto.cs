using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Dto
{
    public class MovieDto
    {
        public string Id { get; set; }
        public string     Name  { get; set; }
        public List<MovieGenre> Genres { get; set; }

        public List<DynamicMetadataDto> Metadata { get; set; }

        public bool IsValid { get; set; }
    }
}
