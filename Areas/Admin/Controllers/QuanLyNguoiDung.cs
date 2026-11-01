using HTNLShop.Data;
using HTNLShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace HTNLShop.Areas.Admin.Controllers
{
    [Route("Admin/[controller]/[action]")]
    public class QuanLyNguoiDung : AdminBaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly HtlnshopContext _db;
        private readonly ILogger<QuanLyNguoiDung> _logger;

        public QuanLyNguoiDung(
            UserManager<User> userManager,
            HtlnshopContext db,
            ILogger<QuanLyNguoiDung> logger)
        {
            _userManager = userManager;
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            const int pageSize = 10;
            var pageNumber = page is null || page < 1 ? 1 : page.Value;

            var users = _userManager.Users
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToPagedList(pageNumber, pageSize);

            var items = new List<AdminUserListItemVM>(users.Count);
            foreach (var user in users)
            {
                items.Add(new AdminUserListItemVM
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Roles = (await _userManager.GetRolesAsync(user)).ToList()
                });
            }

            var model = new StaticPagedList<AdminUserListItemVM>(
                items, users.PageNumber, users.PageSize, users.TotalItemCount);

            return View(model);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            ViewBag.Roles = RoleOptions();
            return View(new AdminUserFormVM());
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AdminUserFormVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = RoleOptions(model.Role);
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(nameof(model.Password), "Nhập mật khẩu cho tài khoản mới.");
                ViewBag.Roles = RoleOptions(model.Role);
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                ViewBag.Roles = RoleOptions(model.Role);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Role);
            _logger.LogInformation("Admin tạo tài khoản {User} với role {Role}", user.UserName, model.Role);

            TempData["Success"] = $"Đã tạo tài khoản {user.UserName}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.Roles = RoleOptions(roles.FirstOrDefault());
            return View(new AdminUserFormVM
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = roles.FirstOrDefault() ?? DbSeeder.CustomerRole
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(AdminUserFormVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user is null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = RoleOptions(model.Role);
                return View(model);
            }

            if (!string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                var byEmail = await _userManager.FindByEmailAsync(model.Email);
                if (byEmail is not null && byEmail.Id != user.Id)
                {
                    ModelState.AddModelError(nameof(model.Email), "Email đã được dùng bởi tài khoản khác.");
                    ViewBag.Roles = RoleOptions(model.Role);
                    return View(model);
                }
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentUserId = _userManager.GetUserId(User);

            if (string.Equals(currentUserId, user.Id.ToString()) &&
                !string.Equals(model.Role, DbSeeder.AdminRole, StringComparison.Ordinal))
            {
                ModelState.AddModelError(nameof(model.Role), "Bạn không thể tự bỏ quyền Admin của chính mình.");
                ViewBag.Roles = RoleOptions(model.Role);
                return View(model);
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
            {
                foreach (var error in update.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                ViewBag.Roles = RoleOptions(model.Role);
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var reset = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (!reset.Succeeded)
                {
                    foreach (var error in reset.Errors)
                        ModelState.AddModelError(nameof(model.Password), error.Description);

                    ViewBag.Roles = RoleOptions(model.Role);
                    return View(model);
                }

                _logger.LogWarning("Admin đặt lại mật khẩu cho {User}", user.UserName);
            }

            if (!currentRoles.Contains(model.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
                _logger.LogInformation("Đổi role của {User} sang {Role}", user.UserName, model.Role);
            }

            TempData["Success"] = $"Đã cập nhật {user.UserName}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return NotFound();

            if (string.Equals(_userManager.GetUserId(User), user.Id.ToString()))
            {
                TempData["Error"] = "Không thể xoá tài khoản đang đăng nhập.";
                return RedirectToAction(nameof(Index));
            }

            var hasOrders = await _db.Orders.AnyAsync(o => o.UserId == id);
            if (hasOrders)
            {
                TempData["Error"] = "Tài khoản đã có đơn hàng — không xoá để giữ lịch sử.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            TempData[result.Succeeded ? "Success" : "Error"] =
                result.Succeeded ? $"Đã xoá {user.UserName}." : "Xoá tài khoản thất bại.";

            return RedirectToAction(nameof(Index));
        }

        private static SelectList RoleOptions(string? selected = null) =>
            new(new[] { DbSeeder.AdminRole, DbSeeder.CustomerRole }, selected);
    }
}
