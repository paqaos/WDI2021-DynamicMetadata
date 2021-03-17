using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Model
{
    public class Person : DatabaseItem
    {
        /// <inheritdoc />
        public override string Type { get; } = "Person";

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
