using System.Diagnostics;
using HTNLShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HTNLShop.Controllers
{
    /// <summary>
    /// Trang lỗi dùng chung. Trước đây pipeline trỏ tới /Home/Error nhưng controller
    /// không tồn tại nên mọi lỗi 500 đều trả về trang trắng.
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        public IActionResult StatusCode(int code)
        {
            ViewData["StatusCode"] = code;
            return View("~/Views/Shared/StatusCode.cshtml");
        }
    }
}
