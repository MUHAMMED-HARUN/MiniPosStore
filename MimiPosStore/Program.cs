using BAL.Interfaces;
using BAL.Services;
using DAL.EF.AppDBContext;
using SharedModels.EF.Models;
using DAL.IRepo;
using DAL.IRepoServ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MimiPosStore
{
    public class Solution
    {

    }
    public class Program
    {

        public static void CreateDataBase( WebApplication app)
        {

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();         
            }

        }
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
            // Build connection string based on environment variables if present (Docker),
            // otherwise fall back to appsettings connection strings.
            var envDbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var envDbName = Environment.GetEnvironmentVariable("DB_Name");
            var envDbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

            string connectionString;
            if (!string.IsNullOrWhiteSpace(envDbHost) && !string.IsNullOrWhiteSpace(envDbName) && !string.IsNullOrWhiteSpace(envDbPassword))
            {
                connectionString = $"Server={envDbHost},1433;Database={envDbName};User Id=sa;Password={envDbPassword};TrustServerCertificate=True;";
            }
            else
            {
                connectionString = builder.Configuration.GetConnectionString("cs")
                    ?? builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? "";
            }

            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<clsUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // لا تحتاج لتأكيد البريد
                options.User.RequireUniqueEmail = false; // إلغاء إلزام البريد الإلكتروني
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();

            var result = new List< clsCustomer>();
         result.Where(i => i.PersonID == 1)                         // 1. فلترة حسب التاريخ
          .GroupBy(i => i.PersonID)                        // 2. تجميع حسب رقم المنتج
          .Select(g => new                                  // 3. اختيار شكل النتيجة
          {
              ProductID = g.Key,
              Count = g.Count()
          })
          .OrderBy(x => x.ProductID); 

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            
            builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();


            var app = builder.Build();

     CreateDataBase(app);

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
