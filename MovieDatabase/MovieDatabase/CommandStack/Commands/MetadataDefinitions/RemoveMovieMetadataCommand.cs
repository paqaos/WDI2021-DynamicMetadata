using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.CommandStack.Commands.MetadataDefinitions
{
    public class RemoveMovieMetadataCommand : ICommand
    {
        public string MetadataKey { get; set; }
        public string MetadataDefinitionId { get; set; }
    }
}
