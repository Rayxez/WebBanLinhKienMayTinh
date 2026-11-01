using System.Net;
using HTNLShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HTNLShop.Tests;

[Collection("shop")]
public class OrderLifecycleTests
{
    private readonly ShopFixture _fx;

    public OrderLifecycleTests(ShopFixture fx) => _fx = fx;

    [Fact]
    public async Task Admin_di_duoc_tron_vong_doi_don_hang()
    {
        var orderId = await _fx.PlaceOrderAsync(productId: 2, quantity: 1);

        foreach (var next in new[] { OrderStatus.Confirmed, OrderStatus.Shipping, OrderStatus.Delivered, OrderStatus.Completed })
        {
            var response = await _fx.SetOrderStatusAsync(orderId, next);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            using var scope = _fx.Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HtlnshopContext>();
            var status = await db.Orders.Where(o => o.OrderId == orderId).Select(o => o.Status).SingleAsync();
            Assert.Equal(next, status);
        }
    }

    [Fact]
    public async Task Huy_don_hoan_lai_dung_so_luong_ton_kho()
    {
        int stockBefore;
        using (var scope = _fx.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<HtlnshopContext>();
            stockBefore = await db.Products.Where(p => p.ProductId == 3).Select(p => p.StockQuantity).SingleAsync();
        }

        var orderId = await _fx.PlaceOrderAsync(productId: 3, quantity: 3);

        using (var scope = _fx.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<HtlnshopContext>();
            var afterOrder = await db.Products.Where(p => p.ProductId == 3).Select(p => p.StockQuantity).SingleAsync();
            Assert.Equal(stockBefore - 3, afterOrder);
        }

        var cancel = await _fx.SetOrderStatusAsync(orderId, OrderStatus.Cancelled);
        Assert.Equal(HttpStatusCode.Redirect, cancel.StatusCode);

        using (var scope = _fx.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<HtlnshopContext>();
            var order = await db.Orders.Where(o => o.OrderId == orderId).Select(o => o.Status).SingleAsync();
            var afterCancel = await db.Products.Where(p => p.ProductId == 3).Select(p => p.StockQuantity).SingleAsync();

            Assert.Equal(OrderStatus.Cancelled, order);
            Assert.Equal(stockBefore, afterCancel);
        }
    }

    [Fact]
    public async Task Khong_the_nhay_coc_tu_cho_xac_nhan_sang_da_giao()
    {
        var orderId = await _fx.PlaceOrderAsync(productId: 4, quantity: 1);

        var response = await _fx.SetOrderStatusAsync(orderId, OrderStatus.Delivered);
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        using var scope = _fx.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HtlnshopContext>();
        var status = await db.Orders.Where(o => o.OrderId == orderId).Select(o => o.Status).SingleAsync();

        Assert.Equal(OrderStatus.Pending, status);
    }
}

public class OrderStatusTests
{
    [Theory]
    [InlineData(OrderStatus.Pending, OrderStatus.Confirmed, true)]
    [InlineData(OrderStatus.Pending, OrderStatus.Cancelled, true)]
    [InlineData(OrderStatus.Confirmed, OrderStatus.Shipping, true)]
    [InlineData(OrderStatus.Confirmed, OrderStatus.Cancelled, true)]
    [InlineData(OrderStatus.Shipping, OrderStatus.Delivered, true)]
    [InlineData(OrderStatus.Delivered, OrderStatus.Completed, true)]
    [InlineData(OrderStatus.Pending, OrderStatus.Delivered, false)]
    [InlineData(OrderStatus.Pending, OrderStatus.Shipping, false)]
    [InlineData(OrderStatus.Cancelled, OrderStatus.Confirmed, false)]
    [InlineData(OrderStatus.Completed, OrderStatus.Cancelled, false)]
    public void Luat_chuyen_trang_thai(string from, string to, bool expected)
    {
        Assert.Equal(expected, OrderStatus.CanTransition(from, to));
    }

    [Fact]
    public void Chi_huy_don_moi_hoan_kho()
    {
        Assert.True(OrderStatus.RestoresStock(OrderStatus.Cancelled));
        Assert.False(OrderStatus.RestoresStock(OrderStatus.Delivered));
    }
}
