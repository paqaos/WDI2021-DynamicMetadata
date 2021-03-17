using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.CommandStack.Commands.MovieMetadatas
{
    public class DeleteMovieMetadataCommand : ICommand
    {
        public string MovieId   { get; set; }
        public string MetdataId { get; set; }
    }
}
