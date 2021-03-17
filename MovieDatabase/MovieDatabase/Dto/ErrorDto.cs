using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Dto
{
    public class ErrorDto
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public List<ErrorDto> InternalErrors { get; set; } = new List<ErrorDto>();
    }
}
