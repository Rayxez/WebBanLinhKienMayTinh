using System.ComponentModel.DataAnnotations;
using HTNLShop.Data;
using HTNLShop.ViewModels;
using Xunit;

namespace HTNLShop.Tests;

public class DomainTests
{
    [Fact]
    public void Don_hang_mac_dinh_o_trang_thai_cho_xac_nhan()
    {
        var order = new Order();

        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void Doanh_thu_chi_tinh_don_da_giao_hoac_hoan_tat()
    {
        Assert.Contains(OrderStatus.Delivered, OrderStatus.Revenue);
        Assert.Contains(OrderStatus.Completed, OrderStatus.Revenue);

        Assert.DoesNotContain(OrderStatus.Pending, OrderStatus.Revenue);
        Assert.DoesNotContain(OrderStatus.Confirmed, OrderStatus.Revenue);
        Assert.DoesNotContain(OrderStatus.Shipping, OrderStatus.Revenue);
        Assert.DoesNotContain(OrderStatus.Cancelled, OrderStatus.Revenue);
    }

    [Fact]
    public void Thanh_tien_dong_hang_bang_so_luong_nhan_don_gia()
    {
        var item = new CartItems { Price = 12_500_000, Quantity = 3 };

        Assert.Equal(37_500_000, item.Pay);
    }

    [Fact]
    public void Dang_ky_sai_mat_khau_nhap_lai_bi_bat_loi()
    {
        var model = new RegisterVM
        {
            Username = "newuser",
            Password = "Abc@12345",
            ConfirmPassword = "Khac@12345",
            FullName = "Nguyen Van A",
            Email = "a@example.com",
            PhoneNumber = "0912345678",
            Address = "1 Duong A"
        };

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterVM.ConfirmPassword)));
    }
}
