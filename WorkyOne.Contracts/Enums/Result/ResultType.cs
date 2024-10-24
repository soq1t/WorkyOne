using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Contracts.Enums.Result
{
    public enum ResultType
    {
        Created = 1,
        Updated = 2,
        Deleted = 3,

        NotFound = 4,
        AlreadyExisted = 5
    }
}
