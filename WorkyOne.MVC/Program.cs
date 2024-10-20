using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.DependencyRegister;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            DependencyRegistrer.RegisterAll(builder.Services, builder.Configuration);

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
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            //Test(app.Services).Wait();
            CreateUserData(app.Services).Wait();
            app.Run();
        }

        private static async Task CreateUserData(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var repo = scope.ServiceProvider.GetRequiredService<IUserDatasRepository>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

            var user = await userManager.FindByNameAsync("soq1t");

            if (user != null)
            {
                var request = new UserDataRequest { UserId = user.Id };
                var data = await repo.GetAsync(request);

                if (data == null)
                {
                    data = new UserDataEntity(user.Id);
                    await repo.CreateAsync(data);
                }
            }
        }

        //private static async Task Test(IServiceProvider services)
        //{
        //    using IServiceScope scope = services.CreateScope();

        //    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        //    var user = new UserEntity();
        //    var userData = new UserDataEntity();

        //    var dto = mapper.Map<UserInfoDto>(user);
        //    mapper.Map(userData, dto);
        //}
    }
}
