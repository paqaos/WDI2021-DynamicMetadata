using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Model.PropertyConfiguration
{
    public class NumericProperty : DynamicMetadataProperty<int>
    {
        /// <inheritdoc />
        public override string Type { get; } = "numeric";
    }
}
