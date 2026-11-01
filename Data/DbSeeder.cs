using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HTNLShop.Data;

/// <summary>
/// Tạo schema (môi trường Development) và dữ liệu khởi tạo: role, tài khoản demo.
/// Idempotent — chạy lại nhiều lần không nhân đôi dữ liệu.
/// </summary>
public static class DbSeeder
{
    public const string AdminRole = "Admin";
    public const string CustomerRole = "Customer";

    public static async Task InitializeAsync(IServiceProvider services, IWebHostEnvironment environment)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

        try
        {
            var db = provider.GetRequiredService<HtlnshopContext>();
            var configuration = provider.GetRequiredService<IConfiguration>();

            // Tự chạy migration khi Development, hoặc khi bật rõ ràng bằng cấu hình
            // (ví dụ docker-compose đặt Database__AutoMigrate=true cho môi trường demo).
            var autoMigrate = configuration.GetValue("Database:AutoMigrate", environment.IsDevelopment());
            if (autoMigrate)
            {
                await db.Database.MigrateAsync();
            }

            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            foreach (var role in new[] { AdminRole, CustomerRole })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole<int>(role));
                    if (!result.Succeeded)
                        logger.LogError("Không tạo được role {Role}: {Errors}", role,
                            string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }

            var userManager = provider.GetRequiredService<UserManager<User>>();

            // Mật khẩu mặc định chỉ dùng ở Development. Ở môi trường khác phải cấu hình
            // Seed__AdminPassword / Seed__CustomerPassword, nếu không sẽ bỏ qua seed.
            var adminPassword = configuration["Seed:AdminPassword"];
            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                if (environment.IsDevelopment())
                    adminPassword = "Admin@12345";
                else
                    logger.LogWarning("Thiếu Seed:AdminPassword — bỏ qua tạo tài khoản quản trị mặc định.");
            }

            if (!string.IsNullOrWhiteSpace(adminPassword))
            {
                await EnsureUserAsync(userManager, logger, new User
                {
                    UserName = "admin",
                    Email = "admin@htnl.shop",
                    EmailConfirmed = true,
                    FullName = "Quản trị viên",
                    PhoneNumber = "0900000001",
                    Address = "218 Lĩnh Nam, Hà Nội"
                }, adminPassword, AdminRole);
            }

            var customerPassword = configuration["Seed:CustomerPassword"];
            if (string.IsNullOrWhiteSpace(customerPassword) && environment.IsDevelopment())
                customerPassword = "Customer@123";

            if (!string.IsNullOrWhiteSpace(customerPassword))
            {
                await EnsureUserAsync(userManager, logger, new User
                {
                    UserName = "customer",
                    Email = "customer@htnl.shop",
                    EmailConfirmed = true,
                    FullName = "Nguyễn Văn Khách",
                    PhoneNumber = "0900000002",
                    Address = "12 Nguyễn Huệ, Quận 1, TP.HCM"
                }, customerPassword, CustomerRole);
            }
        }
        catch (Exception ex)
        {
            // Không nuốt lỗi: DB không sẵn sàng thì ứng dụng phải dừng để người vận hành biết.
            logger.LogCritical(ex, "Khởi tạo dữ liệu thất bại. Kiểm tra chuỗi kết nối và migration.");
            throw;
        }
    }

    private static async Task EnsureUserAsync(
        UserManager<User> userManager,
        ILogger logger,
        User template,
        string password,
        string role)
    {
        var existing = await userManager.FindByNameAsync(template.UserName!);
        if (existing is not null)
        {
            if (!await userManager.IsInRoleAsync(existing, role))
                await userManager.AddToRoleAsync(existing, role);

            return;
        }

        var result = await userManager.CreateAsync(template, password);
        if (!result.Succeeded)
        {
            logger.LogError("Không tạo được tài khoản {User}: {Errors}", template.UserName,
                string.Join("; ", result.Errors.Select(e => e.Description)));
            return;
        }

        await userManager.AddToRoleAsync(template, role);
        logger.LogInformation("Đã tạo tài khoản demo {User} với role {Role}", template.UserName, role);
    }
}
