using HTNLShop.Data;
using HTNLShop.Services;
using HTNLShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HTNLShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICartMergeService _cartMerge;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICartMergeService cartMerge,
            ILogger<CustomerController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartMerge = cartMerge;
            _logger = logger;
        }

        // ---------------------------------------------------------------------
        // Đăng ký
        // ---------------------------------------------------------------------
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "TrangChu");

            ViewData["Title"] = "Đăng ký";
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _userManager.FindByNameAsync(model.Username) is not null)
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại.");
                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng.");
                return View(model);
            }

            var user = new User
            {
                UserName = model.Username,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, DescribeError(error));

                return View(model);
            }

            // Role luôn do server quyết định — không nhận từ form.
            await _userManager.AddToRoleAsync(user, DbSeeder.CustomerRole);
            await _signInManager.SignInAsync(user, isPersistent: false);

            _logger.LogInformation("Tài khoản mới {User} đăng ký với role {Role}", user.UserName, DbSeeder.CustomerRole);

            TempData["Success"] = "Đăng ký thành công. Chào mừng bạn đến với HTNL.Shop!";
            return RedirectToAction("Index", "TrangChu");
        }

        // ---------------------------------------------------------------------
        // Đăng nhập / đăng xuất
        // ---------------------------------------------------------------------
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "TrangChu");

            ViewData["Title"] = "Đăng nhập";
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user is null)
            {
                // Không tiết lộ tài khoản có tồn tại hay không.
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, DbSeeder.AdminRole))
                    return Redirect("/admin");

                await _cartMerge.MergeAsync(user.Id, HttpContext.Session);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Cart");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("Tài khoản {User} bị tạm khoá do đăng nhập sai nhiều lần.", model.Username);
                ModelState.AddModelError(string.Empty, "Tài khoản tạm khoá 15 phút do đăng nhập sai quá 5 lần.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "TrangChu");
        }

        // ---------------------------------------------------------------------
        // Hồ sơ cá nhân
        // ---------------------------------------------------------------------
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ProFile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Challenge();

            ViewData["Title"] = "Hồ sơ của tôi";
            return View(new ProfileVM
            {
                UserName = user.UserName!,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CreatedAt = user.CreatedAt
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewData["Title"] = "Không có quyền truy cập";
            return View();
        }

        private static string DescribeError(IdentityError error) => error.Code switch
        {
            "PasswordTooShort" => "Mật khẩu tối thiểu 8 ký tự.",
            "PasswordRequiresDigit" => "Mật khẩu phải có ít nhất một chữ số.",
            "PasswordRequiresUpper" => "Mật khẩu phải có ít nhất một chữ in hoa.",
            "PasswordRequiresLower" => "Mật khẩu phải có ít nhất một chữ thường.",
            "DuplicateUserName" => "Tên đăng nhập đã tồn tại.",
            "DuplicateEmail" => "Email đã được sử dụng.",
            "InvalidUserName" => "Tên đăng nhập chỉ gồm chữ, số và các ký tự . _ -",
            "InvalidEmail" => "Email không hợp lệ.",
            _ => error.Description
        };
    }
}
