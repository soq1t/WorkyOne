using System.ComponentModel.DataAnnotations;
using WorkyOne.Domain.Interfaces;

namespace WorkyOne.Domain.Abstractions
{
    /// <summary>
    /// Основа для сущностей
    /// </summary>
    public abstract class EntityBase : IEntity
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
