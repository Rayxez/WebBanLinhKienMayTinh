using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using HTNLShop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HTNLShop.Tests;

/// <summary>
/// Dựng ứng dụng thật (WebApplicationFactory) trên một CSDL SQL Server riêng cho mỗi lần chạy,
/// đăng nhập sẵn 3 "khách": ẩn danh, customer, admin — dùng lại cho toàn bộ test.
/// </summary>
public sealed class ShopFixture : IAsyncLifetime
{
    private readonly string _databaseName = $"HTNLShop_Test_{Guid.NewGuid():N}";

    public WebApplicationFactory<Program> Factory { get; private set; } = null!;

    public HttpClient Anonymous { get; private set; } = null!;

    public HttpClient Customer { get; private set; } = null!;

    public HttpClient Admin { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        // Cho phép CI trỏ vào SQL Server container; mặc định dùng LocalDB khi chạy máy cá nhân.
        var baseConnection = Environment.GetEnvironmentVariable("HTNLSHOP_TEST_CONNECTION")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=master;Trusted_Connection=True;TrustServerCertificate=True";

        var connectionString = new SqlConnectionStringBuilder(baseConnection)
        {
            InitialCatalog = _databaseName
        }.ConnectionString;

        Environment.SetEnvironmentVariable("ConnectionStrings__HTLNShop", connectionString);
        Environment.SetEnvironmentVariable("Seed__AdminPassword", "Admin@12345");
        Environment.SetEnvironmentVariable("Seed__CustomerPassword", "Customer@123");

        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
        });

        Anonymous = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Gọi một lần để host khởi động (migration + seed) trước khi đăng nhập.
        var warmup = await Anonymous.GetAsync("/");
        Assert.Equal(HttpStatusCode.OK, warmup.StatusCode);

        Customer = await LoginAsync("customer", "Customer@123");
        Admin = await LoginAsync("admin", "Admin@12345");
    }

    public async Task DisposeAsync()
    {
        try
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HtlnshopContext>();
            await db.Database.EnsureDeletedAsync();
        }
        catch
        {
            // Không che giấu lỗi test, chỉ bỏ qua lỗi dọn dẹp.
        }

        Factory.Dispose();
        Environment.SetEnvironmentVariable("ConnectionStrings__HTLNShop", null);
    }

    public IServiceScope CreateScope() => Factory.Services.CreateScope();

    /// <summary>Đặt một đơn hàng thật qua HTTP và trả về OrderId.</summary>
    public async Task<int> PlaceOrderAsync(int productId, int quantity)
    {
        var add = await Customer.GetAsync($"/Cart/AddToCart?id={productId}&quantity={quantity}");
        Assert.Equal(HttpStatusCode.Redirect, add.StatusCode);

        var checkoutPage = await Customer.GetAsync("/Cart/CheckOut");
        checkoutPage.EnsureSuccessStatusCode();
        var token = ExtractAntiforgeryToken(await checkoutPage.Content.ReadAsStringAsync());

        var response = await Customer.PostAsync("/Cart/CheckOut", new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["FullName"] = "Khach Test",
                ["Phone"] = "0911222333",
                ["Address"] = "1 Duong Test",
                ["GiongKhachHang"] = "false",
                ["Notes"] = "Don test",
                ["__RequestVerificationToken"] = token
            }));

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        var match = Regex.Match(response.Headers.Location!.OriginalString, @"/Order/OrderDetail/(\d+)");
        Assert.True(match.Success, "Không tìm thấy OrderId trong redirect.");
        return int.Parse(match.Groups[1].Value);
    }

    /// <summary>Admin đổi trạng thái đơn (kèm CSRF token lấy từ trang quản lý đơn).</summary>
    public async Task<HttpResponseMessage> SetOrderStatusAsync(int orderId, string status)
    {
        var page = await Admin.GetAsync("/Admin/QuanLyDonHang/Index");
        page.EnsureSuccessStatusCode();
        var token = ExtractAntiforgeryToken(await page.Content.ReadAsStringAsync());

        return await Admin.PostAsync("/Admin/QuanLyDonHang/UpdateStatus", new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["id"] = orderId.ToString(),
                ["status"] = status,
                ["__RequestVerificationToken"] = token
            }));
    }

    public static string ExtractAntiforgeryToken(string html)
    {
        var match = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");
        return match.Success ? match.Groups[1].Value : throw new InvalidOperationException("Không tìm thấy CSRF token.");
    }

    private async Task<HttpClient> LoginAsync(string userName, string password)
    {
        var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var loginPage = await client.GetAsync("/Customer/Login");
        loginPage.EnsureSuccessStatusCode();

        var token = ExtractAntiforgeryToken(await loginPage.Content.ReadAsStringAsync());
        var response = await client.PostAsync("/Customer/Login", new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["Username"] = userName,
                ["Password"] = password,
                ["__RequestVerificationToken"] = token
            }));

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        return client;
    }
}

[CollectionDefinition("shop")]
public class ShopCollection : ICollectionFixture<ShopFixture>
{
}
