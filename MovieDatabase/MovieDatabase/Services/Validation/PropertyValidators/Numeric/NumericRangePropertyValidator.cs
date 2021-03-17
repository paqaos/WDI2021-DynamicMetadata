using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Constants;
using MovieDatabase.Extensions;
using MovieDatabase.Model;

namespace MovieDatabase.Services.Validation.PropertyValidators.Numeric
{
    public class NumericRangePropertyValidator : IValidator
    {
        /// <inheritdoc />
        public string Key { get; } = "numeric-range";

        public string Property { get; private set; }
        public int MaxRange { get; private set; }

        /// <inheritdoc />
        public List<Error> Validate(DynamicMetadata dynamicMetadata, Movie context)
        {
            var property = dynamicMetadata.Properties.First(x => x.Id == Property);

            int value = (int) property.Value;

            if (value > MaxRange)
            {
                return new List<Error>
                {
                    new Error((int)ErrorCodes.NumericPropertyValidator.MaxValueExceeded, ErrorCodes.NumericPropertyValidator.MaxValueExceeded.GetDescription())
                };
            }

            return new List<Error>();
        }

        /// <inheritdoc />
        public void ConfigureValidator(Dictionary<string, string> validatorKeyParameters)
        {
            Property = validatorKeyParameters["property-key"];
            MaxRange = int.Parse(validatorKeyParameters["max"]);

        }

        /// <inheritdoc />
        public bool WillUseProperty(string propertyKey)
        {
            return Property != Key;
        }

        /// <inheritdoc />
        public bool IsValidValidator(DynamicMetadataDefinition definition)
        {
            return definition.Properties.FirstOrDefault(x => x.Id == Property) != null;
        }
    }
}
