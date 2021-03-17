using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Exceptions
{
    public interface IErrorCodeException
    {
        public int ErrorCode { get; set; }
    }
}
