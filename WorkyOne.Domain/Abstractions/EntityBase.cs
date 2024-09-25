using System.ComponentModel.DataAnnotations;

namespace WorkyOne.Domain.Abstractions
{
    /// <summary>
    /// Основа для сущностей
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
