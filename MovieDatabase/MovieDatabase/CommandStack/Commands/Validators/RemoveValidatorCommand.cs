using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.CommandStack.Commands.Validators
{
    public class RemoveValidatorCommand : ICommand
    {
        public string ValidatorId { get; set; }
        public string DefinitionId { get; set; }
    }
}
