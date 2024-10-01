using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.AppServices.DTOs;
using WorkyOne.Domain.Entities;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Mappers.Schedule
{
    /// <summary>
    /// Маппер DTO шаблона рабочего расписания
    /// </summary>
    public class TemplateDtoMapper
    {
        private readonly SingleDayShiftDtoMapper _singleDayShiftDtoMapper;
        private readonly RepititionDtoMapper _repititionDtoMapper;
        private readonly ShiftDtoMapper _shiftDtoMapper;

        public TemplateDtoMapper(
            SingleDayShiftDtoMapper singleDayShiftDtoMapper,
            RepititionDtoMapper repititionDtoMapper,
            ShiftDtoMapper shiftDtoMapper
        )
        {
            _singleDayShiftDtoMapper = singleDayShiftDtoMapper;
            _repititionDtoMapper = repititionDtoMapper;
            _shiftDtoMapper = shiftDtoMapper;
        }

        public TemplateDto MapToTemplateDto(TemplateEntity template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
            TemplateDto dto = new TemplateDto
            {
                Id = template.Id,
                IsMirrored = template.IsMirrored,
                Name = template.Name,
                StartDate = template.StartDate,
                Repetitions = new List<RepititionDto>(),
                SingleDayShifts = new List<SingleDayShiftDto>(),
                Shifts = new List<ShiftDto>()
            };

            foreach (SingleDayShiftEntity shift in template.SingleDayShifts)
            {
                var shiftDto = _singleDayShiftDtoMapper.MapToSingleDayShiftDto(shift);
                dto.SingleDayShifts.Add(shiftDto);
            }

            foreach (RepititionEntity repitition in template.Repititions)
            {
                var repititionDto = _repititionDtoMapper.MapToRepititionDto(repitition);
                dto.Repetitions.Add(repititionDto);
            }

            foreach (ShiftEntity shift in template.Shifts)
            {
                var shiftDto = _shiftDtoMapper.MapToShiftDto(shift);
                dto.Shifts.Add(shiftDto);
            }

            return dto;
        }
    }
}
