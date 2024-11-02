using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.AppServices.Services.Common;
using WorkyOne.AppServices.Services.Schedule.Common;
using WorkyOne.AppServices.Services.Schedule.Shifts;
using WorkyOne.AppServices.Services.Users;
using WorkyOne.Contracts.Configuration;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Common;
using WorkyOne.Infrastructure.Utilities;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Schedule.Common;
using WorkyOne.Repositories.Repositories.Schedule.Shifts;
using WorkyOne.Repositories.Repositories.Users;

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
            RegisterConfigs(services, configuration);
            RegisterContextes(services, configuration);
            RegisterRepositories(services);
            RegisterAuth(services, configuration);
            RegisterServices(services);
            RegisterOther(services);
        }

        private static void RegisterConfigs(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
        }

        /// <summary>
        /// Регистрирует сервисы, используемые приложением
        /// </summary>
        /// <param name="services">Сервисы</param>
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IEntityUpdateUtility, EntityUpdateUtility>();

            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();

#if DEBUG
            services.AddScoped<IUserAccessInfoProvider, UserAccessInfoProvider>();
#else
            services.AddScoped<IUserAccessInfoProvider, UserAccessInfoProvider>();
#endif

            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IWorkGraphicService, WorkGraphicService>();
            services.AddScoped<ITemplateService, TemplateService>();

            services.AddScoped<IDatedShiftsService, DatedShiftsService>();
            services.AddScoped<ITemplatedShiftService, TemplatedShiftService>();
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
                    var settings = configuration.GetSection("JwtSettings").Get<JwtOptions>();

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
            services.AddScoped<IShiftSequencesRepository, ShiftSequencesRepository>();
            services.AddScoped<ITemplatesRepository, TemplatesRepository>();

            services.AddScoped<IDatedShiftsRepository, DatedShiftsRepository>();
            services.AddScoped<IPeriodicShiftsRepository, PeriodicShiftsRepository>();
            services.AddScoped<ITemplatedShiftsRepository, TemplatedShiftsRepository>();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserDatasRepository, UserDatasRepository>();
        }

        /// <summary>
        /// Регистрирует иные компоненты, используемые в приложении
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        private static void RegisterOther(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DailyInfoProfile).Assembly);
        }
    }
}
