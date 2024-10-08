using AutoMapper;
using WorkyOne.Contracts.DTOs.Schedule;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Mappers.AutoMapperProfiles
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
