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
    /// Маппер DTO сущности, описывающей повторение смены
    /// </summary>
    public class RepititionDtoMapper
    {
        private readonly ShiftDtoMapper _shiftDtoMapper;

        public RepititionDtoMapper(ShiftDtoMapper shiftDtoMapper)
        {
            _shiftDtoMapper = shiftDtoMapper;
        }

        public RepititionDto MapToRepititionDto(RepititionEntity repitition)
        {
            var dto = new RepititionDto
            {
                Id = repitition.Id,
                Position = repitition.Position,
                RepetitionAmount = repitition.RepetitionAmount,
                ShiftId = repitition.ShiftId,
            };

            return dto;
        }
    }
}
