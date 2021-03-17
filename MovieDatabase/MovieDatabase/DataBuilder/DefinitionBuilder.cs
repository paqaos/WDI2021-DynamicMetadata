using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.DataBuilder
{
    public class DefinitionBuilder
    {
        private string _id;
        private string _name;
        private List<ValidatorBuilder> _validators;
        private List<PropertyBuilder> _propertyBuilders;

        public DefinitionBuilder(string id, string name)
        {
            _id = id;
            _name = name;
            _validators = new List<ValidatorBuilder>();
            _propertyBuilders = new List<PropertyBuilder>();
        }

        public DefinitionBuilder AddValidator(Action<ValidatorBuilder> validatorConfiguration)
        {
            var validator = new ValidatorBuilder();
            validatorConfiguration(validator);

            _validators.Add(validator);

            return this;
        }

        public DefinitionBuilder AddProperty<TProperty>(string id, Action<TProperty> propertyDefinition) where TProperty : PropertyBuilder, new()
        {
            var propertyBuilder = new TProperty();
            propertyBuilder.Id = id;
            propertyDefinition(propertyBuilder);

            _propertyBuilders.Add(propertyBuilder);

            return this;
        }

        public DynamicMetadataDefinition Build()
        {
            return new DynamicMetadataDefinition
            {
                Id = _id,
                Name = _name,
                Validators = _validators.Select(x => x.Build()).ToList(),
                Properties = _propertyBuilders.Select(x => x.Build()).ToList()
            };
        }
    }
}
