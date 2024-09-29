using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.AppServices.DTOs;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Mappers.Schedule
{
    /// <summary>
    /// Маппер DTO рабочих смен
    /// </summary>
    public class ShiftDtoMapper
    {
        public ShiftDto MapToShiftDto(ShiftEntity shift)
        {
            var dto = new ShiftDto
            {
                Id = shift.Id,
                Name = shift.Name,
                ColorCode = shift.ColorCode,
                Beginning = shift.Beginning,
                Ending = shift.Ending,
            };

            return dto;
        }
    }
}
