using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Common;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Users;

namespace WorkyOne.AppServices.Services.Common
{
    /// <summary>
    /// Сервис управления пользователями
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsersRepository _usersRepo;
        private readonly IUserDatasRepository _usersDataRepo;
        private readonly IMapper _mapper;

        public UserManagementService(
            IUsersRepository usersRepo,
            IUserDatasRepository usersDataRepo,
            IMapper mapper
        )
        {
            _usersRepo = usersRepo;
            _usersDataRepo = usersDataRepo;
            _mapper = mapper;
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(
            string userId,
            CancellationToken cancellation = default
        )
        {
            var user = await _usersRepo.GetAsync(
                new EntityRequest<UserEntity> { EntityId = userId, }
            );

            if (user == null)
            {
                return null;
            }

            UserDataEntity? userData = await _usersDataRepo.GetAsync(
                new UserDataRequest { UserId = userId, }
            );

            if (userData == null)
            {
                userData = new UserDataEntity(userId);
                await _usersDataRepo.CreateAsync(userData, cancellation);
                await _usersDataRepo.SaveChangesAsync(cancellation);
            }

            var dto = _mapper.Map<UserInfoDto>(user);
            _mapper.Map(userData, dto);

            return dto;
        }
    }
}
