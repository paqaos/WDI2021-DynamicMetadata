using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Services.Validation
{
    public class CompositeValidator : IValidator
    {
        /// <inheritdoc />
        public string Key { get; } = "composite";

        private readonly List<IValidator> _validators = new List<IValidator>();

        public void AddValidator(IValidator validator)
        {
            _validators.Add(validator);
        }

        /// <inheritdoc />
        public List<Error> Validate(DynamicMetadata dynamicMetadata, Movie context)
        {
            List<Error> errors = new List<Error>();

            foreach(var validator in _validators)
            {
                errors.AddRange(validator.Validate(dynamicMetadata, context));
            }

            return errors;
        }

        /// <inheritdoc />
        public void ConfigureValidator(Dictionary<string, string> validatorKeyParameters)
        {
            
        }

        /// <inheritdoc />
        public bool WillUseProperty(string propertyKey)
        {
            bool willUse = false;
            foreach (var validator in _validators)
            {
                willUse = validator.WillUseProperty(propertyKey) || willUse;
            }

            return willUse;
        }

        /// <param name="definition"></param>
        /// <inheritdoc />
        public bool IsValidValidator(DynamicMetadataDefinition definition)
        {
            bool valid = true;
            foreach (var validator in _validators)
            {
                valid = valid && validator.IsValidValidator(definition);
            }
            return valid;
        }
    }
}
