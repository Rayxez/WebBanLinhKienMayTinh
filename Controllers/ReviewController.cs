using HTNLShop.Data;
using HTNLShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
namespace HTNLShop.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly HTNLShop.Data.HtlnshopContext _context;

        public ReviewController(HtlnshopContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult _Review(int ProductId)
        {
            var model = new Review { ProductId = ProductId };
            return PartialView("~/Views/Review/_ReviewForm.cshtml", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<PartialViewResult> _ListReview(int productId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreateDate)
                .Select(r => new ReviewVM
                {
                    ReviewId = r.ReviewId,
                    UserId = r.UserId,
                    UserName = r.User != null ? r.User.FullName : "Khách hàng",
                    Email = r.User != null ? r.User.Email : "Ẩn danh",
                    Content = r.Content,
                    Rate = r.Rate,
                    CreatedDate = r.CreateDate
                })
                .ToListAsync();

            return PartialView("~/Views/Review/_ListReview.cshtml", reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _PostReview(ReviewVM review)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Detail", "SanPham", new { id = review.ProductId });

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return Challenge();

            if (review.Rate is < 1 or > 5)
            {
                TempData["Message"] = "Số sao phải từ 1 đến 5.";
                return RedirectToAction("Detail", "SanPham", new { id = review.ProductId });
            }

            // Chỉ người đã mua sản phẩm (đơn không bị huỷ) mới được đánh giá.
            var hasPurchased = await _context.Orders
                .Where(o => o.UserId == userId && o.Status != Data.OrderStatus.Cancelled)
                .SelectMany(o => o.OrderItems)
                .AnyAsync(oi => oi.ProductId == review.ProductId);

            if (!hasPurchased)
            {
                TempData["Message"] = "Bạn cần mua sản phẩm này trước khi đánh giá.";
                return RedirectToAction("Detail", "SanPham", new { id = review.ProductId });
            }

            var entity = new Review
            {
                ProductId = review.ProductId,
                UserId = userId,
                Content = review.Content,
                Rate = review.Rate,
                CreateDate = DateTime.Now
            };

            _context.Reviews.Add(entity);
            await _context.SaveChangesAsync();

            // Sau khi lưu → load lại danh sách review
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == review.ProductId)
                .OrderByDescending(r => r.CreateDate)
                .Select(r => new ReviewVM
                {
                    ReviewId = r.ReviewId,
                    ProductId = r.ProductId,
                    UserId = r.UserId,
                    UserName = r.User != null ? r.User.FullName : "Khách hàng",
                    Email = r.User != null ? r.User.Email : "Ẩn danh",
                    Content = r.Content,
                    Rate = r.Rate,
                    CreatedDate = r.CreateDate
                })
                .ToListAsync();

            return RedirectToAction("Detail", "SanPham", new { id = review.ProductId });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

