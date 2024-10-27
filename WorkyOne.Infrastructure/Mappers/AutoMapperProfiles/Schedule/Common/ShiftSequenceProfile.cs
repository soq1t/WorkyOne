using AutoMapper;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Common
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="UserInfoDto"/>
    /// </summary>
    public class ShiftSequenceProfile : Profile
    {
        public ShiftSequenceProfile()
        {
            CreateMap<ShiftSequenceEntity, ShiftSequenceDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(e => e.Shift.Name))
                .ForMember(dto => dto.Beginning, opt => opt.MapFrom(e => e.Shift.Beginning))
                .ForMember(dto => dto.Ending, opt => opt.MapFrom(e => e.Shift.Ending));

            CreateMap<ShiftSequenceDto, ShiftSequenceEntity>()
                .ForMember(e => e.Shift, opt => opt.Ignore());
        }
    }
}
