using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Exceptions
{
    public class NotFoundException : Exception, IErrorCodeException
    {
        /// <inheritdoc />
        public int ErrorCode { get; set; }
    }
}
