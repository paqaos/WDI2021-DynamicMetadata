using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.CommandStack.CommandHandlers;
using MovieDatabase.CommandStack.Commands.MovieMetadatas;
using MovieDatabase.CommandStack.Queries.MovieMetadata;
using MovieDatabase.CommandStack.QueryHandlers;
using MovieDatabase.Dto;
using MovieDatabase.Dto.MovieMetadatas;

namespace MovieDatabase.Controllers
{
    [Route("movies/{movieId}/metadata")]
    public class MovieMetadataController : Controller
    {
        private readonly IQueryHandler<GetMovieMetadataQuery, List<DynamicMetadataDto>> _getMovieMetadataQueryHandler;
        private readonly ICommandHandler<CreateMovieMetadataCommand, DynamicMetadataDto> _createMovieMetadataCommandHandler;
        private readonly ICommandHandler<UpdateMovieMetadataCommand, DynamicMetadataDto> _updateMovieMetadataCommandHandler;
        private readonly ICommandHandler<DeleteMovieMetadataCommand, bool> _deleteMovieMetadataCommand;

        public MovieMetadataController(IQueryHandler<GetMovieMetadataQuery, List<DynamicMetadataDto>> getMovieMetadataQueryHandler, ICommandHandler<CreateMovieMetadataCommand, DynamicMetadataDto> createMovieMetadataCommandHandler, ICommandHandler<UpdateMovieMetadataCommand, DynamicMetadataDto> updateMovieMetadataCommandHandler, ICommandHandler<DeleteMovieMetadataCommand, bool> deleteMovieMetadataCommand)
        {
            _getMovieMetadataQueryHandler = getMovieMetadataQueryHandler;
            _createMovieMetadataCommandHandler = createMovieMetadataCommandHandler;
            _updateMovieMetadataCommandHandler = updateMovieMetadataCommandHandler;
            _deleteMovieMetadataCommand = deleteMovieMetadataCommand;
        }

        [HttpGet]
        public async Task<List<DynamicMetadataDto>> GetAllMovieMetadatas(string movieId)
        {
            return await _getMovieMetadataQueryHandler.Execute(new GetMovieMetadataQuery
            {
                MovieId = movieId 
            });
        }

        [HttpPost]
        public async Task<DynamicMetadataDto> CreateMovieMetadataAsync([FromRoute] string movieId,
                                                                       [FromBody]
                                                                       CreateMovieMetadataDto movieMetadataDto,
                                                                       CancellationToken ct)
        {
            return await _createMovieMetadataCommandHandler.HandleResult(new CreateMovieMetadataCommand
            {
                MovieId = movieId,
                Metadata = movieMetadataDto
            }, ct);
        }

        [HttpPut("{metadataId}")]
        public async Task<DynamicMetadataDto> UpdateMovieMetadataAsync([FromRoute] string movieId,
                                                                       [FromRoute] string metadataId,
                                                                       [FromBody]
                                                                       UpdateMovieMetadataDto movieMetadataDto,
                                                                       CancellationToken ct)
        {
            return await _updateMovieMetadataCommandHandler.HandleResult(new UpdateMovieMetadataCommand
            {
                MovieId = movieId,
                MetdataId = metadataId,
                Metadata = movieMetadataDto
            }, ct);
        }

        [HttpDelete("{metadataId}")]
        public async Task<bool> DeleteMovieMetadataAsync(string movieId, string metadataId, CancellationToken ct)
        {
            return await _deleteMovieMetadataCommand.HandleResult(new DeleteMovieMetadataCommand
            {
                MetdataId = metadataId,
                MovieId = movieId
            }, ct);
        }

    }
}
