using System;
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
    public class CreateMovieMetadataCommandHandler : ICommandHandler<CreateMovieMetadataCommand, DynamicMetadataDto>
    {
        private readonly IReadService<Movie> _movieReadService;
        private readonly IWriteService<Movie> _movieWriteService;
        private readonly IReadService<DynamicMetadataDefinition> _dynamicMetadataDefinitionReadService;
        private readonly IInputDataParser _inputDataParser;
        private readonly IMapper _mapper;
        private readonly IValidationFactory _validatorFactory;

        public CreateMovieMetadataCommandHandler(IReadService<Movie> movieReadService, IWriteService<Movie> movieWriteService, IInputDataParser inputDataParser, IMapper mapper, IReadService<DynamicMetadataDefinition> dynamicMetadataDefinitionReadService, IValidationFactory validatorFactory)
        {
            _movieReadService = movieReadService;
            _movieWriteService = movieWriteService;
            _inputDataParser = inputDataParser;
            _mapper = mapper;
            _dynamicMetadataDefinitionReadService = dynamicMetadataDefinitionReadService;
            _validatorFactory = validatorFactory;
        }

        /// <inheritdoc />
        public async Task<DynamicMetadataDto> HandleResult(CreateMovieMetadataCommand command, CancellationToken token)
        {
            var movie = _movieReadService.GetById(command.MovieId);

            if(movie == null)
                throw new NotFoundException();

            var definition = _dynamicMetadataDefinitionReadService.GetById(command.Metadata.MetadataDefinitionId);

            if(definition == null)
                throw new NotFoundException();

            var validator = _validatorFactory.GetValidator(definition);

            var metadata = new DynamicMetadata
            {
                DefinitionId = command.Metadata.MetadataDefinitionId,
                Id = Guid.NewGuid().ToString()
            };

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
                    if (existingMetadata == null)
                    {
                        try
                        {
                            var parsedValue = _inputDataParser.DeserializeItem(metadataEntry.Id, metadataEntry.Type,
                                metadataEntry.Value);
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

            foreach (var propertyInDefinition in definition.Properties)
            {
                var existingProperty = metadata.Properties.FirstOrDefault(x => x.Id == propertyInDefinition.Id);
                if (existingProperty == null)
                {
                    metadata.Properties.Add(propertyInDefinition);
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
