using System.Collections.Generic;
using MovieDatabase.Model.EventSourcing;

namespace MovieDatabase.Services.EventSourcing
{
    public interface ICommitService
    {
        List<Commit> GetCommitsForMovie(string movieId);
    }
}
