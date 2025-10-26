using BAL.Interfaces;
using BAL.Services;
using DAL.EF.AppDBContext;
using DAL.IRepo;
using DAL.IRepoServ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MimiPosStore.Hubs;
using SharedModels.EF.Models;

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
            // New repos
            builder.Services.AddScoped<DAL.IRepo.IRawMaterialService, RawMaterialRepo>();
            builder.Services.AddScoped<IRecipeRepo, RecipeRepo>();
            builder.Services.AddScoped<IRecipeInfoRepo, RecipeInfoRepo>();

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
            // New services
            builder.Services.AddScoped<BAL.Interfaces.IRawMaterialService, RawMaterialService>();
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<IRecipeInfoService, RecipeInfoService>();
            
            // Expense Services
            builder.Services.AddScoped<DAL.IRepo.IExpenseService, DAL.IRepoServ.ExpenseService>();
            builder.Services.AddScoped<BAL.Interfaces.IExpenseService, BAL.Services.ExpenseService>();


            // Reports Services - Following SOLID principles
            builder.Services.AddScoped<DAL.IRepoServ.clsReportsRepo>();
            builder.Services.AddScoped<IReportsService, ReportsService>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            string connectionString;

#if DEBUG
            connectionString = builder.Configuration.GetConnectionString("cs1")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "";
#else
            var envDbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var envDbName = Environment.GetEnvironmentVariable("DB_Name");
            var envDbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

            if (!string.IsNullOrWhiteSpace(envDbHost) && !string.IsNullOrWhiteSpace(envDbName) && !string.IsNullOrWhiteSpace(envDbPassword))
            {
                connectionString = $"Server={envDbHost},1433;Database={envDbName};User Id=sa;Password={envDbPassword};TrustServerCertificate=True;";
            }
            else
                throw new InvalidOperationException("Environment variables for DB connection are not set.");
#endif
            // Primary DbContext for regular operations and Identity
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(connectionString));

            // 3️⃣ Identity يستخدم الـ DbContext العادي فقط
            builder.Services.AddIdentity<clsUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = false;
            })
                        .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            
            builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

            builder.Services.AddSignalR();
            var app = builder.Build();

     CreateDataBase(app);
            app.MapHub<MessageHub>("/barcodeHub");

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
