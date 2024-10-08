using WorkyOne.Contracts.DTOs;
using WorkyOne.Domain.Entities;

namespace WorkyOne.AppServices.Mappers
{
    /// <summary>
    /// Маппер DTO данных пользователя
    /// </summary>
    public class UserInfoDtoMapper
    {
        public UserInfoDto MapToUserInfoDto(UserEntity user, UserDataEntity userData)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (userData == null)
            {
                throw new ArgumentNullException(nameof(userData));
            }

            UserInfoDto dto = new UserInfoDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName
            };

            return dto;
        }
    }
}
