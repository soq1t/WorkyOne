using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WorkyOne.AppServices.Decorators;
using WorkyOne.AppServices.Interfaces.Repositories.Auth;
using WorkyOne.AppServices.Interfaces.Repositories.Context;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.AppServices.Interfaces.Services.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Special;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.AppServices.Services.Auth;
using WorkyOne.AppServices.Services.Common;
using WorkyOne.AppServices.Services.Schedule.Common;
using WorkyOne.AppServices.Services.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Services.Schedule.Shifts.Special;
using WorkyOne.AppServices.Services.Users;
using WorkyOne.Contracts.Options.Auth;
using WorkyOne.Contracts.Options.Common;
using WorkyOne.Contracts.Options.Schedules;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Common;
using WorkyOne.Infrastructure.Services;
using WorkyOne.Infrastructure.Stores;
using WorkyOne.Infrastructure.Utilities;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Schedule.Common;
using WorkyOne.Repositories.Repositories.Schedule.Shifts.Basic;
using WorkyOne.Repositories.Repositories.Schedule.Shifts.Special;
using WorkyOne.Repositories.Users.Contextes;
using WorkyOne.Repositories.Users.Repositories.Auth;
using WorkyOne.Repositories.Users.Repositories.Users;

namespace WorkyOne.DependencyRegister
{
    /// <summary>
    /// Компонент, осуществляющий регистрацию зависимостей в приложении
    /// </summary>
    public static class DependencyRegistrer
    {
        /// <summary>
        /// Регистрирует все необходимые зависимости
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        /// <param name="configuration">Конфигурация приложения</param>
        public static void RegisterAll(IServiceCollection services, IConfiguration configuration)
        {
            RegisterOptions(services, configuration);
            RegisterContextes(services, configuration);
            RegisterRepositories(services);
            RegisterAuth(services, configuration);
            RegisterServices(services);
            RegisterStores(services);
            RegisterOther(services, configuration);
        }

        private static void RegisterStores(IServiceCollection services)
        {
            services.AddScoped<IAccessFiltersStore, AccessFiltersStore>();
        }

        private static void RegisterOptions(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            services.Configure<SessionOptions>(configuration.GetSection("SessionOptions"));
            services.Configure<PaginationOptions>(configuration.GetSection("PaginationOptions"));
            services.Configure<WorkGraphicOptions>(configuration.GetSection("WorkGraphicOptions"));
        }

        /// <summary>
        /// Регистрирует сервисы, используемые приложением
        /// </summary>
        /// <param name="services">Сервисы</param>
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICachingService, CachingService>();

            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ISessionService, SessionService>();

            services.AddScoped<IUserAccessInfoProvider, UserAccessInfoProvider>();
            services.Decorate<IUserAccessInfoProvider, UserAccessInfoCachingDecorator>();

            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IWorkGraphicService, WorkGraphicService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<ICalendarService, CalendarService>();

            services.AddScoped<IPersonalShiftsService, PersonalShiftsService>();
            services.AddScoped<ISharedShiftsService, SharedShiftsService>();
            services.AddScoped<IDatedShiftsService, DatedShiftsService>();
            services.AddScoped<IPeriodicShiftsService, PeriodicShiftsService>();
            services.AddScoped<ITemplatedShiftService, TemplatedShiftService>();

            services.AddScoped<IApplicationContextService, ApplicationContextService>();
            services.AddScoped<IUsersContextService, UsersContextService>();
        }

        /// <summary>
        /// Регистрирует контексты баз данных, используемых приложением
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        /// <param name="configuration">Конфигурация приложения</param>
        private static void RegisterContextes(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<UsersDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString($"UsersDatabase"))
            );
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString($"AppDatabase"))
            );
        }

        /// <summary>
        /// Регистрирует систему авторизации и аутентификации приложения
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        private static void RegisterAuth(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<UserEntity, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<UsersDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var settings = configuration.GetSection("JwtOptions").Get<JwtOptions>();

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(settings.Secret)
                        )
                    };
                });

            services.AddAuthorization();
            //services
            //    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = "account/login";
            //        options.AccessDeniedPath = "account/accessDenied";
            //        options.LogoutPath = "account/logout";
            //    });

            //services
            //    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie();
        }

        /// <summary>
        /// Регистрирует репозитории, используемые приложением
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IDailyInfosRepository, DailyInfosRepository>();
            services.AddScoped<ISchedulesRepository, ScheduleRepository>();
            services.AddScoped<ITemplatesRepository, TemplatesRepository>();

            services.AddScoped<IPersonalShiftRepository, PersonalShiftRepository>();
            services.AddScoped<ISharedShiftsRepository, SharedShiftsRepository>();

            services.AddScoped<IDatedShiftsRepository, DatedShiftsRepository>();
            services.AddScoped<IPeriodicShiftsRepository, PeriodicShiftsRepository>();
            services.AddScoped<ITemplatedShiftsRepository, TemplatedShiftsRepository>();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserDatasRepository, UserDatasRepository>();

            services.AddScoped<ISessionsRepository, SessionsRepository>();
        }

        /// <summary>
        /// Регистрирует иные компоненты, используемые в приложении
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        private static void RegisterOther(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(DailyInfoProfile).Assembly);

            services.AddSingleton<IEntityUpdateUtility, EntityUpdateUtility>();
            services.AddSingleton<IColorUtility, ColorUtility>();

            services.AddScoped<IReferenceShiftUtility, ReferenceShiftUtility>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });
        }
    }
}
