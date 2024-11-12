namespace WorkyOne.Infrastructure.Exceptions.Stores
{
    public class AccessFilterNotExistsException : Exception
    {
        public AccessFilterNotExistsException(Type entityType)
            : base($"Не найден фильтра для сущности типа {entityType.Name}") { }
    }
}
