using HTNLShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTNLShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class QuanLyDonHang : AdminBaseController
    {
        private readonly HtlnshopContext _db;
        private readonly ILogger<QuanLyDonHang> _logger;

        public QuanLyDonHang(HtlnshopContext context, ILogger<QuanLyDonHang> logger)
        {
            _db = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string status = "", string searchTerm = "")
        {
            var ordersQuery = _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                ordersQuery = ordersQuery.Where(o => o.Status == status);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                ordersQuery = ordersQuery.Where(o =>
                    o.OrderId.ToString().Contains(searchTerm) ||
                    (o.User.FullName != null && o.User.FullName.Contains(searchTerm)) ||
                    (o.User.Email != null && o.User.Email.Contains(searchTerm)));
            }

            ViewBag.CurrentStatus = status;
            ViewBag.SearchTerm = searchTerm;

            return View(ordersQuery.OrderByDescending(o => o.OrderDate).ToList());
        }

        /// <summary>
        /// Chuyển trạng thái đơn theo đúng luật của <see cref="OrderStatus"/>.
        /// Huỷ đơn sẽ hoàn lại tồn kho đã trừ khi đặt hàng.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order is null)
                return NotFound();

            if (!OrderStatus.CanTransition(order.Status, status))
            {
                TempData["Error"] = $"Không thể chuyển đơn #{id} từ '{order.Status}' sang '{status}'.";
                return RedirectToAction(nameof(Index));
            }

            var previous = order.Status;
            order.Status = status;

            if (OrderStatus.RestoresStock(status))
            {
                foreach (var item in order.OrderItems)
                {
                    if (item.Product is not null)
                        item.Product.StockQuantity += item.Quantity;
                }
            }

            await _db.SaveChangesAsync();

            _logger.LogInformation("Đơn {OrderId}: {From} → {To} (bởi {User})",
                order.OrderId, previous, status, User.Identity?.Name);

            TempData["Success"] = $"Đơn #{order.OrderId}: {previous} → {status}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
