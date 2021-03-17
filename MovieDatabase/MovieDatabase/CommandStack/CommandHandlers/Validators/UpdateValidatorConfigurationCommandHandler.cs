using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.Commands.Validators;
using MovieDatabase.CommandStack.Events.MetadataValidators;
using MovieDatabase.Dto.MetadataDefinitions;
using MovieDatabase.Exceptions;
using MovieDatabase.Model;
using MovieDatabase.Services;
using MovieDatabase.Services.EventProcessing;
using MovieDatabase.Services.Validation;

namespace MovieDatabase.CommandStack.CommandHandlers.Validators
{
    public class UpdateValidatorConfigurationCommandHandler : ICommandHandler<UpdateValidatorConfigurationCommand, MetadataDefinitionDto>
    {
        private readonly IReadService<DynamicMetadataDefinition>  _dynamicMetadataDefinitionService;
        private readonly IWriteService<DynamicMetadataDefinition> _dynamicMetadataDefinitionWriteService;
        private readonly IEventDispatcher                         _eventDispatcher;
        private readonly IMapper                                  _mapper;
        private readonly IValidationFactory _validationFactory;

        public UpdateValidatorConfigurationCommandHandler(IReadService<DynamicMetadataDefinition> dynamicMetadataDefinitionService, IWriteService<DynamicMetadataDefinition> dynamicMetadataDefinitionWriteService, IEventDispatcher eventDispatcher, IMapper mapper, IValidationFactory validationFactory)
        {
            _dynamicMetadataDefinitionService = dynamicMetadataDefinitionService;
            _dynamicMetadataDefinitionWriteService = dynamicMetadataDefinitionWriteService;
            _eventDispatcher = eventDispatcher;
            _mapper = mapper;
            _validationFactory = validationFactory;
        }

        /// <inheritdoc />
        public async Task<MetadataDefinitionDto> HandleResult(UpdateValidatorConfigurationCommand command, CancellationToken token)
        {
            var definition = _dynamicMetadataDefinitionService.GetById(command.DefinitionId);

            if (definition == null)
                throw new NotFoundException();

            var validator = definition.Validators.FirstOrDefault(x => x.ValidatorId == command.ValidatorId);
            if (validator == null)
                throw new NotFoundException();

            validator.Parameters = command.ValidatorConfiguration.Parameters;
            bool result = _validationFactory.CreateValidatorFor(validator, definition);

            if (!result)
                throw new BadRequestException("Validator has invalid configuration");

            _dynamicMetadataDefinitionWriteService.UpdateItem(definition, token);

            await _eventDispatcher.DispatchEvent(new RefreshValidationEvent
            {
                DefinitionId = definition.Id
            });

            return _mapper.Map<MetadataDefinitionDto>(definition);
        }
    }
}
