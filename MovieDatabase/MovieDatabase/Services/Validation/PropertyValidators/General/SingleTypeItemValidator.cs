using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Constants;
using MovieDatabase.Extensions;
using MovieDatabase.Model;

namespace MovieDatabase.Services.Validation.PropertyValidators.Person
{
    public class SingleTypeItemValidator : IValidator
    {
        /// <inheritdoc />
        public string Key { get; } = "single-type-item";

        /// <inheritdoc />
        public List<Error> Validate(DynamicMetadata dynamicMetadata, Movie context)
        {
            var existingItems = context.Metadata.Where(x => x.DefinitionId == dynamicMetadata.DefinitionId).ToList();

            bool isApplied = true;
            foreach (var item in existingItems)
            {
                if (item.Id != dynamicMetadata.Id)
                {
                    isApplied = false;
                }
            }

            if (!isApplied && existingItems.Any())
            {
                return new List<Error>
                {
                    new Error((int)ErrorCodes.GenericValidator.ReachedMaxItemsOfThisType, ErrorCodes.GenericValidator.ReachedMaxItemsOfThisType.GetDescription())
                };
            }

            return new List<Error>();
        }

        /// <inheritdoc />
        public void ConfigureValidator(Dictionary<string, string> validatorKeyParameters)
        {
        }

        /// <inheritdoc />
        public bool WillUseProperty(string propertyKey)
        {
            return false;
        }

        /// <inheritdoc />
        public bool IsValidValidator(DynamicMetadataDefinition definition)
        {
            return true;
        }
    }
}
