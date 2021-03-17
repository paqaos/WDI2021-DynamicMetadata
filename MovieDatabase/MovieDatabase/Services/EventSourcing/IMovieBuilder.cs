using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;
using MovieDatabase.Model.EventSourcing;

namespace MovieDatabase.Services.EventSourcing
{
    public interface IMovieBuilder
    {
        Movie ExecuteCommits(IOrderedEnumerable<Commit> commitToBeApplied);
    }
}
