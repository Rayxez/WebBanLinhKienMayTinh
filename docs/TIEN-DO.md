# Tiến độ thực hiện — HTNL Shop

Cập nhật: 01/11/2026. Mục tiêu: hoàn thiện trong 1 tuần để đưa vào CV tuyển dụng (dùng ASP.NET Core Identity).

## Đã xong — Giai đoạn 1: bảo mật & chạy được

| Hạng mục | Trạng thái | Bằng chứng |
|---|---|---|
| Chuyển sang ASP.NET Core Identity (hash PBKDF2, lockout 5 lần/15 phút) | ✅ | `Program.cs`, `Data/User.cs`, `Data/DbSeeder.cs` |
| Bỏ lỗi đăng ký thành Admin (role do server gán = Customer) | ✅ | `Controllers/CustomerController.cs:56`, test tay: `tester01` → role `Customer` |
| Bảo vệ toàn bộ khu vực quản trị | ✅ | `Areas/Admin/Controllers/AdminBaseController.cs`; ẩn danh `/Admin/...` → 302 |
| CSRF toàn cục + token cho AJAX | ✅ | `Program.cs` (`AutoValidateAntiforgeryToken`); POST thiếu token → 400 |
| Sửa IDOR (+ `[Authorize]` cho đơn hàng) | ✅ | `Controllers/OrderController.cs` |
| Vòng đời đơn hàng: mặc định "Chờ xác nhận" | ✅ | `Data/OrderStatus.cs`, `CartController.CheckOut` |
| Lưu `SalePrice` + thông tin người nhận | ✅ | Đơn test: 36.780.000đ, `SalePrice` 18.390.000, tồn kho 10 → 8 |
| Trang lỗi 404/403/500 thân thiện | ✅ | `Controllers/HomeController.cs`, `Views/Shared/StatusCode.cshtml` |
| Security headers | ✅ | Middleware trong `Program.cs` |
| Migration khởi tạo + seeder idempotent | ✅ | `Migrations/20261031172139_InitialCreate.cs`, `Data/DbSeeder.cs` |
| README hướng dẫn chạy | ✅ | `README.md` |
| Sửa lỗi dashboard 500 (aggregate lồng nhau) | ✅ | `Areas/Admin/Controllers/DashBoard.cs` (TopSellingProducts) |

**Kết quả kiểm chứng (chạy thật trên localhost):**

- `/` → 200; `/Admin/QuanLySanPham/Index` (ẩn danh) → 302; đăng nhập admin → 200
- Đăng ký mới → role `Customer`; POST thiếu CSRF → 400
- Đặt hàng → đơn `Chờ xác nhận`, có người nhận riêng, `SalePrice` đúng, tồn kho giảm
- Dashboard, danh sách người dùng, danh sách đơn, sửa người dùng → 200
- Build: **0 lỗi, 65 cảnh báo**

## Đã xong — Giai đoạn 2: kiểm thử, CI, Docker

| Hạng mục | Trạng thái | Bằng chứng |
|---|---|---|
| Project test xUnit + `WebApplicationFactory` | ✅ | `tests/HTNLShop.Tests`, CSDL riêng mỗi lần chạy |
| Ma trận phân quyền 5 URL × 3 vai trò | ✅ | `AuthorizationMatrixTests.cs` (15 case) |
| Test CSRF (POST thiếu token → 400) | ✅ | `SecurityTests.cs` |
| Test checkout end-to-end (trạng thái, `SalePrice`, tồn kho) | ✅ | `CheckoutTests.cs` |
| Test domain + health check + security headers | ✅ | `DomainTests.cs`, `SecurityTests.cs` |
| **41/41 test xanh** | ✅ | `dotnet test` — coverage **57,1% dòng** (mã tự viết, loại migration/Razor generated) |
| GitHub Actions: build, test + coverage, quét gói lỗ hổng, Docker build | ⚠️ viết xong, **chưa chạy trên GitHub** | `.github/workflows/ci.yml` (máy chưa có Git để push; đã mô phỏng bằng `dotnet build -c Release` + `dotnet test -c Release`) |
| Dockerfile multi-stage, chạy non-root, cổng 8080 | ⚠️ viết xong, **chưa build image** | `Dockerfile`; `dotnet publish` (đúng lệnh trong Dockerfile) chạy thành công, output có `HTNLShop.dll` + appsettings |
| docker-compose: web + SQL Server + health check | ✅ | `docker-compose.yml` |
| Health endpoint | ✅ | `GET /health` → `ok` |
| Seeder không tạo tài khoản mặc định ngoài Development | ✅ | `DbSeeder` (bắt buộc `Seed__AdminPassword`) |

