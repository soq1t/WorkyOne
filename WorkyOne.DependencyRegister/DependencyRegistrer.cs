﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.AppServices.Services.Common;
using WorkyOne.AppServices.Services.Schedule.Common;
using WorkyOne.AppServices.Services.Schedule.Shifts;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Common;
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
            RegisterOther(services);
            RegisterAuth(services);
            RegisterContextes(services, configuration);
            RegisterRepositories(services);
            RegisterServices(services);
        }

        /// <summary>
        /// Регистрирует сервисы, используемые приложением
        /// </summary>
        /// <param name="services">Сервисы</param>
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IUserManagementService, UserManagementService>();

            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IDatedShiftsService, DatedShiftsService>();
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
            services.AddScoped<IBaseRepository, ApplicationBaseRepository>();

            services.AddScoped<IDailyInfosRepository, DailyInfosRepository>();
            services.AddScoped<ISchedulesRepository, SchedulesRepository>();
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
