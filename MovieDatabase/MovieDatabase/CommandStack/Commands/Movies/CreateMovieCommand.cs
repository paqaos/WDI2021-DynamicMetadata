using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.Movies;

namespace MovieDatabase.CommandStack.Commands.Movies
{
    public class CreateMovieCommand : ICommand
    {
        public CreateMovieDto MovieData { get; set; }
    }
}
