using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Dto;
using MovieDatabase.Model.EventSourcing;
using MovieDatabase.Services;
using MovieDatabase.Services.EventSourcing;

namespace MovieDatabase.Controllers.EventSourcingBased
{
    [Route("es/movies")]
    public class EventSourcedMoviesController : Controller
    {
        private readonly ICommitService _commitService;
        private readonly IMovieBuilder _movieBuilder;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public EventSourcedMoviesController(ICommitService commitService, IMovieBuilder movieBuilder, IMapper mapper)
        {
            _commitService = commitService;
            _movieBuilder = movieBuilder;
            _mapper = mapper;
        }

        [HttpGet("{movieId}/version/{version:int}")]
        public MovieDto GetMovie(string movieId, int version)
        {
            var commits = _commitService.GetCommitsForMovie(movieId);

            var commitToBeApplied = commits.Where(x => x.CommitOrder <= version).OrderBy(x => x.CommitOrder);

            var movie = _movieBuilder.ExecuteCommits(commitToBeApplied);

            return _mapper.Map<MovieDto>(movie);
        }
    }
}
