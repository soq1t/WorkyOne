using System.ComponentModel.DataAnnotations;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Abstractions.Common
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
