using System.Net;
using Xunit;

namespace HTNLShop.Tests;

/// <summary>
/// Ma trận phân quyền: mỗi URL quản trị phải chặn ẩn danh (302 → login),
/// chặn Customer (302 → AccessDenied) và cho Admin (200).
/// </summary>
[Collection("shop")]
public class AuthorizationMatrixTests
{
    private readonly ShopFixture _fx;

    public AuthorizationMatrixTests(ShopFixture fx) => _fx = fx;

    public static IEnumerable<object[]> AdminUrls => new[]
    {
        new object[] { "/Admin/QuanLySanPham/Index" },
        new object[] { "/Admin/QuanLyNguoiDung/Index" },
        new object[] { "/Admin/QuanLyDonHang/Index" },
        new object[] { "/Admin/DashBoard/Index" },
        new object[] { "/admin" }
    };

    [Theory]
    [MemberData(nameof(AdminUrls))]
    public async Task Anonymous_bi_chuyen_ve_trang_dang_nhap(string url)
    {
        var response = await _fx.Anonymous.GetAsync(url);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Customer/Login", response.Headers.Location!.OriginalString);
    }

    [Theory]
    [MemberData(nameof(AdminUrls))]
    public async Task Customer_bi_tu_choi_vao_khu_quan_tri(string url)
    {
        var response = await _fx.Customer.GetAsync(url);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Customer/AccessDenied", response.Headers.Location!.OriginalString);
    }

    [Theory]
    [MemberData(nameof(AdminUrls))]
    public async Task Admin_vao_duoc_khu_quan_tri(string url)
    {
        var response = await _fx.Admin.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Admin", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Don_hang_yeu_cau_dang_nhap()
    {
        var response = await _fx.Anonymous.GetAsync("/Order/Index");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Customer/Login", response.Headers.Location!.OriginalString);
    }

    [Fact]
    public async Task Ho_so_ca_nhan_yeu_cau_dang_nhap()
    {
        var response = await _fx.Anonymous.GetAsync("/Customer/ProFile");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    [Fact]
    public async Task Khach_hang_xem_duoc_ho_so_va_don_hang_cua_minh()
    {
        var profile = await _fx.Customer.GetAsync("/Customer/ProFile");
        var orders = await _fx.Customer.GetAsync("/Order/Index");

        Assert.Equal(HttpStatusCode.OK, profile.StatusCode);
        Assert.Equal(HttpStatusCode.OK, orders.StatusCode);
    }
}
