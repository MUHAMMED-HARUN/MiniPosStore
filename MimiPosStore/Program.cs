using DAL.EF.AppDBContext;
using DAL.EF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using DAL.IRepo;
using DAL.IRepoServ;
using BAL.Interfaces;
using BAL.Services;

namespace MimiPosStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DAL Repositories
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();

            builder.Services.AddScoped<IImportOrderRepo, ImportOrderRepo>();
            builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
            builder.Services.AddScoped<IUnitRepo, UnitRepo>();
            builder.Services.AddScoped<ISupplierRepo, SupplierRepo>();
            builder.Services.AddScoped<IPeopleRepo, PeopleRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();

            // BAL Services
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IImportOrderItemService, ImportOrderItemService>();
            builder.Services.AddScoped<IImportOrderItemRepo, ImportOrderItemRepo>();

            builder.Services.AddScoped<IImportOrderService, ImportOrderService>();

            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IUnitService, UnitService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IPeopleService, PeopleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            //string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "miniPosStoreDB";
            //string dbName = Environment.GetEnvironmentVariable("DB_Name") ?? "MiniPosStore";
            //string dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD") ?? "sa123456";

            //string connectionString = $"Server={dbHost},1433;Database={dbName};User Id=sa;Password={dbPassword};TrustServerCertificate=True;";
            //builder.Services.AddDbContext<AppDBContext>(options =>
            //    options.UseSqlServer(connectionString));

            builder.Services.AddDbContext<AppDBContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            builder.Services.AddIdentity<clsUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // لا تحتاج لتأكيد البريد
                options.User.RequireUniqueEmail = false; // إلغاء إلزام البريد الإلكتروني
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();


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

            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
