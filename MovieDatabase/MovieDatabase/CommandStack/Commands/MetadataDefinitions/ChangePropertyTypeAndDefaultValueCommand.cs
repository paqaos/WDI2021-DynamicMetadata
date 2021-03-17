using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.MetadataDefinitions;

namespace MovieDatabase.CommandStack.Commands.MetadataDefinitions
{
    public class ChangePropertyTypeAndDefaultValueCommand : ICommand
    {
        public string DefinitionId { get; set; }

        public string PropertyKey { get; set; }
        public UpdateMetadataPropertyDefinitionDto UpdatedObject { get; set; }
    }
}
