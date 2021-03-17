using System.Collections.Generic;
using MovieDatabase.Model;

namespace MovieDatabase.Services.Validation
{
    public interface IValidator
    {
        string Key { get; }
        List<Error> Validate(DynamicMetadata dynamicMetadata, Movie context);
        void ConfigureValidator(Dictionary<string, string> validatorKeyParameters);
        bool WillUseProperty(string propertyKey);
        bool IsValidValidator(DynamicMetadataDefinition definition);
    }
}
