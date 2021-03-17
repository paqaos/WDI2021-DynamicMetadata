using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Model.PropertyConfiguration
{
    public class PersonProperty : DynamicMetadataProperty<Person>
    {
        /// <inheritdoc />
        public override string Type { get; } = "person";
    }
}
