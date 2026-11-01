using HTNLShop.Data;
using HTNLShop.Helpers;
using HTNLShop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HTNLShop.Services
{
    /// <summary>
    /// Gộp giỏ hàng lưu trong session (khách vãng lai) vào giỏ hàng trong CSDL
    /// ngay sau khi đăng nhập. Tách khỏi controller để dùng lại và test được.
    /// </summary>
    public interface ICartMergeService
    {
        Task MergeAsync(int userId, ISession session);
    }

    public sealed class CartMergeService : ICartMergeService
    {
        public const string CartKey = "MYCART";

        private readonly HtlnshopContext _db;
        private readonly ILogger<CartMergeService> _logger;

        public CartMergeService(HtlnshopContext db, ILogger<CartMergeService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task MergeAsync(int userId, ISession session)
        {
            var sessionCart = session.Get<List<CartItems>>(CartKey);
            if (sessionCart is null || sessionCart.Count == 0)
                return;

            var cart = await _db.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart is null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                _db.Carts.Add(cart);
            }

            var productIds = sessionCart.Select(i => i.ProductId).Distinct().ToList();
            var products = await _db.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId);

            foreach (var item in sessionCart)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                    continue;

                var existing = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);

                if (existing is null)
                {
                    var quantity = Math.Clamp(item.Quantity, 1, Math.Max(product.StockQuantity, 1));
                    cart.CartItems.Add(new CartItem { ProductId = item.ProductId, Quantity = quantity });
                }
                else
                {
                    existing.Quantity = Math.Min(
                        existing.Quantity + item.Quantity,
                        Math.Max(product.StockQuantity, 1));
                }
            }

            await _db.SaveChangesAsync();
            session.Remove(CartKey);

            _logger.LogInformation("Đã gộp {Count} dòng giỏ hàng session cho user {UserId}",
                sessionCart.Count, userId);
        }
    }
}
