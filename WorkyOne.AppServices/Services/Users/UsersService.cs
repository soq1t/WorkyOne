using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Users;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

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
        private readonly IHttpContextAccessor _contextAccessor;

        private AccessFilter<UserEntity> _userFilter;
        private AccessFilter<UserDataEntity> _userDataFilter;

        public UsersService(
            IUsersRepository usersRepo,
            IUserDatasRepository userDatasRepo,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IUserAccessInfoProvider accessInfoProvider
        )
        {
            _usersRepo = usersRepo;
            _userDatasRepo = userDatasRepo;
            _mapper = mapper;
            _contextAccessor = contextAccessor;

            InitFiltersAsync(accessInfoProvider).Wait();
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(
            UserInfoRequest request,
            CancellationToken cancellation = default
        )
        {
            ISpecification<UserEntity> filter;

            if (request.IsCurrentUserRequired)
            {
                var username = _contextAccessor.HttpContext.User?.Identity?.Name;
                filter = new Specification<UserEntity>(x => x.UserName == username);
            }
            else
            {
                filter = new Specification<UserEntity>(x =>
                    x.Id == request.UserId || x.UserName == request.UserName
                ).And(_userFilter);
            }

            var user = await _usersRepo.GetAsync(
                new EntityRequest<UserEntity>(filter),
                cancellation
            );

            if (user == null)
            {
                if (user == null)
                    return null;
            }

            var userDataFilter = new Specification<UserDataEntity>(x => x.UserId == user.Id);

            UserDataEntity? userData = await _userDatasRepo.GetAsync(
                new UserDataRequest(userDataFilter)
                {
                    IncludeFullSchedulesInfo = request.IncludeFullSchedulesInfo,
                    IncludeSchedules = request.IncludeSchedules,
                },
                cancellation
            );

            if (userData == null)
            {
                userData = new UserDataEntity() { UserId = user.Id };

                var result = await _userDatasRepo.CreateAsync(userData, cancellation);

                if (result.IsSucceed)
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

        private async Task InitFiltersAsync(IUserAccessInfoProvider accessInfoProvider)
        {
            var accessInfo = await accessInfoProvider.GetCurrentAsync();

            if (accessInfo == null)
                return;

            _userDataFilter = new UserDataAccessFilter(accessInfo);
            _userFilter = new UserAccessFilter(accessInfo);
        }
    }
}
