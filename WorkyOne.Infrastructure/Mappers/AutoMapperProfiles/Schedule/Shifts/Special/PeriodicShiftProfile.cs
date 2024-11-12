using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Shifts.Special
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="PeriodicShiftEntity"/>
    /// </summary>
    public sealed class PeriodicShiftProfile : Profile
    {
        public PeriodicShiftProfile()
        {
            // Entity > DTO
            CreateMap<PeriodicShiftEntity, PeriodicShiftDto>();

            // DTO > Entity
            CreateMap<PeriodicShiftDto, PeriodicShiftEntity>();
        }
    }
}
