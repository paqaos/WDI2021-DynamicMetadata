using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.Events.MetadataDefinitions;
using MovieDatabase.Constants;
using MovieDatabase.Model;
using MovieDatabase.Services;
using MovieDatabase.Services.MetadataObject;
using MovieDatabase.Services.Validation;

namespace MovieDatabase.CommandStack.EventHandlers.MetadataDefinitions
{
    public class PropertyDefinitionUpdatedEventHandler : IEventHandler<PropertyDefinitionUpdatedEvent>
    {
        private readonly IReadService<Movie>                     _movieReadService;
        private readonly IWriteService<Movie>                    _movieWriteService;
        private readonly IReadService<DynamicMetadataDefinition> _dynamicMetadataDefinition;
        private readonly IValidationFactory _validationFactory;
        private readonly IMapper _mapper;
        public PropertyDefinitionUpdatedEventHandler(IReadService<Movie> movieReadService, IWriteService<Movie> movieWriteService, IReadService<DynamicMetadataDefinition> dynamicMetadataDefinition, IValidationFactory validationFactory, IMapper mapper)
        {
            _movieReadService = movieReadService;
            _movieWriteService = movieWriteService;
            _dynamicMetadataDefinition = dynamicMetadataDefinition;
            _validationFactory = validationFactory;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task HandleEvent(PropertyDefinitionUpdatedEvent eventData, CancellationToken ct)
        {
            var definition = _dynamicMetadataDefinition.GetById(eventData.DefinitionId);
            var allMovies = _movieReadService.GetAll();
            var validator = _validationFactory.GetValidator(definition);

            var propertyInDefinition = definition.Properties.Single(x => x.Id == eventData.UpdatedPropertyKey);

            foreach (var movie in allMovies)
            {
                var matchedMetadatas = movie.Metadata.Where(x => x.DefinitionId == eventData.DefinitionId).ToList();

                if (matchedMetadatas.Any())
                {
                    foreach (var matchedMetadata in matchedMetadatas)
                    {
                        var property = matchedMetadata.Properties.Single(x => x.Id == eventData.UpdatedPropertyKey);

                        bool alreadySetStatus = false;
                        if (property.HasDefaultValue)
                        {
                            var copy = _mapper.Map<DynamicMetadataProperty>(propertyInDefinition);
                            copy.HasDefaultValue = true;

                            matchedMetadata.Properties.Remove(property);
                            matchedMetadata.Properties.Add(copy);

                        }
                        else if (property.Type != propertyInDefinition.Type)
                        {
                            property.State = PropertyState.UsesOldType;
                            alreadySetStatus = true;
                        }

                        var validationErrors = validator.Validate(matchedMetadata, movie);

                        if (validationErrors.Any() && !alreadySetStatus)
                        {
                            property.State = PropertyState.Invalidated;
                        }
                    }

                    _movieWriteService.UpdateItem(movie, ct);
                }
            }
        }
    }
}
