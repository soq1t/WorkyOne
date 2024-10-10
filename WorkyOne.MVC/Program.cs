using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkyOne.DependencyRegister;
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
            app.Run();
        }

        private static async Task Test(IServiceProvider services)
        {
            using (IServiceScope scope = services.CreateScope())
            {
                UserManager<UserEntity> userManager = scope.ServiceProvider.GetRequiredService<
                    UserManager<UserEntity>
                >();

                var users = await userManager.Users.ToListAsync();
            }
        }
    }
}
