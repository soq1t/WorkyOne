using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Shifts.Basic
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="SharedShiftEntity"/>
    /// </summary>
    public class SharedShiftProfile : Profile
    {
        public SharedShiftProfile()
        {
            // Entity > DTO
            CreateMap<SharedShiftEntity, SharedShiftDto>();

            // DTO > Entity
            CreateMap<SharedShiftDto, SharedShiftEntity>();
        }
    }
}
