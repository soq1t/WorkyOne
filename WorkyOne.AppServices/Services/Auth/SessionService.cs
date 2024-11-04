using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WorkyOne.AppServices.Interfaces.Repositories.Auth;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.Contracts.Options.Auth;
using WorkyOne.Domain.Entities.Auth;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Auth
{
    /// <summary>
    /// Сервис управления токенами пользовательскими сессиями
    /// </summary>
    public class SessionService : ISessionService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IUsersRepository _usersRepository;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<SessionOptions> _options;

        public SessionService(
            IUsersRepository usersRepository,
            ISessionsRepository sessionsRepository,
            IHttpContextAccessor contextAccessor,
            IOptions<SessionOptions> options,
            IDateTimeService dateTimeService
        )
        {
            _usersRepository = usersRepository;
            _sessionsRepository = sessionsRepository;
            _contextAccessor = contextAccessor;
            _options = options;
            _dateTimeService = dateTimeService;
        }

        public async Task<string?> CreateSessionAsync(
            string userId,
            CancellationToken cancellation = default
        )
        {
            var user = await _usersRepository.GetAsync(
                new EntityRequest<UserEntity>(new EntityIdFilter<UserEntity>(userId)),
                cancellation
            );

            if (user == null)
            {
                return null;
            }

            var context = _contextAccessor.HttpContext;

            var session = new SessionEntity
            {
                User = user,
                Expiration = _dateTimeService.GetUtcNow().AddDays(_options.Value.ExpirationDays),
                IpAddress = context.Connection.RemoteIpAddress.ToString(),
            };

            var result = await _sessionsRepository.CreateAsync(session, cancellation);

            if (result.IsSucceed)
            {
                await _sessionsRepository.SaveChangesAsync(cancellation);
                return session.Token;
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteCurrentSessionAsync(CancellationToken cancellation = default)
        {
            var token = GetTokenFromCookies();

            var session = await _sessionsRepository.GetAsync(
                new EntityRequest<SessionEntity>(
                    new Specification<SessionEntity>(x => x.Token == token)
                ),
                cancellation
            );

            if (session != null)
            {
                _sessionsRepository.Delete(session);
                await _sessionsRepository.SaveChangesAsync(cancellation);
            }

            var context = _contextAccessor.HttpContext;

            context.Response.Cookies.Delete(_options.Value.CookiesName);
        }

        public Task<SessionEntity?> GetSessionByTokenAsync(
            string sessionToken,
            CancellationToken cancellation = default
        )
        {
            return _sessionsRepository.GetAsync(
                new EntityRequest<SessionEntity>(
                    new Specification<SessionEntity>(x => x.Token == sessionToken)
                ),
                cancellation
            );
        }

        public string? GetTokenFromCookies()
        {
            var context = _contextAccessor.HttpContext;

            return context.Request.Cookies[_options.Value.CookiesName];
        }

        public async Task<string?> RefreshTokenAsync(
            string token,
            CancellationToken cancellation = default
        )
        {
            var session = await _sessionsRepository.GetAsync(
                new EntityRequest<SessionEntity>(
                    new Specification<SessionEntity>(x => x.Token == token)
                ),
                cancellation
            );

            var ipAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (session != null)
            {
                session.Token = Guid.NewGuid().ToString();
                session.Expiration = _dateTimeService
                    .GetUtcNow()
                    .AddDays(_options.Value.ExpirationDays);
                session.IpAddress = ipAddress;

                _sessionsRepository.Update(session);
                await _sessionsRepository.SaveChangesAsync(cancellation);

                return session.Token;
            }

            return null;
        }

        public async Task<bool> VerifyTokenAsync(
            string token,
            CancellationToken cancellation = default
        )
        {
            var session = await _sessionsRepository.GetAsync(
                new EntityRequest<SessionEntity>(
                    new Specification<SessionEntity>(x => x.Token == token)
                ),
                cancellation
            );

            if (session == null)
            {
                return false;
            }

            if (_dateTimeService.GetUtcNow() > session.Expiration)
            {
                _sessionsRepository.Delete(session);
                await _sessionsRepository.SaveChangesAsync();
                return false;
            }
            else
            {
                return true;
            }
        }

        public void WriteTokenToCookies(string token)
        {
            var context = _contextAccessor.HttpContext;

            context.Response.Cookies.Append(
                _options.Value.CookiesName,
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = _dateTimeService.GetUtcNow().AddDays(_options.Value.ExpirationDays)
                }
            );
        }
    }
}
