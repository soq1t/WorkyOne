using WorkyOne.AppServices.Interfaces.Services;

namespace WorkyOne.AppServices.Services.Common
{
    /// <summary>
    /// Сервис по работе с датой и временем
    /// </summary>
    public sealed class DateTimeService : IDateTimeService
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
