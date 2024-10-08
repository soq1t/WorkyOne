using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Mappers.AutoMapperProfiles
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="UserInfoDto"/>
    /// </summary>
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            // Из Entity в DTO
            CreateMap<ScheduleEntity, ScheduleDto>();

            // Из DTO в Entity
            CreateMap<ScheduleDto, ScheduleEntity>();
        }
    }
}
