using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkyOne.Domain.Entities;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.DependencyRegister
{
    public static class DependencyRegistrer
    {
        public static void RegisterAll(IServiceCollection services, IConfiguration configuration)
        {
            RegisterContextes(services, configuration);
            RegisterAuth(services);
        }

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
    }
}
