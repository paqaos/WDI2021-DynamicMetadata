using System;
using System.Collections.Generic;
using System.Linq;
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
using MovieDatabase.Services.Validation;

namespace MovieDatabase.CommandStack.CommandHandlers.MetadataDefinitions
{
    public class RemoveMovieMetadataCommandHandler : ICommandHandler<RemoveMovieMetadataCommand, MetadataDefinitionDto>
    {
        private readonly IReadService<DynamicMetadataDefinition>  _dynamicMetadataDefinitonService;
        private readonly IWriteService<DynamicMetadataDefinition> _dynamicMetadataDefinitionWriteService;
        private readonly IMapper                                  _mapper;
        private readonly IEventDispatcher                         _eventDispatcher;
        private readonly IValidationFactory _validationFactory;

        public RemoveMovieMetadataCommandHandler(IReadService<DynamicMetadataDefinition> dynamicMetadataDefinitonService, IWriteService<DynamicMetadataDefinition> dynamicMetadataDefinitionWriteService, IMapper mapper, IEventDispatcher eventDispatcher, IValidationFactory validationFactory)
        {
            _dynamicMetadataDefinitonService = dynamicMetadataDefinitonService;
            _dynamicMetadataDefinitionWriteService = dynamicMetadataDefinitionWriteService;
            _mapper = mapper;
            _eventDispatcher = eventDispatcher;
            _validationFactory = validationFactory;
        }
        /// <inheritdoc />
        public async Task<MetadataDefinitionDto> HandleResult(RemoveMovieMetadataCommand command, CancellationToken token)
        {
            var definition = _dynamicMetadataDefinitonService.GetById(command.MetadataDefinitionId);

            if (definition == null)
            {
                throw new NotFoundException();
            }

            var existingProperty = definition.Properties.SingleOrDefault(x => x.Id == command.MetadataKey);

            if (existingProperty == null)
            {
                throw new NotFoundException();
            }

            var validator = _validationFactory.GetValidator(definition);
            if (validator.WillUseProperty(existingProperty.Id))
            {
                throw new BadRequestException("Property is used by validator")
                {
                    ErrorCode = 11,

                };
            }

            definition.Properties.Remove(existingProperty);
            _dynamicMetadataDefinitionWriteService.UpdateItem(definition, token);

            await _eventDispatcher.DispatchEvent(new PropertyRemovedFromMetadataDefinitionEvent
            {
                RemovedPropertyKey = command.MetadataKey,
                DefinitionId = command.MetadataDefinitionId
            });

            return _mapper.Map<MetadataDefinitionDto>(definition);
        }
    }
}
