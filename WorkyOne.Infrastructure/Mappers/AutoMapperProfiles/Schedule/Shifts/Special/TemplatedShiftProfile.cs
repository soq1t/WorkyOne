using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Shifts.Special
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="TemplatedShiftEntity"/>
    /// </summary>
    public sealed class TemplatedShiftProfile : Profile
    {
        public TemplatedShiftProfile()
        {
            // Entity > DTO
            CreateMap<TemplatedShiftEntity, TemplatedShiftDto>();

            // DTO > Entity
            CreateMap<TemplatedShiftDto, TemplatedShiftEntity>();
        }
    }
}
