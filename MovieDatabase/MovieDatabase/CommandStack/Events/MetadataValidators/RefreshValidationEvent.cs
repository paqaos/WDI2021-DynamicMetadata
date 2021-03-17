using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Services.EventProcessing;

namespace MovieDatabase.CommandStack.Events.MetadataValidators
{
    public class RefreshValidationEvent : EventBase
    {
        public string DefinitionId { get; set; }
    }
}
