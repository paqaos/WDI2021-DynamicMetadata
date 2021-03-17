using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Model
{
    public class DynamicMetadata
    {
        public IList<DynamicMetadataProperty> Properties { get; set; } = new List<DynamicMetadataProperty>();

        public string DefinitionId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public bool IsValid { get; set; }
    }
}
