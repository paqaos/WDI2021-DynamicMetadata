using System;
using System.Collections.Generic;
using MovieDatabase.Model;

namespace MovieDatabase.Exceptions
{
    public class AggregatedValidationException : Exception, IErrorCodeException
    {

        public AggregatedValidationException(string message) : base(message)
        {

        }

        public IReadOnlyList<Error> ValidationErrors { get; set; }

        /// <inheritdoc />
        public int ErrorCode { get; set; }
    }
}
