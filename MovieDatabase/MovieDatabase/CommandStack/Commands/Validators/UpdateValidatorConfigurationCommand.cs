using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.Validators;

namespace MovieDatabase.CommandStack.Commands.Validators
{
    public class UpdateValidatorConfigurationCommand : ICommand
    {
        public string DefinitionId { get; set; }
        public string ValidatorId { get; set; }
        public UpdateValidatorConfigurationDto ValidatorConfiguration { get; set; }
    }
}
