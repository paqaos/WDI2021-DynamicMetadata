using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.DataBuilder
{
    public class ValidatorBuilder
    {
        private string _validatorKey;
        private Dictionary<string, string> _configuration = new Dictionary<string, string>();

        public ValidatorConfiguration Build()
        {
            if(string.IsNullOrEmpty(_validatorKey))
                throw new Exception("Invalid validator in dataseed");

            return new ValidatorConfiguration
            {
                ValidatorId = Guid.NewGuid().ToString(),
                ValidatorKey= _validatorKey,
                Parameters = _configuration
            };
        }

        public ValidatorBuilder UseKey(string validatorKey)
        {
            _validatorKey = validatorKey;

            return this;
        }

        public ValidatorBuilder UseValidatorConfiguration(string property, string value)
        {
            if (string.IsNullOrEmpty(property))
                throw new Exception("no id of property defined");

            _configuration[property] = value;

            return this;
        }
    }
}