## Đã xong — Giai đoạn 3: nghiệp vụ đơn hàng nâng cao & tài sản CV

| Hạng mục | Trạng thái | Bằng chứng |
|---|---|---|
| Luật chuyển trạng thái đơn (6 trạng thái) | ✅ | `Data/OrderStatus.cs` (`CanTransition`, `Next`, `RestoresStock`) |
| Admin đổi trạng thái đơn trên UI | ✅ | `QuanLyDonHang.UpdateStatus` + nút thao tác trong `Views/QuanLyDonHang/Index.cshtml` |
| Huỷ đơn hoàn lại tồn kho | ✅ | Test `Huy_don_hoan_lai_dung_so_luong_ton_kho` |
| Chặn nhảy cóc trạng thái | ✅ | Test `Khong_the_nhay_coc_tu_cho_xac_nhan_sang_da_giao` |
| Chống oversell bằng `rowversion` | ✅ | `Product.RowVersion`, migration `AddProductRowVersion`, bắt `DbUpdateConcurrencyException` |
| Ảnh chụp màn hình (7 ảnh) | ✅ | `docs/screenshots/` |
| Sơ đồ kiến trúc + luồng đặt hàng (Mermaid) | ✅ | `docs/architecture.md` |
| README có ảnh + mô tả kiến trúc | ✅ | `README.md` |

## Đã xong — Giai đoạn 4: tiền tệ, hiệu năng, SEO

| Hạng mục | Trạng thái | Bằng chứng |
|---|---|---|
| `decimal(18,2)` cho `Products.Price`, `Orders.TotalPrice`, `OrderItems.SalePrice` | ✅ | migration `DecimalMoney`; DB kiểm tra `numeric_precision=18, scale=2` |
| Nén ảnh sản phẩm | ✅ | **108,8 MB → 23,1 MB** (485 ảnh, max 1000px, quality 82); `wwwroot` 121,2 → 35,5 MB |
| Sửa ảnh chi tiết dùng `Model.ImageUrl` (trước hard-code đường dẫn) | ✅ | `Views/SanPham/Detail.cshtml` |
| Title/meta động + Open Graph | ✅ | `_LayoutMain.cshtml`, `SanPhamController.Detail` |
| `sitemap.xml` động (trang chủ, 16 danh mục, 480 sản phẩm) | ✅ | `Controllers/SitemapController.cs`, test 200 + nội dung XML |
| `robots.txt` chặn `/Admin`, `/Order`, `/Cart/CheckOut` | ✅ | `wwwroot/robots.txt` |
| Cache tĩnh 7 ngày | ✅ | `Program.cs` (`StaticFileOptions`) |
| **44/44 test xanh**, coverage **60,8% dòng** | ✅ | `dotnet test` + `coverlet.runsettings` |

## Tiếp theo (theo thứ tự ưu tiên)

1. **Thương mại**: thanh toán VNPay/MoMo sandbox, email giao dịch, phí ship, coupon.
2. **Vận hành**: deploy staging/production, backup/restore drill, health monitoring.
3. **Đo lường**: load test k6, Lighthouse, OWASP ZAP baseline; lưu báo cáo vào `docs/`.
4. **Portfolio**: video demo 3–5 phút theo kịch bản trong `docs/CV_VA_DEMO.md`, cập nhật bullet CV lần cuối.

## Ghi chú kỹ thuật

- Máy hiện tại chỉ có runtime .NET 10 → chạy thử bằng `DOTNET_ROLL_FORWARD=LatestMajor`.
- `dotnet-ef` 8.0.11 đã cài global, cần biến `DOTNET_ROLL_FORWARD=LatestMajor` khi chạy trên máy này.
- Migration cũ (schema trước Identity) đã được thay bằng migration khởi tạo mới; CSDL LocalDB đã được tạo lại.
- Chưa có Docker và Git trên máy → CI/Docker đã viết xong nhưng chỉ kiểm chứng được bằng `dotnet publish` và kiểm tra cú pháp YAML; cần cài Docker Desktop + Git để chạy thật và push lên GitHub.
