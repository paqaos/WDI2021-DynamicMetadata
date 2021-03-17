using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Dto.Movies
{
    public class CreateMovieDto
    {
        public string Name { get; set; }
        public List<MovieGenre> Genres { get; set; }
    }
}
