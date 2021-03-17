using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Operations
{
    public abstract class MovieOperationBase : IMovieOperation
    {
        /// <inheritdoc />
        public int OperationOrder { get; set; }

        /// <inheritdoc />
        public string MovieId { get; set; }

        /// <inheritdoc />
        public virtual Movie ExecuteOperation(Movie movie)
        {
            movie.Version += 1;

            return movie;
        }
    }
}
