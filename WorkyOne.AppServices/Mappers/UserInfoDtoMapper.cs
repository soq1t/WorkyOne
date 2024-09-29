using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.AppServices.DTOs;
using WorkyOne.AppServices.Mappers.Schedule;
using WorkyOne.Domain.Entities;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Mappers
{
    /// <summary>
    /// Маппер DTO данных пользователя
    /// </summary>
    public class UserInfoDtoMapper
    {
        private readonly TemplateDtoMapper _templateDtoMapper;

        public UserInfoDtoMapper(TemplateDtoMapper templateDtoMapper)
        {
            _templateDtoMapper = templateDtoMapper;
        }

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
                UserDataId = userData.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                Templates = new List<TemplateDto>()
            };

            foreach (TemplateEntity template in userData.Templates)
            {
                TemplateDto templateDto = _templateDtoMapper.MapToTemplateDto(template);
                dto.Templates.Add(templateDto);
            }

            return dto;
        }
    }
}
