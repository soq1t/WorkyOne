using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.AppServices.Interfaces.Repositories.Requests
{
    /// <summary>
    /// Запрос для получения шаблона расписаний
    /// </summary>
    public class TemplateRequest
    {
        /// <summary>
        /// ID запрашиваемого шаблона
        /// </summary>
        public string? TemplateId { get; set; }

        /// <summary>
        /// ID пользовательских данных, для которых запрашиваются все шаблоны
        /// </summary>
        public string? UserDataId { get; set; }

        /// <summary>
        /// Нужно ли включать все данные, которые содержит шаблон (смены и т.д.)
        /// </summary>
        public bool IncludeFullData { get; set; } = false;
    }
}
