using WorkyOne.Contracts.Requests.Common;

namespace WorkyOne.Contracts.Requests.Schedule
{
    /// <summary>
    /// Запрос на получение из базы данных информации о шаблоне
    /// </summary>
    public class TemplateRequest : RequestBase
    {
        public string TemplateId { get; set; }

        public string ScheduleId { get; set; }
    }
}
