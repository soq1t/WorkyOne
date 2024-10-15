using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Shifts
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
