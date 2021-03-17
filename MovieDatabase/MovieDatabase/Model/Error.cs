using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Extensions;

namespace MovieDatabase.Model
{
    public class Error
    {
        public Error(int code, string message) 
        {
            Code = code;
            Message = message;

        }

        public List<Error> InternalErrors { get; set; }
        public int    Code    { get; protected set; }
        public string Message { get; protected set; }
    }
}
