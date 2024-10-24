using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Users;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Users;

namespace WorkyOne.AppServices.Services.Users
{
    /// <summary>
    /// Сервис по работе с пользователями
    /// </summary>
    public sealed class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepo;
        private readonly IUserDatasRepository _userDatasRepo;
        private readonly IMapper _mapper;

        public UsersService(
            IUsersRepository usersRepo,
            IUserDatasRepository userDatasRepo,
            IMapper mapper
        )
        {
            _usersRepo = usersRepo;
            _userDatasRepo = userDatasRepo;
            _mapper = mapper;
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(
            UserInfoRequest request,
            CancellationToken cancellation = default
        )
        {
            var user = await _usersRepo.GetAsync(
                new EntityRequest<UserEntity>(request.UserId),
                cancellation
            );

            if (user == null)
            {
                user = await _usersRepo.GetAsync(
                    new EntityRequest<UserEntity>()
                    {
                        Predicate = (x) => x.UserName == request.UserName,
                    },
                    cancellation
                );

                if (user == null)
                    return null;
            }

            UserDataEntity? userData = await _userDatasRepo.GetAsync(
                new UserDataRequest(user.Id),
                cancellation
            );

            if (userData == null)
            {
                userData = new UserDataEntity() { UserId = user.Id };

                var result = await _userDatasRepo.CreateAsync(userData, cancellation);

                if (result.IsSuccess)
                {
                    await _userDatasRepo.SaveChangesAsync(cancellation);
                }
                else
                {
                    return null;
                }
            }

            var dto = _mapper.Map<UserInfoDto>(user);
            _mapper.Map(userData, dto);

            return dto;
        }
    }
}
