using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MovieDatabase.Dto;
using MovieDatabase.Model;
using MovieDatabase.Model.PropertyConfiguration;

namespace MovieDatabase.Services.MetadataObject
{
    public class InputDataParser : IInputDataParser
    {
        /// <inheritdoc />
        public DynamicMetadataProperty DeserializeItem(string id, string type, object value)
        {
            switch (type)
            {
                case "number":
                {
                    if (value == null)
                        return new NumericProperty
                        {
                            WrappedValue = 0,
                            Id = id
                        };

                    var item = (JsonElement) value;

                    return new NumericProperty
                    {
                        WrappedValue = item.GetInt32(),
                        Id = id
                    };
                }

                case "string":
                {
                    if (value == null)
                        return new StringProperty
                        {
                            WrappedValue = null,
                            Id = id
                        };

                        var item = (JsonElement) value;

                    return new StringProperty
                    {
                        WrappedValue = item.GetString(),
                        Id = id
                    };
                }

                case "person":
                {
                    if (value == null)
                        return new PersonProperty { Id = id, WrappedValue = null };
                    var item = (JsonElement) value;
                    var inputData = JsonSerializer.Deserialize<PersonDto>(item.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return new PersonProperty
                    {
                        Id = id,
                        WrappedValue = new Person
                        {
                            Id = inputData.Id,
                            FirstName = inputData.FirstName,
                            LastName = inputData.LastName
                        }
                    };
                }
            }
            throw new NotImplementedException();
        }
    }
}
