using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Model
{
    public class DynamicMetadataDefinition : DatabaseItem
    {
        /// <inheritdoc />
        public override string Type { get; } = "DynamicMetadata";

        public string Name { get; set; }

        public IList<DynamicMetadataProperty> Properties { get; set; }

        public List<ValidatorConfiguration> Validators { get; set; }
    }
}
