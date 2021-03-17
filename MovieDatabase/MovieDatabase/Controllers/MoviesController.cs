using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.CommandHandlers;
using MovieDatabase.CommandStack.Commands.Movies;
using MovieDatabase.Model;
using MovieDatabase.Dto;
using MovieDatabase.Dto.Movies;
using MovieDatabase.Services;

namespace MovieDatabase.Controllers
{
    [Route("movies")]
    public class MoviesController : Controller
    {
        private readonly IReadService<Movie> _movieReadService;
        private readonly ICommandHandler<CreateMovieCommand, MovieDto> _createMovieCommandHandler; 

        public MoviesController(IReadService<Movie> movieReadService, ICommandHandler<CreateMovieCommand, MovieDto> createMovieCommandHandler)
        {
            _movieReadService = movieReadService;
            _createMovieCommandHandler = createMovieCommandHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public List<MovieDto> GetAll()
        {
            return _movieReadService.GetAllAs<MovieDto>();
        }

        [HttpPost("")]
        public async Task<MovieDto> CreateMovieAsync([FromBody] CreateMovieDto createMovie, CancellationToken cancellationToken)
        {
            return await _createMovieCommandHandler.HandleResult(new CreateMovieCommand
            {
                MovieData = createMovie
            }, cancellationToken);
        }
    }
}
