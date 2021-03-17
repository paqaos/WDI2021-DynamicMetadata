using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Services.Validation.PropertyValidators.General
{
    public class NotNullValidator : IValidator
    {
        /// <inheritdoc />
        public string Key { get; } = "not-null";
        public string Property { get; set; }

        /// <inheritdoc />
        public List<Error> Validate(DynamicMetadata dynamicMetadata, Movie context)
        {
            var property = dynamicMetadata.Properties.First(x => x.Id == Property);

            if (property.Value != null)
            {
                return new List<Error>();
            }

            return new List<Error>
            {
                new Error(102312, "Null value")
            };
        }

        /// <inheritdoc />
        public void ConfigureValidator(Dictionary<string, string> validatorKeyParameters)
        {
            Property = validatorKeyParameters["property-key"];
        }

        /// <inheritdoc />
        public bool WillUseProperty(string propertyKey)
        {
            return propertyKey != Key;
        }

        /// <inheritdoc />
        public bool IsValidValidator(DynamicMetadataDefinition definition)
        {
            return definition.Properties.FirstOrDefault(x => x.Id == Property) != null;
        }
    }
}
