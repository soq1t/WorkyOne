using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Common
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="DailyInfoEntity"/>
    /// </summary>
    public class DailyInfoProfile : Profile
    {
        public DailyInfoProfile()
        {
            // Из Entity в DTO
            CreateMap<DailyInfoEntity, DailyInfoDto>();

            // Из DTO в Entity
            CreateMap<DailyInfoDto, DailyInfoEntity>();
        }
    }
}
