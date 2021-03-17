using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Services.Validation
{
    public interface IValidationFactory
    {
        IValidator GetValidator(DynamicMetadataDefinition definition);
        bool CreateValidatorFor(ValidatorConfiguration validator, DynamicMetadataDefinition definition);
    }
}
