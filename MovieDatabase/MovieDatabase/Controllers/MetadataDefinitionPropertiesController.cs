using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.CommandHandlers;
using MovieDatabase.CommandStack.Commands.MetadataDefinitions;
using MovieDatabase.Dto.MetadataDefinitions;

namespace MovieDatabase.Controllers
{
    [Route("metadata-definitions/{definitionId}/properties")]
    public class MetadataDefinitionPropertiesController : Controller
    {
        private readonly ICommandHandler<AddMetadataDefinitionPropertyCommand, MetadataDefinitionDto> _addMetadataDefinitionPropertyCommandHandler;
        private readonly ICommandHandler<RemoveMovieMetadataCommand, MetadataDefinitionDto> _removeMovieMetadataCommandHandler;
        private readonly ICommandHandler<ChangePropertyTypeAndDefaultValueCommand, MetadataDefinitionDto>
            _changePropertyTypeCommandHandler;

        public MetadataDefinitionPropertiesController(ICommandHandler<AddMetadataDefinitionPropertyCommand, MetadataDefinitionDto> addMetadataDefinitionPropertyCommandHandler, ICommandHandler<RemoveMovieMetadataCommand, MetadataDefinitionDto> removeMovieMetadataCommandHandler, ICommandHandler<ChangePropertyTypeAndDefaultValueCommand, MetadataDefinitionDto> changePropertyTypeCommandHandler)
        {
            _addMetadataDefinitionPropertyCommandHandler = addMetadataDefinitionPropertyCommandHandler;
            _removeMovieMetadataCommandHandler = removeMovieMetadataCommandHandler;
            _changePropertyTypeCommandHandler = changePropertyTypeCommandHandler;
        }

        [HttpPost]
        public async Task<MetadataDefinitionDto> AddMetadataDefinitionPropertyAsync([FromBody] CreateMetadataPropertyDefinitionDto definitionDto, string definitionId, CancellationToken ct)
        {
            return await _addMetadataDefinitionPropertyCommandHandler.HandleResult(
                new AddMetadataDefinitionPropertyCommand
                {
                    DefinitionId = definitionId,
                    NewPropertyData = definitionDto
                }, ct);
        }

        [HttpDelete("{key}")]
        public async Task<MetadataDefinitionDto> RemoveMetadataDefinitionPropertyAsync(
            string definitionId, string key, CancellationToken ct)
        {
            return await _removeMovieMetadataCommandHandler.HandleResult(new RemoveMovieMetadataCommand
            {
                MetadataDefinitionId = definitionId,
                MetadataKey = key
            }, ct);
        }

        [HttpPut("{key}")]
        public async Task<MetadataDefinitionDto> UpdatePropertyAsync(string definitionId, string key,
                                                                     [FromBody]
                                                                     UpdateMetadataPropertyDefinitionDto contentToUpdate,
                                                                     CancellationToken ct)
        {
            return await _changePropertyTypeCommandHandler.HandleResult(new ChangePropertyTypeAndDefaultValueCommand
            {
                UpdatedObject = contentToUpdate,
                PropertyKey = key,
                DefinitionId = definitionId
            }, ct);
        }
    }
}
