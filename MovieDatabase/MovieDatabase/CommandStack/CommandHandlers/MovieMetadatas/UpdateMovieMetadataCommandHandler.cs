using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.Commands.MovieMetadatas;
using MovieDatabase.Dto;
using MovieDatabase.Exceptions;
using MovieDatabase.Model;
using MovieDatabase.Services;
using MovieDatabase.Services.MetadataObject;
using MovieDatabase.Services.Validation;

namespace MovieDatabase.CommandStack.CommandHandlers.MovieMetadatas
{
    public class UpdateMovieMetadataCommandHandler : ICommandHandler<UpdateMovieMetadataCommand, DynamicMetadataDto>
    {
        private readonly IReadService<Movie> _movieReadService;
        private readonly IWriteService<Movie> _movieWriteService;
        private readonly IMapper _mapper;
        private readonly IInputDataParser _inputDataParser;
        private readonly IReadService<DynamicMetadataDefinition> _dynamicMetadataDefinitionReadService;
        private readonly IValidationFactory _validationFactory;

        public UpdateMovieMetadataCommandHandler(IReadService<Movie> movieReadService, IWriteService<Movie> movieWriteService, IMapper mapper, IInputDataParser inputDataParser, IReadService<DynamicMetadataDefinition> dynamicMetadataDefinitionReadService, IValidationFactory validationFactory)
        {
            _movieReadService = movieReadService;
            _movieWriteService = movieWriteService;
            _mapper = mapper;
            _inputDataParser = inputDataParser;
            _dynamicMetadataDefinitionReadService = dynamicMetadataDefinitionReadService;
            _validationFactory = validationFactory;
        }

        /// <inheritdoc />
        public  async Task<DynamicMetadataDto> HandleResult(UpdateMovieMetadataCommand command, CancellationToken token)
        {
            var movie = _movieReadService.GetById(command.MovieId);

            var metadata = movie.Metadata.FirstOrDefault(x => x.Id == command.MetdataId);

            var definition = _dynamicMetadataDefinitionReadService.GetById(metadata.DefinitionId);

            var validator = _validationFactory.GetValidator(definition);

            foreach (var metadataEntry in command.Metadata.DynamicProperties)
            {
                var propertyDefinition = definition.Properties.SingleOrDefault(x => x.Id == metadataEntry.Id);
                if (propertyDefinition != null)
                {
                    if (propertyDefinition.Type != metadataEntry.Type)
                    {
                        throw new AggregatedValidationException("Type mismatch")
                        {
                            ErrorCode = 11231,
                            ValidationErrors = new List<Error>()
                        };
                    }

                    var existingMetadata = metadata.Properties.SingleOrDefault(x => x.Id == metadataEntry.Id);
                    if (existingMetadata != null)
                    {
                        try
                        {
                            var parsedValue = _inputDataParser.DeserializeItem(metadataEntry.Id, metadataEntry.Type, metadataEntry.Value);

                            parsedValue.HasDefaultValue = false;
                            metadata.Properties.Remove(existingMetadata);
                            metadata.Properties.Add(parsedValue);
                        }
                        catch (JsonException exception)
                        {
                            throw new AggregatedValidationException("Invalid type mismatch")
                            {
                                ErrorCode = 11231,
                                ValidationErrors = new List<Error>()
                            };
                        }
                    }
                }
            }

            var validationResult = validator.Validate(metadata, movie);

            if (validationResult.Any())
            {
                throw new AggregatedValidationException("Validation exception")
                {
                    ErrorCode = 1,
                    ValidationErrors = validationResult
                };
            }


            movie.Metadata.Add(metadata);

            _movieWriteService.UpdateItem(movie, token);

            var resultValue = _mapper.Map<DynamicMetadataDto>(metadata);
            return resultValue;
        }
    }
}
