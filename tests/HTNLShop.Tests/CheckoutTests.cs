using System.Net;
using HTNLShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HTNLShop.Tests;

[Collection("shop")]
public class CheckoutTests
{
    private readonly ShopFixture _fx;

    public CheckoutTests(ShopFixture fx) => _fx = fx;

    [Fact]
    public async Task Dat_hang_tao_don_cho_xac_nhan_va_tru_dung_ton_kho()
    {
        int stockBefore;
        decimal price;

        using (var scope = _fx.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<HtlnshopContext>();
            var product = await db.Products.AsNoTracking().SingleAsync(p => p.ProductId == 1);
            stockBefore = product.StockQuantity;
            price = (decimal)product.Price;
        }

        var add = await _fx.Customer.GetAsync("/Cart/AddToCart?id=1&quantity=2");
        Assert.Equal(HttpStatusCode.Redirect, add.StatusCode);

        var checkoutPage = await _fx.Customer.GetAsync("/Cart/CheckOut");
        checkoutPage.EnsureSuccessStatusCode();
        var token = ShopFixture.ExtractAntiforgeryToken(await checkoutPage.Content.ReadAsStringAsync());

        var response = await _fx.Customer.PostAsync("/Cart/CheckOut", new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["FullName"] = "Tran Van Test",
                ["Phone"] = "0987654321",
                ["Address"] = "99 Duong Test, Ha Noi",
                ["GiongKhachHang"] = "false",
                ["Notes"] = "Don test tu dong",
                ["__RequestVerificationToken"] = token
            }));

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Order/OrderDetail/", response.Headers.Location!.OriginalString);

        using var verifyScope = _fx.Factory.Services.CreateScope();
        var verifyDb = verifyScope.ServiceProvider.GetRequiredService<HtlnshopContext>();

        var order = await verifyDb.Orders
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.OrderId)
            .FirstAsync();

        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal("Tran Van Test", order.ReceiverName);
        Assert.Equal("0987654321", order.ReceiverPhone);
        Assert.Contains(order.OrderItems, i => i.ProductId == 1 && i.SalePrice > 0);

        var stockAfter = await verifyDb.Products
            .Where(p => p.ProductId == 1)
            .Select(p => p.StockQuantity)
            .SingleAsync();

        Assert.Equal(stockBefore - 2, stockAfter);
        Assert.Equal(price, (decimal)order.OrderItems.First(i => i.ProductId == 1).SalePrice);
    }
}
