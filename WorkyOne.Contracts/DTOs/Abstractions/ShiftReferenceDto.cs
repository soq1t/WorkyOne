using System.ComponentModel.DataAnnotations;

namespace WorkyOne.Contracts.DTOs.Abstractions
{
    /// <summary>
    /// Асбтакция DTO для сущности, имеющей ссылку на какую-то смену
    /// </summary>
    public abstract class ShiftReferenceDto : DtoBase
    {
        [Required]
        public string ShiftId { get; set; }
    }
}
