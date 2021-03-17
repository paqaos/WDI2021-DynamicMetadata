using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;
using MovieDatabase.Model.EventSourcing;

namespace MovieDatabase.Services.EventSourcing
{
    public class MovieBuilder : IMovieBuilder
    {
        /// <inheritdoc />
        public Movie ExecuteCommits(IOrderedEnumerable<Commit> commitToBeApplied)
        {
            Movie movie = null;
            foreach (var commit in commitToBeApplied)
            {
                var orderedOperations = commit.Operations.OrderBy(x => x.OperationOrder).ToList();

                foreach (var operation in orderedOperations)
                {
                    movie = operation.ExecuteOperation(movie);
                }
            }

            return movie;
        }
    }
}
