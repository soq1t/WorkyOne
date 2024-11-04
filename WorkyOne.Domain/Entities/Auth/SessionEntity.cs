using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Domain.Entities.Auth
{
    public class SessionEntity : EntityBase
    {
        public string Token { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        public UserEntity User { get; set; }

        [Required]
        public DateTime Expiration { get; set; } = DateTime.Now.AddDays(10);

        public string? IpAddress { get; set; }
    }
}
