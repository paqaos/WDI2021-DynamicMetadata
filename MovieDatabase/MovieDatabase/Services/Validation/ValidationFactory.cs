using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;
using SimpleInjector;

namespace MovieDatabase.Services.Validation
{
    public class ValidationFactory : IValidationFactory
    {
        private readonly Container _container;
        private readonly List<IValidator> _validators;

        public ValidationFactory(List<IValidator> validators, Container container)
        {
            _validators = validators;
            _container = container;
        }

        public IValidator GetValidator(DynamicMetadataDefinition definition)
        {
            var compositeValidator = new CompositeValidator();


            foreach (var validatorKey in definition.Validators)
            {
                var validator = _validators.First(x => x.Key == validatorKey.ValidatorKey);

                var validatorType = validator.GetType();

                var fromContainer = (IValidator) _container.GetInstance(validatorType);

                fromContainer.ConfigureValidator(validatorKey.Parameters);

                compositeValidator.AddValidator(fromContainer);
            }
            return compositeValidator;
        }

        /// <inheritdoc />
        public bool CreateValidatorFor(ValidatorConfiguration validatorConfiguration, DynamicMetadataDefinition definition)
        {
            var validator = _validators.FirstOrDefault(x => x.Key == validatorConfiguration.ValidatorKey);

            var validatorType = validator.GetType();

            var fromContainer = (IValidator)_container.GetInstance(validatorType);
            fromContainer.ConfigureValidator(validatorConfiguration.Parameters);

            return fromContainer.IsValidValidator(definition);
        }
    }
}
