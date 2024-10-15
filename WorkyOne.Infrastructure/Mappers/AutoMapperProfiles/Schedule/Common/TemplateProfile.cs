using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Common
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="TemplateProfile"/>
    /// </summary>
    public class TemplateProfile : Profile
    {
        public TemplateProfile()
        {
            // Из Entity в DTO
            CreateMap<TemplateEntity, TemplateDto>();

            // Из DTO в Entity
            CreateMap<TemplateDto, TemplateEntity>();
        }
    }
}
