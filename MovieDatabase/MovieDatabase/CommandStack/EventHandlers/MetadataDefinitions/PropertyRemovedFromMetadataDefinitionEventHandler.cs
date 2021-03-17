using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.Events.MetadataDefinitions;
using MovieDatabase.Constants;
using MovieDatabase.Model;
using MovieDatabase.Services;

namespace MovieDatabase.CommandStack.EventHandlers.MetadataDefinitions
{
    public class PropertyRemovedFromMetadataDefinitionEventHandler : IEventHandler<PropertyRemovedFromMetadataDefinitionEvent>
    {
        private readonly IReadService<Movie>                     _movieReadService;
        private readonly IWriteService<Movie>                    _movieWriteService;

        public PropertyRemovedFromMetadataDefinitionEventHandler(IReadService<Movie> movieReadService, IWriteService<Movie> movieWriteService)
        {
            _movieReadService = movieReadService;
            _movieWriteService = movieWriteService;
        }

        /// <inheritdoc />
        /// 
        public async Task HandleEvent(PropertyRemovedFromMetadataDefinitionEvent eventData, CancellationToken ct)
        {
            var allMovies = _movieReadService.GetAll();

            foreach (var movie in allMovies)
            {
                var matchedMetadatas = movie.Metadata.Where(x => x.DefinitionId == eventData.DefinitionId).ToList();

                if (matchedMetadatas.Any())
                {
                    foreach (var matchedMetadata in matchedMetadatas)
                    {
                        var property = matchedMetadata.Properties.Single(x => x.Id == eventData.RemovedPropertyKey);
                        property.State = PropertyState.MarkedAsDeleted;
                    }

                    _movieWriteService.UpdateItem(movie, ct);
                }
            }
        }
    }
}
