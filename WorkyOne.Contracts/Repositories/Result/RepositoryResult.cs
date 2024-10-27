using System.Collections.ObjectModel;

namespace WorkyOne.Contracts.Repositories.Result
{
    public class RepositoryResult
    {
        private readonly List<ResultItem> _succeed = [];
        private readonly List<ResultItem> _errors = [];
        public bool IsSucceed => _succeed.Count > 0;

        public List<string>? Succeed =>
            (_succeed.Count > 0) ? _succeed.Select(x => x.Message).ToList() : null;
        public List<string>? Errors =>
            (_errors.Count > 0) ? _errors.Select(x => x.Message).ToList() : null;

        public void AddError(ResultItem item)
        {
            _errors.Add(item);
        }

        public void AddSucceed(ResultItem item)
        {
            _succeed.Add(item);
        }
    }
}
