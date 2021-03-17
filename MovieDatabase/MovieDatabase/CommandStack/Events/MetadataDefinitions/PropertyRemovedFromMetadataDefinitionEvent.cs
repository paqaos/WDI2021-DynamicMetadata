using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Services.EventProcessing;

namespace MovieDatabase.CommandStack.Events.MetadataDefinitions
{
    public class PropertyRemovedFromMetadataDefinitionEvent : EventBase
    {
        public override string Name { get; set; } = "property-removed";

        public string DefinitionId    { get; set; }
        public string RemovedPropertyKey { get; set; }
    }
}
