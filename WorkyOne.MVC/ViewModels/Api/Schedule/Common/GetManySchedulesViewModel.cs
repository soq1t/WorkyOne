using System.ComponentModel.DataAnnotations;

namespace WorkyOne.MVC.ViewModels.Api.Schedule.Common
{
    /// <summary>
    /// Вьюмодель на получение множества расписаний
    /// </summary>
    public sealed class GetManySchedulesViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Укажите корректный номер страницы (более 1)")]
        public int PageIndex { get; set; }

        [Required]
        [Range(
            1,
            100,
            ErrorMessage = "Количество объектов на странице должно составлять от 1 до 100"
        )]
        public int Amount { get; set; }

        [Required]
        public bool? IncludeFullData { get; set; }
    }
}
