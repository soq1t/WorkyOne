using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Interfaces;

namespace WorkyOne.Contracts.Repositories.Result
{
    public class RepositoryResult : IResult
    {
        public bool IsSucceed => SucceedItems.Count > 0;

        public List<ResultItem> SucceedItems => throw new NotImplementedException();

        public List<ResultItem> ErrorItems => throw new NotImplementedException();
    }
}
