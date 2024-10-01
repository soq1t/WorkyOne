using WorkyOne.AppServices.DTOs;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Utilities
{
    /// <summary>
    /// Инструмент по обновлению сущности смены из DTO смены
    /// </summary>
    public class ShiftEntityUpdater : IEntityFromDtoUpdater<ShiftEntity, ShiftDto>
    {
        public void Update(ShiftEntity entity, ShiftDto dto)
        {
            entity.Name = dto.Name;
            entity.ColorCode = dto.ColorCode;
            entity.Beginning = dto.Beginning;
            entity.Ending = dto.Ending;
            entity.IsPredefined = dto.IsPredefined;
        }
    }
}
