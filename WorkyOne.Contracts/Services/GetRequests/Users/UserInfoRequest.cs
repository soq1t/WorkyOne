using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Interfaces.Services.GetRequests;

namespace WorkyOne.Contracts.Services.GetRequests.Users
{
    /// <summary>
    /// Запрос на получение <see cref="UserInfoDto"/>
    /// </summary>
    public class UserInfoRequest : IUserInfoRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public bool IsCurrentUserRequired { get; set; } = false;

        [Required]
        public bool IncludeSchedules { get; set; }

        [Required]
        public bool IncludeFullSchedulesInfo { get; set; }
    }
}
