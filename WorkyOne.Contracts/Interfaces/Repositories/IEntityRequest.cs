namespace WorkyOne.Contracts.Interfaces.Repositories
{
    /// <summary>
    /// Интрерфейс запроса на получение сущности из базы данных
    /// </summary>
    public interface IEntityRequest
    {
        /// <summary>
        /// Идентификатор запрашиваемой сущности
        /// </summary>
        public string Id { get; set; }
    }
}
