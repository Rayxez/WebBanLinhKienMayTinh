using System.Net;
using Xunit;

namespace HTNLShop.Tests;

[Collection("shop")]
public class SecurityTests
{
    private readonly ShopFixture _fx;

    public SecurityTests(ShopFixture fx) => _fx = fx;

    [Fact]
    public async Task Post_thieu_CSRF_token_bi_chan_400()
    {
        var response = await _fx.Admin.PostAsync("/Admin/QuanLySanPham/DeleteProduct",
            new FormUrlEncodedContent(new Dictionary<string, string> { ["id"] = "1" }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Dang_nhap_thieu_CSRF_token_bi_chan_400()
    {
        var response = await _fx.Anonymous.PostAsync("/Customer/Login",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Username"] = "admin",
                ["Password"] = "Admin@12345"
            }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Health_check_tra_ve_ok()
    {
        var response = await _fx.Anonymous.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("ok", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Sitemap_tra_ve_xml_hop_le()
    {
        var response = await _fx.Anonymous.GetAsync("/sitemap.xml");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("urlset", body);
        Assert.Contains("/SanPham/Detail/1", body);
    }

    [Fact]
    public async Task Robots_txt_ton_tai_va_chan_admin()
    {
        var response = await _fx.Anonymous.GetAsync("/robots.txt");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Disallow: /Admin", body);
    }

    [Fact]
    public async Task Trang_chi_tiet_co_title_va_meta_theo_san_pham()
    {
        var response = await _fx.Anonymous.GetAsync("/SanPham/Detail/1");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Acer Nitro", body);
        Assert.Contains("name=\"description\"", body);
        Assert.Contains("og:title", body);
    }

    [Fact]
    public async Task Response_co_security_headers()
    {
        var response = await _fx.Anonymous.GetAsync("/");

        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.Equal("strict-origin-when-cross-origin", response.Headers.GetValues("Referrer-Policy").Single());
    }
}
