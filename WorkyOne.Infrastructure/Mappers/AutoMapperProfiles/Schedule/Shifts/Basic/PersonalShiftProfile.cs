using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Shifts.Basic
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="PersonalShiftEntity"/>
    /// </summary>
    public class PersonalShiftProfile : Profile
    {
        public PersonalShiftProfile()
        {
            // Entity > DTO
            CreateMap<PersonalShiftEntity, PersonalShiftDto>();

            // DTO > Entity
            CreateMap<PersonalShiftDto, PersonalShiftEntity>();
        }
    }
}
