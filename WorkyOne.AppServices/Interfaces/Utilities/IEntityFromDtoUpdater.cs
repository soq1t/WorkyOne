using WorkyOne.AppServices.DTOs.Abstractions;

namespace WorkyOne.AppServices.Interfaces.Utilities
{
    /// <summary>
    /// Интерфейс инструмента по обновлению сущности из связанной DTO
    /// </summary>
    public interface IEntityFromDtoUpdater<TEntity, TDto>
        where TEntity : class
        where TDto : DtoBase
    {
        /// <summary>
        /// Обновляет сущность
        /// </summary>
        /// <param name="entity">Обновляемая сущность</param>
        /// <param name="dto">DTO, данные из которой используются для обновления сущности</param>
        public void Update(TEntity entity, TDto dto);
    }
}
