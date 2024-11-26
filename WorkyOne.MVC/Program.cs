using AspNetCore.ReCaptcha;
using Serilog;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.DependencyRegister;
using WorkyOne.MVC.Middlewares;

namespace WorkyOne.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls("https://0.0.0.0:7202");

            builder.Configuration.AddJsonFile("appsettings.json");

#if DEBUG
            builder.Configuration.AddJsonFile("appsettings.Development.json");
#elif RELEASE
            builder.Configuration.AddJsonFile("appsettings.Production.json");

#endif

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));

            DependencyRegistrer.RegisterAll(builder.Services, builder.Configuration);

            builder.Host.UseSerilog(
                (context, services, configuration) =>
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAntiforgery();
            app.UseMiddleware<SessionMiddleware>();
            app.UseAuthentication();
            app.UseMiddleware<AccessFiltersMiddleware>();
            app.UseMiddleware<UnauthorizedRedirectionMiddleware>();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

#if DEBUG
            //CreateUserData(app.Services).Wait();
            //Test(app.Services).Wait();
            //CreateUserData(app.Services).Wait();
#endif

            app.Run();
        }

        private static async Task CreateUserData(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var usersService = scope.ServiceProvider.GetRequiredService<IUsersService>();

            var userInfo = await usersService.GetUserInfoAsync(
                new UserInfoRequest() { UserName = "test", }
            );
        }
    }
}
