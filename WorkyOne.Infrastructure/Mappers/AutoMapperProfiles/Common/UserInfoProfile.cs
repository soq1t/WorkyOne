using AutoMapper;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Common
{
    /// <summary>
    /// Профиль <see cref="AutoMapper"/> для маппинга <see cref="UserInfoDto"/>
    /// </summary>
    public sealed class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            // Маппинг Entity в DTO
            CreateMap<UserEntity, UserInfoDto>()
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(e => e.Id));

            CreateMap<UserDataEntity, UserInfoDto>()
                .ForMember(dto => dto.UserDataId, opt => opt.MapFrom(e => e.Id));

            // Маппинг DTO в Entity
            CreateMap<UserInfoDto, UserEntity>()
                .ForMember(e => e.Id, opt => opt.MapFrom(dto => dto.UserId));
            CreateMap<UserInfoDto, UserDataEntity>()
                .ForMember(e => e.Id, opt => opt.MapFrom(dto => dto.UserDataId));
        }
    }
}
