using System.ComponentModel.DataAnnotations;

namespace HTNLShop.ViewModels
{
    public class AdminUserListItemVM
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
    }

    public class AdminUserFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập tên đăng nhập")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập 3–50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Nhập họ tên")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Chọn vai trò")]
        [Display(Name = "Vai trò")]
        public string Role { get; set; } = DbSeederRoles.Customer;
    }

    /// <summary>Tên role dùng chung cho view-model (tránh phụ thuộc trực tiếp vào Data).</summary>
    public static class DbSeederRoles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
    }

    public class ProfileVM
    {
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
