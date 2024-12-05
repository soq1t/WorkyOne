namespace WorkyOne.Contracts.Services.GetRequests.Common
{
    /// <summary>
    /// Ответ, присылаемый пагинированным запросом
    /// </summary>
    /// <typeparam name="T">Тип объекта, который присылается в ответе</typeparam>
    public class PaginatedResponse<T>
        where T : class
    {
        public T? Value { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageAmount { get; set; }
    }
}
