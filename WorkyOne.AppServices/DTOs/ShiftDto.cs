using System.ComponentModel.DataAnnotations;
using WorkyOne.AppServices.DTOs.Abstractions;

namespace WorkyOne.AppServices.DTOs
{
    /// <summary>
    /// DTO рабочей смены
    /// </summary>
    public class ShiftDto : DtoBase
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string? ColorCode { get; set; }
        public TimeOnly? Beginning { get; set; }
        public TimeOnly? Ending { get; set; }

        public bool IsPredefined { get; set; }
    }
}
