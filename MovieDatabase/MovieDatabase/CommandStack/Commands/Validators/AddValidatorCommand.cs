using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.Validators;

namespace MovieDatabase.CommandStack.Commands.Validators
{
    public class AddValidatorCommand : ICommand
    {
        public string DefinitionId { get; set; }
        public AddValidatorDto ValidatorData { get; set; }
    }
}
