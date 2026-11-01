using System.Text;
using HTNLShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTNLShop.Controllers
{
    /// <summary>Sitemap động cho công cụ tìm kiếm (trang chủ, danh mục, sản phẩm).</summary>
    public class SitemapController : Controller
    {
        private readonly HtlnshopContext _db;

        public SitemapController(HtlnshopContext db) => _db = db;

        [HttpGet]
        [Route("/sitemap.xml")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var products = await _db.Products.AsNoTracking()
                .Select(p => p.ProductId)
                .ToListAsync();
            var categories = await _db.Categories.AsNoTracking()
                .Select(c => c.CategoryId)
                .ToListAsync();

            var xml = new StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xml.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            void Add(string loc)
            {
                xml.AppendLine($"  <url><loc>{System.Security.SecurityElement.Escape(loc)}</loc></url>");
            }

            Add($"{baseUrl}/");
            Add($"{baseUrl}/SanPham");
            foreach (var categoryId in categories)
                Add($"{baseUrl}/SanPham?id={categoryId}");
            foreach (var productId in products)
                Add($"{baseUrl}/SanPham/Detail/{productId}");

            xml.AppendLine("</urlset>");

            return Content(xml.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
