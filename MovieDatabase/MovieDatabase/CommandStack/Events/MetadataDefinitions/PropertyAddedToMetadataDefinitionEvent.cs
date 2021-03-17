using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Services.EventProcessing;

namespace MovieDatabase.CommandStack.Events.MetadataDefinitions
{
    public class PropertyAddedToMetadataDefinitionEvent : EventBase
    {
        /// <inheritdoc />
        public override string Name { get; set; } = "property-added";

        public string DefinitionId { get; set; }
        public string AddedPropertyId { get; set; }
    }
}
