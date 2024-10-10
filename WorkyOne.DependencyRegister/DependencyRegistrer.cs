using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Repositories.Contextes;

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
            RegisterOther(services);
            RegisterAuth(services);
            RegisterContextes(services, configuration);
            RegisterRepositories(services);
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
#if DEBUG
            string prefix = "Remote";
#else
            string prefix = "Local";
#endif
            services.AddDbContext<UsersDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString($"{prefix}UsersConnection"))
            );
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString($"{prefix}MainConnection"))
            );
        }

        /// <summary>
        /// Регистрирует систему авторизации и аутентификации приложения
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        private static void RegisterAuth(IServiceCollection services)
        {
            //services
            //    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = "account/login";
            //        options.AccessDeniedPath = "account/accessDenied";
            //        options.LogoutPath = "account/logout";
            //    });

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services
                .AddIdentity<UserEntity, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<UsersDbContext>()
                .AddDefaultTokenProviders();
        }

        /// <summary>
        /// Регистрирует репозитории, используемые приложением
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IEntityRepository<UserEntity, UserRequest>>();
            services.AddScoped<IEntityRepository<UserDataEntity, UserDataRequest>>();
            services.AddScoped<IEntityRepository<TemplateEntity, TemplateRequest>>();
            services.AddScoped<IEntityRepository<TemplatedShiftEntity, TemplatedShiftRequest>>();
        }

        /// <summary>
        /// Регистрирует иные компоненты, используемые в приложении
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        private static void RegisterOther(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyRegistrer).Assembly);
        }
    }
}
