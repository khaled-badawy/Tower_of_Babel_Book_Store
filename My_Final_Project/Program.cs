using book_store.Repositry;
using book_store.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using My_Final_Project.Models;
using My_Final_Project.Repositry;

namespace My_Final_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICategoryRepositry, CategoryRepositry>();
            builder.Services.AddScoped<IBookRepositry, BookRepositry>();
            builder.Services.AddScoped<IAuthorRepositry, AuthorRepositry>();
            builder.Services.AddScoped<IReviewRepositry, ReviewRepositry>();
            builder.Services.AddScoped<IOrderRepositry, OrderRepositry>();
            builder.Services.AddScoped<IApplicationUserRepositry, ApplicationUserRepositry>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IOrderItemsRepositry, OrderItemsRepositry>();
            builder.Services.AddDbContext<StoreContext>(options => {
                options.UseNpgsql(builder.Configuration.GetConnectionString("cs"));
                options.EnableSensitiveDataLogging();
            }
                
                
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;

            })
                .AddEntityFrameworkStores<StoreContext>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.AccessDeniedPath = "/Home/Index";
                options.LogoutPath = "/Home/Index";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}