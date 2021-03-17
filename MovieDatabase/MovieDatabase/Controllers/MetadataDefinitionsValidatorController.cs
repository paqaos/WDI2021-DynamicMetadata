using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.CommandHandlers;
using MovieDatabase.CommandStack.Commands.MetadataDefinitions;
using MovieDatabase.CommandStack.Commands.Validators;
using MovieDatabase.Dto.MetadataDefinitions;
using MovieDatabase.Dto.Validators;

namespace MovieDatabase.Controllers
{
    [Route("metadata-definitions/{definitionId}/validators")]
    public class MetadataDefinitionsValidatorController : Controller
    {
        private readonly ICommandHandler<AddValidatorCommand, MetadataDefinitionDto> _addValidatorCommandHandler;
        private readonly ICommandHandler<RemoveValidatorCommand, MetadataDefinitionDto> _removeValidatorCommandHandler;
        private readonly ICommandHandler<UpdateValidatorConfigurationCommand, MetadataDefinitionDto> _updateValidatorConfigurationCommandHandler;

        /// <inheritdoc />
        public MetadataDefinitionsValidatorController(ICommandHandler<AddValidatorCommand, MetadataDefinitionDto> addValidatorCommandHandler, ICommandHandler<RemoveValidatorCommand, MetadataDefinitionDto> removeValidatorCommandHandler, ICommandHandler<UpdateValidatorConfigurationCommand, MetadataDefinitionDto> updateValidatorConfigurationCommandHandler)
        {
            _addValidatorCommandHandler = addValidatorCommandHandler;
            _removeValidatorCommandHandler = removeValidatorCommandHandler;
            _updateValidatorConfigurationCommandHandler = updateValidatorConfigurationCommandHandler;
        }

        [HttpPost]
        public async Task<MetadataDefinitionDto> AddValidatorAsync(string definitionId,
                                                                   [FromBody] AddValidatorDto validatorData,
                                                                   CancellationToken ct)
        {
            return await _addValidatorCommandHandler.HandleResult(new AddValidatorCommand
            {
                DefinitionId = definitionId,
                ValidatorData = validatorData
            }, ct);
        }

        [HttpPut("{validatorId}")]
        public async Task<MetadataDefinitionDto> UpdateValidatorAsync(string definitionId, string validatorId,
                                                                      [FromBody]
                                                                      UpdateValidatorConfigurationDto
                                                                          validatorConfiguration, CancellationToken ct)
        {
            return await _updateValidatorConfigurationCommandHandler.HandleResult(
                new UpdateValidatorConfigurationCommand
                {
                    ValidatorId = validatorId, ValidatorConfiguration = validatorConfiguration,
                    DefinitionId = definitionId
                }, ct);
        }

        [HttpDelete("{validatorId}")]
        public async Task<MetadataDefinitionDto> RemoveValidatorAsync(string definitionId, string validatorId,
                                                                      CancellationToken ct)
        {
            return await _removeValidatorCommandHandler.HandleResult(new RemoveValidatorCommand
            {
                ValidatorId = validatorId,
                DefinitionId = definitionId
            }, ct);
        }
    }
}
