using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Model
{
    public class Movie : DatabaseItem
    {
        public string Name { get; set; }
        public List<MovieGenre> Genres { get; set; }

        /// <inheritdoc />
        public override string Type { get; } = "Movie";

        public List<DynamicMetadata> Metadata { get; set; }
        public int Version { get; set; }
        public bool IsValid { get; set; }
    }
}
