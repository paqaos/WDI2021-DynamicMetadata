using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.Events.MetadataDefinitions;
using MovieDatabase.Model;
using MovieDatabase.Services;

namespace MovieDatabase.CommandStack.EventHandlers.MetadataDefinitions
{
    public class PropertyAddedToMetadataDefinitionEventHandler : IEventHandler<PropertyAddedToMetadataDefinitionEvent>
    {
        private readonly IReadService<Movie> _movieReadService;
        private readonly IWriteService<Movie> _movieWriteService;
        private readonly IReadService<DynamicMetadataDefinition> _dynamicMetadataDefinition;
        private readonly IMapper _mapper;

        public PropertyAddedToMetadataDefinitionEventHandler(IReadService<Movie> movieReadService, IWriteService<Movie> movieWriteService, IReadService<DynamicMetadataDefinition> dynamicMetadataDefinition, IMapper mapper)
        {
            _movieReadService = movieReadService;
            _movieWriteService = movieWriteService;
            _dynamicMetadataDefinition = dynamicMetadataDefinition;
            _mapper = mapper;
        }
     
        /// <inheritdoc />
        public async Task HandleEvent(PropertyAddedToMetadataDefinitionEvent eventData, CancellationToken ct)
        {
            var allMovies = _movieReadService.GetAll();
            var updatedDefinition = _dynamicMetadataDefinition.GetById(eventData.DefinitionId);
            var addedProperty = updatedDefinition.Properties.SingleOrDefault(x => x.Id == eventData.AddedPropertyId);

            foreach (var movie in allMovies)
            {
                var matchedMetadatas = movie.Metadata.Where(x => x.DefinitionId == eventData.DefinitionId).ToList();

                if (matchedMetadatas.Any())
                {
                    foreach (var matchedMetadata in matchedMetadatas)
                    {
                        var copy = _mapper.Map<DynamicMetadataProperty>(addedProperty);
                        copy.HasDefaultValue = true;
                        matchedMetadata.Properties.Add(copy);
                    }

                    _movieWriteService.UpdateItem(movie, ct);
                }
            }
        }
    }
}
