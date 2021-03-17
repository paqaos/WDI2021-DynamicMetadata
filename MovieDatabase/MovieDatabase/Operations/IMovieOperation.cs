using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Operations
{
    public interface IMovieOperation
    {
        int OperationOrder { get; set; }
        string MovieId { get; set; }
        Movie ExecuteOperation(Movie movie);
    }
}
