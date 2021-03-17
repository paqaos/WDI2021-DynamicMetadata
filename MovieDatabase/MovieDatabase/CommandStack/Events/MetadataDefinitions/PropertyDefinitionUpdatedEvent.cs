using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Services.EventProcessing;

namespace MovieDatabase.CommandStack.Events.MetadataDefinitions
{
    public class PropertyDefinitionUpdatedEvent : EventBase
    {
        /// <inheritdoc />
        public override string Name => "property-updated";

        public string DefinitionId       { get; set; }
        public string UpdatedPropertyKey { get; set; }
    }
}
