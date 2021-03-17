using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.Events.MetadataValidators;
using MovieDatabase.Model;
using MovieDatabase.Services;
using MovieDatabase.Services.Validation;

namespace MovieDatabase.CommandStack.EventHandlers.MetadataValidators
{
    public class RefreshValidationEventHandler : IEventHandler<RefreshValidationEvent>
    {
        private readonly IReadService<DynamicMetadataDefinition> _metadataReadService;
        private readonly IReadService<Movie> _movieReadService;
        private readonly IWriteService<Movie> _movieWriteService;
        private readonly IValidationFactory _validationFactory;

        public RefreshValidationEventHandler(IReadService<DynamicMetadataDefinition> metadataReadService, IReadService<Movie> movieReadService, IWriteService<Movie> movieWriteService, IValidationFactory validationFactory)
        {
            _metadataReadService = metadataReadService;
            _movieReadService = movieReadService;
            _movieWriteService = movieWriteService;
            _validationFactory = validationFactory;
        }

        public async Task HandleEvent(RefreshValidationEvent eventData, CancellationToken ct)
        {
            var allMovies = _movieReadService.GetAll();
            var metadataDefinition = _metadataReadService.GetById(eventData.DefinitionId);
            var validators = _validationFactory.GetValidator(metadataDefinition);

            foreach (var movie in allMovies)
            {
                var updatedDefinitions = movie.Metadata.Where(x => x.DefinitionId == metadataDefinition.Id).ToList();

                if(!updatedDefinitions.Any())
                    continue;

                bool isValidMovie = true;
                foreach (var definition in updatedDefinitions)
                {
                    var propertyValidationErrors = validators.Validate(definition, movie);

                    if (propertyValidationErrors.Any())
                    {
                        definition.IsValid = false;
                        isValidMovie = false;
                    }

                    if (!propertyValidationErrors.Any())
                    {
                        definition.IsValid = true;
                    }
                }

                movie.IsValid = isValidMovie;

                _movieWriteService.UpdateItem(movie, ct);
            }

        }
    }
}
