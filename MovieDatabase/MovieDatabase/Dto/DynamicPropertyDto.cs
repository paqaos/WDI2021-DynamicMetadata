using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MovieDatabase.Constants;

namespace MovieDatabase.Dto
{
    public class DynamicPropertyDto
    {
        public  string Id   { get; set; }
        public  string Type { get; set; }

        public object Value { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PropertyState State { get; set; }
        public bool HasDefaultValue { get; set; }
    }
}
