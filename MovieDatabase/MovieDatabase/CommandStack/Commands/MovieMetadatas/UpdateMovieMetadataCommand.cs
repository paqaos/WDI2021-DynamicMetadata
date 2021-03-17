using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.MovieMetadatas;

namespace MovieDatabase.CommandStack.Commands.MovieMetadatas
{
    public class UpdateMovieMetadataCommand : ICommand
    {
        public string MovieId { get; set; }
        public string MetdataId { get; set; }
        public UpdateMovieMetadataDto Metadata { get; set; }
    }
}
