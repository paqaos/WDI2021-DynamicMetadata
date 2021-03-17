using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model.EventSourcing;
using MovieDatabase.Repositories;

namespace MovieDatabase.Services.EventSourcing
{
    public class CommitService : ICommitService
    {
        private readonly IRepository<Commit> _commitRepository;

        public CommitService(IRepository<Commit> commitRepository)
        {
            _commitRepository = commitRepository;
        }

        /// <inheritdoc />
        public List<Commit> GetCommitsForMovie(string movieId)
        {
            return _commitRepository.GetAll().Where(x => x.MovieId == movieId).ToList();
        }
    }
}
