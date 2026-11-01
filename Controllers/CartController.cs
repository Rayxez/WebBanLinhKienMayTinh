using HTNLShop.Data;
using HTNLShop.Helpers;
using HTNLShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace HTNLShop.Controllers
{
    public class CartController : Controller
    {
        private readonly HtlnshopContext _context;
        private readonly ILogger<CartController> _logger;
        const string CART_KEY = "MYCART";
        public CartController(HtlnshopContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out int userId))
                {
                    var count = await _context.CartItems
                        .Where(ci => ci.Cart.UserId == userId)
                        .SumAsync(ci => ci.Quantity);
                    return Json(count);
                }
            }
            else
            {
                var cart = HttpContext.Session.Get<List<CartItems>>(CART_KEY);
                return Json(cart?.Sum(i => i.Quantity) ?? 0);
            }

            return Json(0);
        }
        public List<CartItems> Cart => HttpContext.Session.Get<List<CartItems>>(CART_KEY) ?? new List<CartItems>();
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out int userId))
                {
                    var cart = _context.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(i => i.Product)
                        .FirstOrDefault(c => c.UserId == userId);

                    var model = cart?.CartItems.Select(ci => new CartItems
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Product?.ProductName,
                        Price = ci.Product?.Price ?? 0,
                        Quantity = ci.Quantity,
                        Image = ci.Product?.ImageUrl
                    }).ToList() ?? new List<CartItems>();

                    return View(model);
                }
            }

            // Nếu chưa đăng nhập → lấy từ Session
            var sessionCart = HttpContext.Session.Get<List<CartItems>>("MYCART") ?? new List<CartItems>();
            return View(sessionCart);
        }
        #region Old AddToCart 
        //public IActionResult AddToCart(int id, int quantity = 1)
        //{
        //    if (User.Identity != null && User.Identity.IsAuthenticated)
        //    {

        //        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        if (!int.TryParse(userIdStr, out int userId))
        //            return Unauthorized();


        //        var cart =  _context.Carts
        //            .Include(c => c.CartItems)
        //            .FirstOrDefault(c => c.UserId == userId);

        //        if (cart == null)
        //        {
        //            cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
        //            _context.Carts.Add(cart);
        //        }

        //        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == id);
        //        if (existingItem != null)
        //        {
        //            existingItem.Quantity += quantity;
        //        }
        //        else
        //        {
        //            var product =  _context.Products.FindAsync(id);
        //            if (product == null)
        //                return NotFound();

        //            cart.CartItems.Add(new CartItem
        //            {
        //                ProductId = id,
        //                Quantity = quantity
        //            });
        //        }

        //         _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        var cart = Cart;
        //        var item = cart.SingleOrDefault(p => p.ProductId == id);
        //        if (item == null)
        //        {
        //            var product = _context.Products.SingleOrDefault(p => p.ProductId == id);
        //            if (product == null)
        //            {
        //                TempData["Message"] = $"Không tìm thấy hàng hoá có mã {id}";
        //                return Redirect("/404");
        //            }
        //            item = new CartItems
        //            {
        //                ProductId = product.ProductId,
        //                ProductName = product.ProductName,
        //                Price = product.Price,
        //                Image = product.ImageUrl ?? string.Empty,
        //                Quantity = quantity
        //            };
        //            cart.Add(item);
        //        }
        //        else
        //        {
        //            item.Quantity += quantity;
        //        }
        //        HttpContext.Session.Set(CART_KEY, cart);
        //    }
        //    return RedirectToAction("Index");
        //}
        #endregion
        // Replace/Add the modified AddToCart, IncreaseQuantity and DecreaseQuantity methods
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            if (quantity < 1)
            {
                TempData["Message"] = "Số lượng phải lớn hơn 0.";
                return RedirectToAction("Detail", "SanPham", new { id });
            }

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId))
                    return Unauthorized();

                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                    _context.Carts.Add(cart);
                }

                var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == id);
                if (existingItem != null)
                {
                    var maxStock = existingItem.Product?.StockQuantity ?? (_context.Products.Find(id)?.StockQuantity ?? 0);
                    if (existingItem.Quantity + quantity > maxStock)
                    {
                        TempData["Message"] = "Không đủ tồn kho.";
                        return RedirectToAction("Index");
                    }
                    existingItem.Quantity += quantity;
                }
                else
                {
                    var product = await _context.Products.FindAsync(id);
                    if (product == null)
                        return NotFound();

                    if (quantity > product.StockQuantity)
                    {
                        TempData["Message"] = "Không đủ tồn kho.";
                        return RedirectToAction("Index");
                    }

                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = id,
                        Quantity = quantity
                    });
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                var cart = Cart;
                var item = cart.SingleOrDefault(p => p.ProductId == id);
                var product = _context.Products.SingleOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hoá có mã {id}";
                    return Redirect("/404");
                }

                var max = product.StockQuantity;
                if (item == null)
                {
                    if (quantity > max)
                    {
                        TempData["Message"] = "Không đủ tồn kho.";
                        return RedirectToAction("Index");
                    }
                    item = new CartItems
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Image = product.ImageUrl ?? string.Empty,
                        Quantity = quantity
                    };
                    cart.Add(item);
                }
                else
                {
                    if (item.Quantity + quantity > max)
                    {
                        TempData["Message"] = "Không đủ tồn kho.";
                        return RedirectToAction("Index");
                    }
                    item.Quantity += quantity;
                }
                HttpContext.Session.Set(CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int id)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

                var cartItem = _context.CartItems
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Cart)
                    .FirstOrDefault(ci => ci.Cart.UserId == userId && ci.ProductId == id);

                if (cartItem == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm.", maxStock = 0 });

                var maxStock = cartItem.Product?.StockQuantity ?? 0;
                if (cartItem.Quantity + 1 > maxStock)
                {
                    // trả về maxStock để client vô hiệu nút '+'
                    return Json(new { success = false, message = "Đã đạt tối đa tồn kho.", quantity = cartItem.Quantity, maxStock });
                }

                cartItem.Quantity++;
                _context.SaveChanges();

                var lineTotal = cartItem.Quantity * cartItem.Product.Price;
                var total = _context.CartItems
                    .Where(ci => ci.Cart.UserId == userId)
                    .Sum(ci => ci.Quantity * ci.Product.Price);

                return Json(new { success = true, quantity = cartItem.Quantity, total, lineTotal, maxStock });
            }

            var cart = Cart;
            var item = cart.FirstOrDefault(p => p.ProductId == id);
            if (item == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm.", maxStock = 0 });

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            var max = product?.StockQuantity ?? 0;
            if (item.Quantity + 1 > max)
                return Json(new { success = false, message = "Đã đạt tối đa tồn kho.", quantity = item.Quantity, maxStock = max });

            item.Quantity++;
            HttpContext.Session.Set(CART_KEY, cart);

            var totalSession = cart.Sum(i => i.Price * i.Quantity);
            var lineTotalSession = item.Quantity * item.Price;
            return Json(new { success = true, quantity = item.Quantity, total = totalSession, lineTotal = lineTotalSession, maxStock = max });
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int id)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

                var cartItem = _context.CartItems
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Cart)
                    .FirstOrDefault(ci => ci.Cart.UserId == userId && ci.ProductId == id);

                if (cartItem == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm.", maxStock = 0 });

                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    _context.SaveChanges();
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                    _context.SaveChanges();
                    // nếu xóa thì quantity = 0
                    return Json(new { success = true, quantity = 0, total = _context.CartItems.Where(ci => ci.Cart.UserId == userId).Sum(ci => ci.Quantity * ci.Product.Price), lineTotal = 0, maxStock = 0 });
                }

                var lineTotal = cartItem.Quantity * cartItem.Product.Price;
                var total = _context.CartItems
                    .Where(ci => ci.Cart.UserId == userId)
                    .Sum(ci => ci.Quantity * ci.Product.Price);

                return Json(new { success = true, quantity = cartItem.Quantity, total, lineTotal, maxStock = cartItem.Product.StockQuantity });
            }

            var cart = Cart;
            var item = cart.FirstOrDefault(p => p.ProductId == id);
            if (item == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm.", maxStock = 0 });

            if (item.Quantity > 1)
            {
                item.Quantity--;
                HttpContext.Session.Set(CART_KEY, cart);
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
                var maxStock = product?.StockQuantity ?? 0;
                var totalSession = cart.Sum(i => i.Price * i.Quantity);
                var lineTotalSession = item.Quantity * item.Price;
                return Json(new { success = true, quantity = item.Quantity, total = totalSession, lineTotal = lineTotalSession, maxStock });
            }
            else
            {
                // remove
                cart.Remove(item);
                HttpContext.Session.Set(CART_KEY, cart);
                var totalSession = cart.Sum(i => i.Price * i.Quantity);
                return Json(new { success = true, quantity = 0, total = totalSession, lineTotal = 0, maxStock = 0 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCart(int productId)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId))
                    return Json(new { success = false, message = "Unauthorized" });

                var cartItem = await _context.CartItems
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Cart)
                    .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);

                if (cartItem == null)
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm." });

                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                var total = await _context.CartItems
                    .Where(ci => ci.Cart.UserId == userId)
                    .SumAsync(ci => ci.Quantity * ci.Product.Price);

                return Json(new { success = true, total });
            }
            else
            {
                var cart = Cart;
                var item = cart.FirstOrDefault(p => p.ProductId == productId);

                if (item == null)
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm." });

                cart.Remove(item);
                HttpContext.Session.Set(CART_KEY, cart);

                var total = cart.Sum(i => i.Price * i.Quantity);
                return Json(new { success = true, total });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Challenge();
            var customer = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            ViewBag.Customer = customer;

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["Message"] = "Giỏ hàng trống, vui lòng thêm sản phẩm.";
                return RedirectToAction("Index");
            }

            var model = cart.CartItems.Select(ci => new CartItems
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.ProductName,
                Price = ci.Product.Price,
                Quantity = ci.Quantity,
                Image = ci.Product.ImageUrl
            }).ToList();

            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuyNow(int productId, int quantity = 1)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Challenge();

            var product = await _context.Products.FindAsync(productId);
            if (product is null)
                return NotFound();

            if (quantity < 1) quantity = 1;

            if (quantity > product.StockQuantity)
            {
                TempData["Message"] = "Số lượng vượt quá tồn kho.";
                return RedirectToAction("Detail", "SanPham", new { id = productId });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart is null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
            }

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (item is null)
                cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
            else
                item.Quantity = quantity;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CheckOut));
        }
     
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckoutVM model)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Challenge();

            var customer = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (customer is null)
            {
                TempData["Message"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToAction("Index", "SanPham");
            }

            ViewBag.Customer = customer;

            if (string.IsNullOrWhiteSpace(model.FullName))
                ModelState.AddModelError(nameof(model.FullName), "Nhập tên người nhận.");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError(nameof(model.Phone), "Nhập số điện thoại người nhận.");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError(nameof(model.Address), "Nhập địa chỉ nhận hàng.");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (!ModelState.IsValid)
                return View(ToCartItems(cart));

            if (cart is null || cart.CartItems.Count == 0)
            {
                TempData["Message"] = "Giỏ hàng trống.";
                return RedirectToAction(nameof(Index));
            }

            await using var tran = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in cart.CartItems)
                {
                    if (item.Product.StockQuantity < item.Quantity)
                    {
                        TempData["Message"] = $"Sản phẩm {item.Product.ProductName} không đủ hàng.";
                        await tran.RollbackAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }

                // "Giống thông tin khách hàng" = lấy địa chỉ/tên/SĐT từ hồ sơ,
                // ngược lại dùng đúng thông tin người dùng nhập cho đơn này.
                var receiverName = model.GiongKhachHang ? customer.FullName : model.FullName;
                var receiverPhone = model.GiongKhachHang ? customer.PhoneNumber : model.Phone;
                var shippingAddress = model.GiongKhachHang ? customer.Address : model.Address;

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.Pending,
                    ShippingAddress = shippingAddress ?? string.Empty,
                    ReceiverName = receiverName,
                    ReceiverPhone = receiverPhone,
                    Note = model.Notes,
                    TotalPrice = cart.CartItems.Sum(i => i.Quantity * i.Product.Price)
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cart.CartItems)
                {
                    item.Product.StockQuantity -= item.Quantity;

                    _context.OrderItems.Add(new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        SalePrice = item.Product.Price
                    });
                }

                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                _logger.LogInformation("Đơn {OrderId} được tạo bởi user {UserId}, tổng {Total}",
                    order.OrderId, userId, order.TotalPrice);

                TempData["Success"] = $"Đặt hàng thành công. Mã đơn ORD{order.OrderId:D6}.";
                return RedirectToAction("OrderDetail", "Order", new { id = order.OrderId });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Một người khác vừa mua cùng sản phẩm: rowversion không khớp.
                await tran.RollbackAsync();
                _logger.LogWarning(ex, "Xung đột tồn kho khi đặt hàng cho user {UserId}", userId);
                TempData["Message"] = "Sản phẩm vừa được người khác mua hết, vui lòng kiểm tra lại giỏ hàng.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                _logger.LogError(ex, "Lỗi khi tạo đơn hàng cho user {UserId}", userId);
                TempData["Message"] = "Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại.";
                return RedirectToAction(nameof(Index));
            }
        }

        private static List<CartItems> ToCartItems(Cart? cart) =>
            cart?.CartItems.Select(ci => new CartItems
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.ProductName,
                Price = ci.Product.Price,
                Quantity = ci.Quantity,
                Image = ci.Product.ImageUrl ?? string.Empty
            }).ToList() ?? new List<CartItems>();
    }
}
