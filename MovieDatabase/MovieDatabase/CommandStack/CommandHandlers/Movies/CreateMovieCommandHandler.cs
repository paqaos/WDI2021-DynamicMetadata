using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.Commands.Movies;
using MovieDatabase.Dto;
using MovieDatabase.Dto.Movies;
using MovieDatabase.Model;
using MovieDatabase.Services;

namespace MovieDatabase.CommandStack.CommandHandlers.Movies
{
    public class CreateMovieCommandHandler : ICommandHandler<CreateMovieCommand, MovieDto>
    {
        private readonly IWriteService<Movie> _movieWriteService;
        private readonly IMapper _mapper;

        public CreateMovieCommandHandler(IWriteService<Movie> movieWriteService, IMapper mapper)
        {
            _movieWriteService = movieWriteService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<MovieDto> HandleResult(CreateMovieCommand command, CancellationToken token)
        {
            var movie = _mapper.Map<Movie>(command.MovieData);

            movie = _movieWriteService.CreateItem(movie, token);

            return _mapper.Map<MovieDto>(movie);
        }
    }
}
