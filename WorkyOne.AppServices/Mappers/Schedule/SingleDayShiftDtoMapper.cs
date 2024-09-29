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
    /// Маппер DTO "однодневных" смен
    /// </summary>
    public class SingleDayShiftDtoMapper
    {
        private readonly ShiftDtoMapper _shiftDtoMapper;

        public SingleDayShiftDtoMapper(ShiftDtoMapper shiftDtoMapper)
        {
            _shiftDtoMapper = shiftDtoMapper;
        }

        public SingleDayShiftDto MapToSingleDayShiftDto(SingleDayShiftEntity singleDayShift)
        {
            var dto = new SingleDayShiftDto
            {
                Id = singleDayShift.Id,
                ShiftDate = singleDayShift.ShiftDate
            };

            dto.Shift = _shiftDtoMapper.MapToShiftDto(singleDayShift.Shift);

            return dto;
        }
    }
}
