using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.Commands.MovieMetadatas;
using MovieDatabase.Exceptions;
using MovieDatabase.Model;
using MovieDatabase.Services;

namespace MovieDatabase.CommandStack.CommandHandlers.MovieMetadatas
{
    public class DeleteMovieMetadataCommandHandler : ICommandHandler<DeleteMovieMetadataCommand, bool>
    {
        private readonly IReadService<Movie>  _movieReadService;
        private readonly IWriteService<Movie> _movieWriteService;

        public DeleteMovieMetadataCommandHandler(IReadService<Movie> movieReadService, IWriteService<Movie> movieWriteService)
        {
            _movieReadService = movieReadService;
            _movieWriteService = movieWriteService;
        }

        /// <inheritdoc />
        public async Task<bool> HandleResult(DeleteMovieMetadataCommand command, CancellationToken token)
        {
            var movie = _movieReadService.GetById(command.MovieId);
            if(movie == null)
                throw new NotFoundException();

            var metadata = movie.Metadata.Single(x => x.Id == command.MetdataId);

            if(metadata == null)
                throw new NotFoundException();

            movie.Metadata.Remove(metadata);

            _movieWriteService.UpdateItem(movie, token);

            return true;
        }
    }
}
