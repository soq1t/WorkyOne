using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.AppServices.DTOs;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Utilities
{
    /// <summary>
    /// Инструмент по обновлению "однодневной" смены из DTO "однодневной смены"
    /// </summary>
    public class SingleDayShiftUpdater
        : IEntityFromDtoUpdater<SingleDayShiftEntity, SingleDayShiftDto>
    {
        public void Update(SingleDayShiftEntity entity, SingleDayShiftDto dto)
        {
            entity.ShiftId = dto.Shift.Id;
            entity.ShiftDate = dto.ShiftDate;
        }
    }
}
