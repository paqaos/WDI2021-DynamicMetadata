using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Constants
{
    public static class ErrorCodes
    {
        public enum GenericValidator
        {
            [Description("There is maximum number of this entry")]
            ReachedMaxItemsOfThisType = 1000
        }

        public enum NumericPropertyValidator
        {
            [Description("Number exceeds maximum value")]
            MaxValueExceeded = 1100
        }
    }
}
