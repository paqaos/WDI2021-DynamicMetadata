using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.Commands.MetadataDefinitions;
using MovieDatabase.CommandStack.Events.MetadataDefinitions;
using MovieDatabase.Dto;
using MovieDatabase.Dto.MetadataDefinitions;
using MovieDatabase.Exceptions;
using MovieDatabase.Model;
using MovieDatabase.Services;
using MovieDatabase.Services.EventProcessing;
using MovieDatabase.Services.MetadataObject;

namespace MovieDatabase.CommandStack.CommandHandlers.MetadataDefinitions
{
    public class AddMetadataDefinitionPropertyCommandHandler : ICommandHandler<AddMetadataDefinitionPropertyCommand, MetadataDefinitionDto>
    {
        private readonly IReadService<DynamicMetadataDefinition> _dynamicMetadataDefinitonService;
        private readonly IWriteService<DynamicMetadataDefinition> _dynamicMetadataDefinitionWriteService;
        private readonly IInputDataParser _inputDataParser;
        private readonly IMapper _mapper;
        private readonly IEventDispatcher _eventDispatcher;

        public AddMetadataDefinitionPropertyCommandHandler(IReadService<DynamicMetadataDefinition> dynamicMetadataDefinitonService, IMapper mapper, IWriteService<DynamicMetadataDefinition> dynamicMetadataDefinitionWriteService, IInputDataParser inputDataParser, IEventDispatcher eventDispatcher)
        {
            _dynamicMetadataDefinitonService = dynamicMetadataDefinitonService;
            _mapper = mapper;
            _dynamicMetadataDefinitionWriteService = dynamicMetadataDefinitionWriteService;
            _inputDataParser = inputDataParser;
            _eventDispatcher = eventDispatcher;
        }

        /// <inheritdoc />
        public async Task<MetadataDefinitionDto> HandleResult(AddMetadataDefinitionPropertyCommand command, CancellationToken token)
        {
            var definition = _dynamicMetadataDefinitonService.GetById(command.DefinitionId);

            if (definition == null)
            {
                throw new NotFoundException();
            }

            var existingProperty = definition.Properties.SingleOrDefault(x => x.Id == command.NewPropertyData.Key);

            DynamicMetadataProperty property;

            if (existingProperty != null)
            {
                throw new BadRequestException("Property already exist");
            }

            try
            {
                property = _inputDataParser.DeserializeItem(command.NewPropertyData.Key, command.NewPropertyData.Type, command.NewPropertyData.DefaultValue);
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
            definition.Properties.Add(property);
            _dynamicMetadataDefinitionWriteService.UpdateItem(definition, token);

            await _eventDispatcher.DispatchEvent(new PropertyAddedToMetadataDefinitionEvent
            {
                AddedPropertyId = property.Id,
                DefinitionId = command.DefinitionId
            });

            return _mapper.Map<MetadataDefinitionDto>(definition);
        }
    }
}
