using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Services.MetadataObject
{
    public interface IInputDataParser
    {
        DynamicMetadataProperty DeserializeItem(string propertyId, string type, object value);
    }
}
