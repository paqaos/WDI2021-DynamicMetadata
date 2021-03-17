using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.Commands.MetadataDefinitions;
using MovieDatabase.CommandStack.Events.MetadataDefinitions;
using MovieDatabase.Dto.MetadataDefinitions;
using MovieDatabase.Exceptions;
using MovieDatabase.Model;
using MovieDatabase.Services;
using MovieDatabase.Services.EventProcessing;
using MovieDatabase.Services.MetadataObject;

namespace MovieDatabase.CommandStack.CommandHandlers.MetadataDefinitions
{
    public class ChangePropertyTypeAndDefaultValueCommandHandler : ICommandHandler<ChangePropertyTypeAndDefaultValueCommand, MetadataDefinitionDto>
    {
        private readonly IReadService<DynamicMetadataDefinition>  _dynamicMetadataDefinitonService;
        private readonly IWriteService<DynamicMetadataDefinition> _dynamicMetadataDefinitionWriteService;
        private readonly IInputDataParser                         _inputDataParser;
        private readonly IMapper                                  _mapper;
        private readonly IEventDispatcher                         _eventDispatcher;

        public ChangePropertyTypeAndDefaultValueCommandHandler(IReadService<DynamicMetadataDefinition> dynamicMetadataDefinitonService, IWriteService<DynamicMetadataDefinition> dynamicMetadataDefinitionWriteService, IInputDataParser inputDataParser, IMapper mapper, IEventDispatcher eventDispatcher)
        {
            _dynamicMetadataDefinitonService = dynamicMetadataDefinitonService;
            _dynamicMetadataDefinitionWriteService = dynamicMetadataDefinitionWriteService;
            _inputDataParser = inputDataParser;
            _mapper = mapper;
            _eventDispatcher = eventDispatcher;
        }

        /// <inheritdoc />
        public async Task<MetadataDefinitionDto> HandleResult(ChangePropertyTypeAndDefaultValueCommand command, CancellationToken token)
        {
            var definition = _dynamicMetadataDefinitonService.GetById(command.DefinitionId);

            if (definition == null)
            {
                throw new NotFoundException();
            }

            var existingProperty = definition.Properties.SingleOrDefault(x => x.Id == command.PropertyKey);

            if (existingProperty == null)
            {
                throw new BadRequestException("Property doesn't already exist");
            }

            DynamicMetadataProperty property;

            try
            {
                property = _inputDataParser.DeserializeItem(command.PropertyKey, command.UpdatedObject.Type, command.UpdatedObject.DefaultValue);
            }
            catch (JsonException exception)
            {
                throw new AggregatedValidationException("Invalid type mismatch")
                {
                    ErrorCode = 11231,
                    ValidationErrors = new List<Error>()
                };
            }

            // parsing property
            definition.Properties.Remove(existingProperty);
            definition.Properties.Add(property);

            _dynamicMetadataDefinitionWriteService.UpdateItem(definition, token);

            await _eventDispatcher.DispatchEvent(new PropertyDefinitionUpdatedEvent
            {
                UpdatedPropertyKey = property.Id,
                DefinitionId = command.DefinitionId
            });

            return _mapper.Map<MetadataDefinitionDto>(definition);
        }
    }
}
