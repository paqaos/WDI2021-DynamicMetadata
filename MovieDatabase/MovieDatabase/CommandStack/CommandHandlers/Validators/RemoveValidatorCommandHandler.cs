using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using MovieDatabase.CommandStack.Commands.Validators;
using MovieDatabase.CommandStack.Events.MetadataValidators;
using MovieDatabase.Dto.MetadataDefinitions;
using MovieDatabase.Exceptions;
using MovieDatabase.Model;
using MovieDatabase.Services;
using MovieDatabase.Services.EventProcessing;

namespace MovieDatabase.CommandStack.CommandHandlers.Validators
{
    public class RemoveValidatorCommandHandler : ICommandHandler<RemoveValidatorCommand, MetadataDefinitionDto>
    {
        private readonly IReadService<DynamicMetadataDefinition>  _dynamicMetadataDefinitionService;
        private readonly IWriteService<DynamicMetadataDefinition> _dynamicMetadataDefinitionWriteService;
        private readonly IEventDispatcher                         _eventDispatcher;
        private readonly IMapper _mapper;

        public RemoveValidatorCommandHandler(IReadService<DynamicMetadataDefinition> dynamicMetadataDefinitionService, IWriteService<DynamicMetadataDefinition> dynamicMetadataDefinitionWriteService, IEventDispatcher eventDispatcher, IMapper mapper)
        {
            _dynamicMetadataDefinitionService = dynamicMetadataDefinitionService;
            _dynamicMetadataDefinitionWriteService = dynamicMetadataDefinitionWriteService;
            _eventDispatcher = eventDispatcher;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<MetadataDefinitionDto> HandleResult(RemoveValidatorCommand command, CancellationToken token)
        {
            var definition = _dynamicMetadataDefinitionService.GetById(command.DefinitionId);

            if(definition == null)
                throw new NotFoundException();

            var validator = definition.Validators.FirstOrDefault(x => x.ValidatorId == command.ValidatorId);
            if(validator == null)
                throw new NotFoundException();

            definition.Validators.Remove(validator);
            _dynamicMetadataDefinitionWriteService.UpdateItem(definition, token);

            await _eventDispatcher.DispatchEvent(new RefreshValidationEvent
            {
                DefinitionId = definition.Id
            });

            return _mapper.Map<MetadataDefinitionDto>(definition);
        }
    }
}
