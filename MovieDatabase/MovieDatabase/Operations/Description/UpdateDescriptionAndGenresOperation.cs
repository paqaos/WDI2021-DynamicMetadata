using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Operations.Description
{
    public class UpdateDescriptionAndGenresOperation : MovieOperationBase
    {
        public string MovieName { get; set; }
        public List<MovieGenre> Genres { get; set; }
        /// <inheritdoc />
        public int OperationOrder { get; set; }

        /// <inheritdoc />
        public string MovieId { get; set; }

        /// <inheritdoc />
        public override Movie ExecuteOperation(Movie movie)
        {
            movie.Name = MovieName;
            movie.Genres = Genres;
            movie = base.ExecuteOperation(movie);

            return movie;
        }
    }
}
