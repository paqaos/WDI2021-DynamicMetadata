using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.MetadataDefinitions;

namespace MovieDatabase.CommandStack.Commands.MetadataDefinitions
{
    public class AddMetadataDefinitionPropertyCommand : ICommand
    {
        public CreateMetadataPropertyDefinitionDto NewPropertyData { get; set; }
        public string DefinitionId { get; set; }
    }
}
