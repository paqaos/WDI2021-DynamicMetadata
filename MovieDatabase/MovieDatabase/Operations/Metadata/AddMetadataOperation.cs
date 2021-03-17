using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Operations.Metadata
{
    public class AddMetadataOperation : MovieOperationBase
    {
        public DynamicMetadata MetadataContent;
        /// <inheritdoc />
        public override Movie ExecuteOperation(Movie movie)
        {
            movie.Metadata.Add(MetadataContent);

            return base.ExecuteOperation(movie);
        }
    }
}
