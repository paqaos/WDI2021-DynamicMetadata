using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Operations;

namespace MovieDatabase.Model.EventSourcing
{
    public class Commit : DatabaseItem
    {
        /// <inheritdoc />
        public override string Type { get; } = "Commit";

        public string MovieId { get; set; }
        public List<IMovieOperation> Operations { get; set; }
        public int CommitOrder { get; set; }
    }
}
