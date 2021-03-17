using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Constants
{
    public enum PropertyState
    {
        Active = 0,

        MarkedAsDeleted = 1,
        Invalidated = 2,
        UsesOldType = 3
    }
}
