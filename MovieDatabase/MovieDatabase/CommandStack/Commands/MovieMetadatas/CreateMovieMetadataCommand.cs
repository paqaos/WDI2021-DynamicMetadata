using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.MovieMetadatas;

namespace MovieDatabase.CommandStack.Commands.MovieMetadatas
{
    public class CreateMovieMetadataCommand : ICommand
    {
        public string MovieId { get; set; }
        public CreateMovieMetadataDto Metadata { get; set; }
    }
}
