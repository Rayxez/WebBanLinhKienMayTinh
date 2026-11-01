using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HTNLShop.Areas.Admin.Controllers
{
    /// <summary>
    /// Base cho toàn bộ controller khu quản trị: mọi action kế thừa đều bắt buộc
    /// đăng nhập với role Admin. Trước đây 4/5 controller admin không có [Authorize].
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public abstract class AdminBaseController : Controller
    {
    }
}
