using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Dto.Validators
{
    public class ValidatorConfigurationDto
    {
        public string ValidatorId { get; set; }
        public string ValidatorKey { get; set; }

        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
