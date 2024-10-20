using System.ComponentModel.DataAnnotations;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Abstractions
{
    /// <summary>
    /// Основа для сущностей
    /// </summary>
    public abstract class EntityBase : IEntity, IUpdatable<EntityBase>
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public virtual void UpdateFields(EntityBase entity)
        {
            Id = entity.Id;
        }
    }
}
