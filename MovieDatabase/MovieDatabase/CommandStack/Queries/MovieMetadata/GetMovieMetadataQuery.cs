using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.CommandStack.Queries.MovieMetadata
{
    public class GetMovieMetadataQuery : IQuery
    {
        public string MovieId { get; set; }
    }
}
