using WorkyOne.Contracts.Repositories.Result;

namespace WorkyOne.Contracts.Interfaces
{
    /// <summary>
    /// Интерфейс объекта, описывающего результат выполнения какого-то действия
    /// </summary>
    public interface IResult
    {
        public bool IsSucceed { get; }

        public List<ResultItem> SucceedItems { get; }

        public List<ResultItem> ErrorItems { get; }
    }
}
