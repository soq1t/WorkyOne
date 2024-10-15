using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Shifts
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="DatedShiftEntity"/>
    /// </summary>
    public sealed class DatedShiftProfile : Profile
    {
        public DatedShiftProfile()
        {
            // Entity > DTO
            CreateMap<DatedShiftEntity, DatedShiftDto>();

            // DTO > Entity
            CreateMap<DatedShiftDto, DatedShiftEntity>();
        }
    }
}
