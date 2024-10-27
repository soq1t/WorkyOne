using System.ComponentModel.DataAnnotations;

namespace WorkyOne.Contracts.DTOs.Abstractions
{
    /// <summary>
    /// Базовый класс для DTO
    /// </summary>
    public class DtoBase
    {
        /// <summary>
        /// Идентификатор сущности, представляемой DTO
        /// </summary>
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
