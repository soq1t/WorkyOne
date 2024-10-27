using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Users
{
    /// <summary>
    /// Интерфейс сервиса, предоставляющего <see cref="UserAccessInfo"/>
    /// </summary>
    public interface IUserAccessInfoProvider
    {
        public Task<UserAccessInfo> GetCurrentAsync(CancellationToken cancellation = default);
    }
}
