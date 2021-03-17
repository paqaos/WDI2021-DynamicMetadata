using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services.EventProcessing
{
    public class EventBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual string Name { get; set; }
    }
}
