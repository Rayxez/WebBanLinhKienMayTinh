using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HTNLShop.Data;

/// <summary>
/// Dùng cho `dotnet ef migrations` / `dotnet ef database update` khi không muốn
/// khởi động toàn bộ ứng dụng (tránh chạy seeder ở design-time).
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HtlnshopContext>
{
    public HtlnshopContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("HTNLSHOP_CONNECTION")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=HTLNShop;Trusted_Connection=True;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<HtlnshopContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new HtlnshopContext(options);
    }
}
