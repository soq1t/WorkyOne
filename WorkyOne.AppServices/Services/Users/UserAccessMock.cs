using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.AppServices.Services.Users
{
    public sealed class UserAccessMock : IUserAccessInfoProvider
    {
        public Task<UserAccessInfo> GetCurrentAsync(CancellationToken cancellation = default)
        {
            //var userId = "71970b97-bb57-4632-83af-16dba934d9e2";
            //var userDataId = "da777f7c-5327-421f-a96f-1dd7f58dbde7";
            var userId = "1";
            var userDataId = "1";
            return Task.FromResult(new UserAccessInfo(userDataId, userId, true));
        }
    }
}
