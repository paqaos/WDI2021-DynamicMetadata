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
    [Route("movies/{movieId}")]
    public class MovieController : Controller
    {
        private readonly IReadService<Movie> _movieReadService;

        public MovieController(IReadService<Movie> movieReadService)
        {
            _movieReadService = movieReadService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public MovieDto GetAll(string movieId, CancellationToken ct)
        {
            return _movieReadService.GetById<MovieDto>(movieId);
        }
    }
}
