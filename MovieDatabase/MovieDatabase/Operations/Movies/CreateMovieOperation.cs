using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Operations.Movies
{
    public class CreateMovieOperation : MovieOperationBase
    {
        public string MovieId { get; set; }

        public override Movie ExecuteOperation(Movie movie)
        {
            return new Movie
            {
                Id = MovieId,
                Genres = new List<MovieGenre>(),
                Metadata = new List<DynamicMetadata>()
            };
        }
    }
}
