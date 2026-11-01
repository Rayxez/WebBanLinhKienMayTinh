using Microsoft.AspNetCore.Identity;

namespace HTNLShop.Data;

/// <summary>
/// Tài khoản người dùng của cửa hàng. Kế thừa ASP.NET Core Identity (khoá kiểu int)
/// để có sẵn hash mật khẩu, lockout, role, token reset mật khẩu.
/// </summary>
public class User : IdentityUser<int>
{
    public string? FullName { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
