using AutoMapper;
using WorkyOne.Contracts.DTOs;
using WorkyOne.Domain.Entities;

namespace WorkyOne.AppServices.Mappers.AutoMapperProfiles
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="UserInfoDto"/>
    /// </summary>
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            // Маппинг Entity в DTO
            CreateMap<UserEntity, UserInfoDto>()
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(e => e.Id));

            CreateMap<UserDataEntity, UserInfoDto>();

            // Маппинг DTO в Entity
            CreateMap<UserInfoDto, UserEntity>()
                .ForMember(e => e.Id, opt => opt.MapFrom(dto => dto.UserId));
            CreateMap<UserInfoDto, UserDataEntity>();
        }
    }
}
