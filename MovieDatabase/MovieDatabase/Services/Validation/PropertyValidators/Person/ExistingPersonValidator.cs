using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Services.Validation.PropertyValidators.Person
{
    public class ExistingPersonValidator : IValidator
    {
        private readonly IReadService<Model.Person> _personReadService;

        public ExistingPersonValidator(IReadService<Model.Person> personReadService)
        {
            _personReadService = personReadService;
        }

        /// <inheritdoc />
        public string Key { get; } = "existing-person";
        public string Property { get; set; }

        /// <inheritdoc />
        public List<Error> Validate(DynamicMetadata dynamicMetadata, Movie context)
        {
            var property = dynamicMetadata.Properties.First(x => x.Id == Property);


            if (property.Value != null)
            {
                Model.Person value = (Model.Person)property.Value;
                var dbItem = _personReadService.GetById(value.Id);

                if (dbItem == null)
                {
                    return new List<Error>
                    {
                        new Error(23123123, "Person doesnt exist")
                    };
                }
            }

            return new List<Error>();
        }

        /// <inheritdoc />
        public void ConfigureValidator(Dictionary<string, string> validatorKeyParameters)
        {
            Property = validatorKeyParameters["property-key"];
        }

        /// <inheritdoc />
        public bool WillUseProperty(string propertyKey)
        {
            return false;
        }

        /// <param name="definition"></param>
        /// <inheritdoc />
        public bool IsValidValidator(DynamicMetadataDefinition definition)
        {
            var property = definition.Properties.FirstOrDefault(x => x.Id == Property);


            return property != null;
        }
    }
}
