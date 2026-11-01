# HTNL Shop — Báo cáo đánh giá & lộ trình nâng cấp lên chuẩn sản phẩm thương mại

> **Phạm vi:** toàn bộ mã nguồn tại `D:\Project\sellPc_final\HTNLShop` (ASP.NET Core MVC 8, EF Core 9, SQL Server).
> **Cách đánh giá:** đọc mã nguồn + biên dịch thật + chạy thử ứng dụng trên localhost + đối chiếu bộ tiêu chí sản phẩm thương mại.
> **Ngày:** 31/10/2026.

---

## 0. TL;DR — Kết luận trong 60 giây

**Điểm mạnh:** đây là một đồ án có *nghiệp vụ thật*: catalogue 480 sản phẩm, giỏ hàng kép (session + DB), đặt hàng có transaction, đánh giá, dashboard quản trị có KPI, phân quyền theo role, giao diện bám template bán hàng. Nền tảng để nâng cấp là có.

**Điểm chặn:** dự án **chưa thể gọi là sản phẩm thương mại**, và hiện tại **không thể chạy được ngay sau khi clone**. Ba nhóm vấn đề lớn nhất:

1. **Lỗ hổng bảo mật nghiêm trọng, có thể khai thác ngay:** đăng ký xong là thành Admin; toàn bộ khu vực quản trị (sản phẩm, người dùng, đơn hàng) **không có `[Authorize]`**; mật khẩu lưu **plaintext**; 11/15 action POST thiếu chống CSRF (một action còn thiếu cả `[HttpPost]`); một action trả về đơn hàng của **user id = 1** cho bất kỳ ai.
2. **Nghiệp vụ đơn hàng sai bản chất:** đơn mới tạo bị gán ngay `"Đã giao hàng"`; `OrderItem.SalePrice` không được gán (bằng 0đ); địa chỉ giao hàng lấy ngược điều kiện; form checkout thu `FullName/Phone/Notes` nhưng controller bỏ qua.
3. **Không có nền tảng kỹ thuật để vận hành:** 0 test, 0 CI/CD (thư mục workflow rỗng), không Docker, không logging/health check, `Migrations/` bị `.gitignore`, README 26 byte, DB LocalDB không tồn tại → 100% trang trả **HTTP 500** khi vừa clone.

**Kết luận định lượng:** 12/60 điểm theo thang 12 trụ cột (mục 4). Mục tiêu khả thi sau **8 tuần × ~20 giờ** (tổng ~158 giờ công: 20 / 48 / 60 / 30) là **≥ 45/60** — mức "một sản phẩm nhỏ nhưng vận hành được, bảo mật được, có bằng chứng kỹ thuật để đưa vào CV".

**Việc cần làm ngay (tuần 1):** 9 việc trong mục 5.1, tổng ~18–22 giờ, chặn được toàn bộ lỗ hổng P0.

---

## 1. Hồ sơ dự án (số liệu đo được, không phỏng đoán)

| Hạng mục | Số liệu | Ghi chú |
|---|---|---|
| Mã C# (trừ `Migrations`, `wwwroot`) | **45 tệp / 3.309 dòng** | 14 controller, 8 entity, 12 view-model, 4 helper |
| Giao diện Razor | **43 tệp / 4.527 dòng** | 1 layout khách + 1 layout admin, 43 view |
| Dữ liệu seed | **480 sản phẩm**, 16 danh mục, 4 user, 2 role | nhúng trực tiếp trong `HtlnshopContext.OnModelCreating` |
| Tài nguyên tĩnh | **833 tệp / 121,2 MB** (riêng ảnh sản phẩm 109,3 MB) | nhiều ảnh 1–3,3 MB, không resize/WebP |
| Kiểm thử | **0 tệp** | không có project test |
| CI/CD | **0 workflow** | `.github/workflows` rỗng |
| Build | **0 lỗi / 75 cảnh báo** (nullable, `int.Parse` có thể null) | `dotnet build` trên SDK 10, target `net8.0` |
| Chạy thử | **Không chạy được "out of the box"** | thiếu DB `HTLNShop`, máy chỉ có runtime .NET 10 |
| Tài liệu | `README.md` = **26 byte** (`# HTNLShop`, UTF-16) | không có hướng dẫn cài đặt/kiến trúc |

### 1.1 Chức năng hiện có

| Nhóm | Đã có | Chưa có (bắt buộc với thương mại) |
|---|---|---|
| Catalogue | danh mục, chi tiết, tìm theo tên, sắp xếp theo giá, phân trang 9/trang | lọc hãng/khoảng giá/thông số, so sánh, sản phẩm biến thể, tồn kho theo SKU |
| Giỏ hàng | session cho khách, DB cho user, merge khi đăng nhập, +/- số lượng, kiểm tra tồn kho | giữ hàng có thời hạn, đồng bộ đa thiết bị có kiểm soát, coupon |
| Đặt hàng | checkout, transaction, trừ tồn kho, lịch sử đơn, chi tiết đơn | vòng đời trạng thái, huỷ/đổi trả, phí ship, thanh toán online, hoá đơn, email |
| Tài khoản | đăng ký, đăng nhập cookie, đăng xuất | hash mật khẩu, quên/đổi mật khẩu, xác thực email, khoá tài khoản, địa chỉ nhiều nơi |
| Đánh giá | form + danh sách theo sản phẩm | kiểm duyệt, xác thực "đã mua", chống spam, phản hồi của shop |
| Quản trị | dashboard KPI, CRUD sản phẩm/người dùng, danh sách đơn, upload ảnh | phân quyền chi tiết, audit log, đổi trạng thái đơn, nhập/xuất Excel, báo cáo đúng |

### 1.2 Mức trưởng thành

```
Cấp 1  Demo chạy được trên máy tác giả        ← đang ở đây (một phần)
Cấp 2  Người khác clone về chạy được           ← mục tiêu tuần 1–2
Cấp 3  Có test + CI + môi trường staging       ← mục tiêu tuần 3
Cấp 4  Có thanh toán, vòng đời đơn, quan sát   ← mục tiêu tuần 6
Cấp 5  Vận hành thật, có SLA, có người dùng    ← ngoài phạm vi 8 tuần
```

---

## 2. Điểm mạnh — giữ lại, đừng viết lại từ đầu

1. **Mô hình dữ liệu nghiệp vụ đúng hướng**: `Product – Category – Cart – CartItem – Order – OrderItem – Review – User – Role`, có khoá ngoại và chỉ mục duy nhất `UQ_Cart_Product`.
2. **Đã dùng EF Core + Migrations** (dù bị gitignore) thay vì chắp vá SQL thuần.
3. **Luồng giỏ hàng kép có chủ đích**: khách vãng lai dùng session, đăng nhập thì merge vào DB — đây là chi tiết nghiệp vụ mà nhiều đồ án bỏ qua.
4. **Checkout có transaction** và kiểm tra tồn kho trước khi trừ (`CartController.CheckOut`).
5. **Cookie authentication có kiểm tra role mỗi request** (`OnValidatePrincipal`) — tư duy đúng, chỉ thiếu cấu hình.
6. **Dashboard có KPI thật** (doanh thu, đơn chờ, tồn thấp, top bán chạy) — chất liệu tốt để kể chuyện trong CV.
7. **Giao diện đủ đẹp để demo**: template bán hàng + admin dashboard, responsive cơ bản.
8. **Có ý thức tách ViewModel** (`ProductVM`, `CartItems`, `CheckoutVM`…) thay vì bind thẳng entity ở mọi nơi.

---

## 3. Phát hiện chi tiết (có bằng chứng `tệp:dòng`)

### 3.1 P0 — Chặn phát hành (bảo mật & khả năng chạy)

| ID | Vấn đề | Bằng chứng | Tác động |
|---|---|---|---|
| **P0-1** | **Đăng ký → thành Admin.** `RegisterVM` khai báo `RoleId = 1`; controller gán `RoleId = 1`; seed quy định `RoleId = 1` là `Admin`. | `ViewModels/RegisterVM.cs:36`, `Controllers/CustomerController.cs:48`, `Data/HtlnshopContext.cs:156-159` | Bất kỳ ai đăng ký cũng có toàn quyền quản trị. Lỗi leo thang đặc quyền (CWE-269) mức Critical. |
| **P0-2** | **Khu vực quản trị không được bảo vệ.** 4 controller admin thiếu `[Authorize]`; chỉ `AdminController` có. | `Areas/Admin/Controllers/QuanLySanPham.cs:11`, `QuanLyNguoiDung.cs:11`, `QuanLyDonHang.cs:8`, `DashBoard.cs:8` (so với `AdminController.cs:17`) | Ẩn danh gọi được CRUD sản phẩm/người dùng, xem toàn bộ đơn hàng. Chạy thử: `GET /Admin/QuanLySanPham/Index` **không bị chuyển về login** (trả 500 vì DB thiếu, tức là đã vào tới action). |
| **P0-3** | **Mật khẩu plaintext**, cột `varchar(20)`, seed `admin/admin`. So sánh mật khẩu bằng `==` trong LINQ. | `Data/User.cs:20`, `Data/HtlnshopContext.cs:140-142,162`, `Controllers/CustomerController.cs:75-79` | Lộ toàn bộ mật khẩu nếu lộ DB; không thể đáp ứng bất kỳ tiêu chuẩn nào (OWASP A02/A07). |
| **P0-4** | **IDOR đơn hàng.** `FilterOrders` hard-code `userId = 1`; `OrderController` không có `[Authorize]` → `int.Parse(null)` ném lỗi 500. | `Controllers/OrderController.cs:68-71,118-125,169-172` | Xem đơn của người khác; người dùng chưa đăng nhập nhận 500 thay vì 401/302. |
| **P0-5** | **Thiếu CSRF + overposting.** 15 action `[HttpPost]`, chỉ **4** có `[ValidateAntiForgeryToken]` (3 ở Categories, 1 ở Review); toàn bộ admin CRUD và checkout không có, cũng không có filter toàn cục. `EditProduct` còn **thiếu `[HttpPost]`** nên bind được cả qua GET. | `Areas/Admin/Controllers/*.cs`, `Controllers/CartController.cs:530`, `QuanLySanPham.cs:122` | Kẻ tấn công dụ admin/khách submit form ngoài site: xoá sản phẩm, tạo đơn, đổi dữ liệu; hoặc sửa sản phẩm qua URL GET. Overposting cho phép ghi cả `ProductId`, `ImageUrl` ngoài ý muốn. |
| **P0-6** | **Xử lý lỗi hỏng.** `UseExceptionHandler("/Home/Error")` nhưng **không tồn tại `HomeController`**; Development bật stack trace + chuỗi kết nối. | `Program.cs:59-62`; chạy thử `GET /` → 500 kèm `SqlException` và tên đăng nhập Windows | Rò rỉ thông tin hệ thống; production ra trang trắng 500. |
| **P0-7** | **Không chạy được sau clone.** LocalDB không có DB `HTLNShop`; `Migrations/` bị ignore; không có script seed/README; máy chỉ có runtime .NET 10. | `appsettings.json:9`, `.gitignore` (dòng `Migrations/`), `README.md` (26 byte) | Nhà tuyển dụng/đồng đội bấm F5 là lỗi → mất điểm ngay vòng đầu. |
| **P0-8** | **Đánh giá ẩn danh gây lỗi FK.** `[AllowAnonymous]` + `UserId = userId ?? 0`; không kiểm tra đã mua. | `Controllers/ReviewController.cs:53-75` | Gửi đánh giá khi chưa đăng nhập → `UserId = 0` không tồn tại → 500; ngược lại spam đánh giá không kiểm soát. |

### 3.2 P1 — Sai nghiệp vụ & nợ kỹ thuật (phải sửa trước khi demo "thương mại")

| ID | Vấn đề | Bằng chứng | Tác động |
|---|---|---|---|
| **P1-1** | Đơn mới tạo bị gán `Status = "Đã giao hàng"` ngay lập tức (cả 2 nhánh checkout). | `CartController.cs:565,630` | Dashboard tính doanh thu trên đơn chưa giao; không có vòng đời đơn; khách không thể theo dõi. |
| **P1-2** | `OrderItem.SalePrice` không được gán ở luồng giỏ hàng → **0đ**. | `CartController.cs:641-647` | Dữ liệu đơn vĩnh viễn sai; báo cáo doanh thu theo item sai. |
| **P1-3** | Chi tiết đơn hiển thị `Product.Price` hiện tại thay vì giá đã bán. | `OrderController.cs:197` | Sửa giá sản phẩm làm thay đổi hoá đơn quá khứ. |
| **P1-4** | Địa chỉ giao hàng lấy **ngược** logic; `FullName`, `Phone`, `Notes` của form bị bỏ qua. | `CartController.cs:564,629` + `Views/Cart/CheckOut.cshtml:26-43` | Không giao được đến người nhận khác; dữ liệu khách nhập bị mất. |
| **P1-5** | Luồng "Mua ngay" chết: hidden `BuyNowProductId` bị comment; `BuyNow` GET sửa `Quantity` trên entity tracked mà không `SaveChanges`. | `Views/Cart/CheckOut.cshtml:13-16`, `CartController.cs:487-528` | Nút mua ngay đưa vào checkout của cả giỏ; thay đổi số lượng có thể bị ghi nhầm ở request sau. |
| **P1-6** | Tiền dùng `double`/`float` (`Product.Price`, `Orders.TotalPrice`). | `Data/Product.cs:13`, `Data/Order.cs:14`, `Data/HtlnshopContext.cs` | Sai số làm tròn; không đối soát được kế toán. Chuẩn: `decimal(18,2)`. |
| **P1-7** | Không có kiểm soát đồng thời tồn kho (không `RowVersion`). | `Data/Product.cs`, `CartController.cs:616-651` | Oversell khi 2 khách mua cùng SKU còn 1 chiếc. |
| **P1-8** | Controller-as-service: `CustomerController` phụ thuộc `CartController`, và `AddScoped<CartController>()`. | `CustomerController.cs:18,20,24`, `Program.cs:13` | Vi phạm vòng đời DI, không test được, dễ vòng lặp phụ thuộc. |
| **P1-9** | Hai nguồn giỏ hàng với logic nhân bản ~600 dòng; merge không giới hạn theo tồn kho. | `CartController.cs:45-392,393-449` | Bán vượt tồn kho ngay sau khi đăng nhập; sửa 1 chỗ phải sửa 3 nơi. |
| **P1-10** | Dashboard chạy **24 truy vấn trong 2 vòng lặp** (12 tháng × 2); danh sách đơn admin `ToList()` không phân trang; nhiều `Include` thừa. | `DashBoard.cs:83-133`, `QuanLyDonHang.cs:21-45` | Trang chậm dần theo dữ liệu; N+1 tiềm ẩn. |
| **P1-11** | Schema lệch nhau: `SQLQuery1.sql` **không có bảng Review**, `Password varchar(20)`; EF map `Review` → `tb_Review`; snapshot migration 303 KB; 480 sản phẩm seed trong `OnModelCreating`. | `SQLQuery1.sql`, `Data/Review.cs:6`, `Migrations/*` | Không có nguồn sự thật duy nhất về schema; build/migration chậm; không thể quản lý catalogue thật. |
| **P1-12** | Ảnh: 109 MB, file tới 3,3 MB, không resize/WebP; view chi tiết **hard-code** đường dẫn ảnh thay vì `Model.ImageUrl`; upload không kiểm tra dung lượng/định dạng. | `wwwroot/Assets/img`, `Views/SanPham/Detail.cshtml:26`, `QuanLySanPham.cs:65-100` | Ảnh sai với sản phẩm đặt tên khác; tải trang rất chậm; rủi ro upload file độc hại. |
| **P1-13** | Rác template trong `wwwroot/Admin` (`signin.html`, `signup.html`, `index.html`…) được phục vụ công khai. | chạy thử: `GET /Admin/signin.html` → **200** | Rủi ro thương hiệu/phishing; tăng dung lượng deploy. |
| **P1-14** | Không có audit, soft-delete, `CreatedAt/UpdatedAt`; xoá user/sản phẩm là xoá cứng. | `QuanLyNguoiDung.cs:91-103`, `QuanLySanPham.cs:167-190` | Mất dấu vết; xoá user có đơn hàng sẽ vỡ FK. |
| **P1-15** | `ProFile.cshtml` in `User.Claims.ElementAt(3).Value` — phụ thuộc thứ tự claim, không có chức năng hồ sơ. | `Views/Customer/ProFile.cshtml:7` | Trang hồ sơ vô nghĩa, dễ vỡ khi đổi claim. |

### 3.3 P2 — Vận hành, chất lượng, trình diễn

| ID | Vấn đề | Bằng chứng |
|---|---|---|
| P2-1 | 0 test, 0 CI, 0 Docker, không có `.editorconfig`/format. | `.github/workflows` rỗng; không có `*Tests*` |
| P2-2 | Không HTTPS redirect/HSTS/security headers; `UseStaticFiles` gọi 2 lần; không nén/Bundling. | `Program.cs:58,64`; `launchSettings.json` chỉ có profile `http` |
| P2-3 | Cookie auth thiếu `SameSite`, `Secure`, tên cookie riêng; Data Protection key không lưu bền → restart là logout toàn bộ, không scale-out được. | `Program.cs:30-54` |
| P2-4 | Không logging tập trung, không health check, không `Application Insights`/Serilog; lỗi chỉ `Console.WriteLine` + `ex.Message`. | `CartController.cs:447,662` |
| P2-5 | SEO/UX: `<title>` rỗng hoặc "Detail", meta keywords/description rỗng, `alt` sai, 2 bản jQuery/Bootstrap (CDN + local), CSS inline 100 dòng, không accessibility. | `_LayoutMain.cshtml:6-9`, `Detail.cshtml:10` |
| P2-6 | Không có trang 404/500 thân thiện; route `/404` tự chế; không `UseStatusCodePages`. | `Program.cs`, `SanPhamController.cs:77-81` |
| P2-7 | Thiếu năng lực thương mại: thanh toán, phí ship, coupon, email, hoá đơn, huỷ/đổi trả, quên mật khẩu, xác thực email, wishlist, lọc theo hãng/giá. | — |
| P2-8 | Hard-code tiếng Việt trong controller/view; không resource/i18n. | toàn bộ |
| P2-9 | Git hygiene: `Migrations/` bị ignore; `bin/`, `obj/` còn trong cây làm việc; không có tag/release. | `.gitignore` |
| P2-10 | 75 cảnh báo nullable (CS8601/CS8602/CS8604) — dấu hiệu null-check chưa hoàn chỉnh. | output build |

---

## 4. Scorecard "chuẩn sản phẩm thương mại" (12 trụ cột)

Thang 0–5. Cột "mục tiêu" là mức đủ để gọi là sản phẩm nhỏ vận hành được và đưa vào CV.

| # | Trụ cột | Hiện tại | Mục tiêu | Bằng chứng cần có sau nâng cấp |
|---|---|---|---|---|
| 1 | Bảo mật ứng dụng (OWASP) | **1** | 4 | không còn plaintext, có CSRF toàn cục, headers, rate limit, ZAP baseline sạch mức High |
| 2 | Xác thực & phân quyền | **1** | 4 | ASP.NET Identity, policy Admin/Staff/Customer, test ma trận quyền |
| 3 | Đúng nghiệp vụ đơn hàng/thanh toán | **1** | 4 | state machine + payment webhook idempotent + đối soát |
| 4 | Mô hình dữ liệu & toàn vẹn | **2** | 4 | decimal, unique index, RowVersion, FK Restrict, soft delete |
| 5 | Kiến trúc & khả năng bảo trì | **2** | 4 | 4 lớp, không controller-as-service, DTO/validator, 0 warning |
| 6 | Kiểm thử | **0** | 4 | ≥ 60% coverage, 100% luồng tiền có test |
| 7 | CI/CD | **0** | 4 | pipeline xanh, build+test+scan+deploy 1 lệnh |
| 8 | Vận hành & quan sát | **0** | 3 | Serilog + health check + dashboard lỗi + backup drill |
| 9 | Hiệu năng | **1** | 3 | p95 < 300 ms, LCP < 2,5 s, ảnh < 15 MB tổng |
| 10 | UX/UI & accessibility | **3** | 4 | Lighthouse ≥ 90, keyboard/alt/contrast, empty/error states |
| 11 | SEO & đo lường | **1** | 3 | meta động, sitemap, JSON-LD, GA4 + sự kiện |
| 12 | Tài liệu & bàn giao | **0** | 4 | README chạy 3 lệnh, ADR, API docs, runbook |
| | **Tổng** | **12/60** | **45/60** | |

> Cách đọc: 12/60 = "đồ án demo". 30/60 = "MVP chạy được". 45/60 = "sản phẩm nhỏ, tuyển dụng được". 55+/60 = "vận hành thật".

### 4.1 Nếu đây là đồ án môn học / đồ án tốt nghiệp

| Tiêu chí chấm thường gặp | Hiện trạng | Nhận xét |
|---|---|---|
| Đủ chức năng CRUD + luồng mua | 8/10 | tốt, có giỏ hàng kép và dashboard |
| Giao diện, trải nghiệm | 7/10 | đẹp nhưng còn copy template, thiếu trạng thái lỗi/rỗng |
| Bảo mật & phân quyền | 2/10 | **điểm rơi nặng nhất** |
| Kiến trúc, chất lượng mã | 5/10 | có ViewModel nhưng controller quá tải, logic trùng lặp |
| Kiểm thử, triển khai | 1/10 | gần như không có |
| Tài liệu, thuyết trình | 2/10 | README rỗng, không sơ đồ |
| **Khả năng ghi vào CV** | 4/10 | kể được nghiệp vụ, nhưng người phỏng vấn sẽ hỏi đúng 3 chỗ đang hỏng: auth, test, deploy |

---

## 5. Lộ trình nâng cấp 8 tuần

Giả định: 1 người, ~20 giờ/tuần, vẫn giữ MVC + EF Core + SQL Server (không đổi stack — đổi stack lúc này là mất điểm). Mỗi giai đoạn kết thúc bằng một **bản demo được** và **một cột mốc đo được**.

**Tổng ước lượng: ~158 giờ** — GĐ0 20,5 h · GĐ1 48 h · GĐ2 60 h · GĐ3 30 h. Nếu mỗi tuần chỉ có 10 giờ, lộ trình thành 14–16 tuần; nội dung và thứ tự giữ nguyên, chỉ giãn lịch.

### Giai đoạn 0 — "An toàn để demo" (Tuần 1, ~20 giờ)

Mục tiêu: vá hết P0 + P1-1..P1-4, dựng được DB từ đầu, chạy 3 lệnh.

| ID | Việc | Ước lượng | Nghiệm thu |
|---|---|---|---|
| 0.1 | Sửa `RoleId`: khách = 2; bỏ field `RoleId` khỏi `RegisterVM`; không cho client gửi role | 0,5 h | Đăng ký mới tạo role Customer; test: không thể tự nâng quyền |
| 0.2 | Bảo vệ toàn bộ area Admin: `[Authorize(Roles="Admin")]` ở base controller hoặc convention `AuthorizeFilter` cho area | 1 h | Ẩn danh → 302 login; Customer → 403; test 4 route × 3 vai trò |
| 0.3 | Hash mật khẩu: `PasswordHasher<User>` (hoặc migrate sang Identity ở GĐ 1), seed hash sẵn, đổi `admin/admin` | 2 h | DB không còn mật khẩu đọc được; login cũ vẫn hoạt động sau migration |
| 0.4 | Bật CSRF toàn cục `AutoValidateAntiforgeryToken`; kiểm tra mọi form có token | 1 h | POST thiếu token → 400 |
| 0.5 | Sửa `FilterOrders` + `[Authorize]` cho `OrderController`; dùng `User.FindFirstValue` an toàn | 1 h | Không còn `userId = 1`; ẩn danh → login |
| 0.6 | Vòng đời đơn: thêm enum/status `Chờ xác nhận → Đã xác nhận → Đang giao → Đã giao → Hoàn tất`, mặc định `Chờ xác nhận` | 2 h | Đơn mới không còn "Đã giao hàng"; dashboard tính doanh thu theo trạng thái hoàn tất |
| 0.7 | Gán `SalePrice` khi tạo `OrderItem`; hiển thị `SalePrice` thay vì `Product.Price` | 1 h | Đơn cũ/đơn mới có giá bán đúng |
| 0.8 | Checkout: lưu `FullName/Phone/ShippingAddress/Notes`, đảo lại logic "giống thông tin khách hàng" | 2 h | Nhập người nhận khác → hoá đơn đúng; có test |
| 0.9 | Làm chạy được từ clone: commit migrations (bỏ ignore), `README` 3 lệnh, script seed, `dotnet user-secrets` cho chuỗi kết nối | 3 h | Máy sạch: `git clone` → `dotnet ef database update` → `dotnet run` chạy |
| 0.10 | Trang lỗi 404/500 thật + `UseStatusCodePagesWithReExecute`; tắt stack trace ở Production | 1 h | `/khong-ton-tai` → 404 đẹp; lỗi DB → 500 thân thiện, không lộ SQL |
| 0.11 | Dọn `wwwroot/Admin` template, nén ảnh về ≤ 200 KB/ảnh, sửa `Detail.cshtml` dùng `Model.ImageUrl` | 3 h | Dung lượng web ≤ 20 MB; ảnh chi tiết đúng |
| 0.12 | Viết 6 test đầu tiên (xUnit): đăng ký, login, phân quyền admin, checkout tổng tiền, trừ kho, CSRF | 3 h | `dotnet test` xanh; 6 test |

**DoD GĐ 0:** 12/12 việc xong; demo 5 phút không có lỗi 500; không còn mục P0 nào; `dotnet test` xanh.

### Giai đoạn 1 — "Nền tảng kỹ thuật" (Tuần 2–3, ~48 giờ)

| ID | Việc | Ước lượng |
|---|---|---|
| 1.1 | Tách solution 4 project: `Domain / Application / Infrastructure / Web` (+ `tests`), giữ nguyên hành vi | 6 h |
| 1.2 | Chuyển sang **ASP.NET Identity** (bảng `AspNetUsers`…), giữ claim tương thích, seed admin qua seeder | 8 h |
| 1.3 | Đưa toàn bộ truy vấn vào service/repository theo use case (`PlaceOrder`, `AddToCart`, `GetDashboard`) | 8 h |
| 1.4 | `decimal(18,2)` cho tiền + migration dữ liệu; `RowVersion` cho `Product`; index `Username/Email/OrderCode` | 4 h |
| 1.5 | FluentValidation + exception middleware (`ProblemDetails`) + Serilog (file + console) | 4 h |
| 1.6 | Chuyển seed 480 sản phẩm ra `IEntityTypeConfiguration`/seeder riêng; sinh mã đơn `ORD-YYYYMMDD-xxxx` | 3 h |
| 1.7 | `Dockerfile` + `docker-compose` (web + SQL Server) + `.env.example` | 3 h |
| 1.8 | GitHub Actions: restore → build (`-warnaserror`) → test + coverage → `dotnet format --verify-no-changes` → upload artifact | 4 h |
| 1.9 | Test tích hợp `WebApplicationFactory` + SQL Server container: **ma trận phân quyền** (3 vai trò × 12 endpoint), checkout, tồn kho | 8 h |

**DoD GĐ 1:** pipeline xanh trên PR; coverage ≥ 45%; `docker compose up` chạy được; 0 warning; mọi controller < 150 dòng.

### Giai đoạn 2 — "Tính năng thương mại" (Tuần 4–6, ~60 giờ)

Ưu tiên theo giá trị tuyển dụng (thứ tự này cũng là thứ tự nên kể trong CV):

| ID | Việc | Ước lượng |
|---|---|---|
| 2.1 | **Thanh toán VNPay/MoMo sandbox**: tạo payment, callback có verify chữ ký, bảng `PaymentTransaction` unique `ExternalId`, idempotent, đối soát trạng thái | 12 h |
| 2.2 | **Vòng đời đơn + quản trị**: đổi trạng thái có kiểm tra hợp lệ, ghi audit, huỷ đơn hoàn kho, khách huỷ trước "Đã xác nhận" | 8 h |
| 2.3 | **Email giao dịch** (SMTP/SendGrid): xác nhận đơn, đổi trạng thái, đặt lại mật khẩu; template HTML + retry | 6 h |
| 2.4 | **Phí ship + địa chỉ**: bảng phí theo tỉnh, miễn phí theo ngưỡng, sổ địa chỉ, chọn khi checkout | 6 h |
| 2.5 | **Mã giảm giá**: coupon %, số tiền, điều kiện, giới hạn lượt, hạn dùng; tính lại server-side | 6 h |
| 2.6 | **Catalogue thật**: biến thể/thông số (CPU/RAM/VGA…), lọc facet + khoảng giá + hãng, sắp xếp, tìm kiếm có index | 8 h |
| 2.7 | **Pipeline ảnh**: ImageSharp resize 3 cỡ + WebP, lưu S3/Azure Blob (hoặc thư mục ngoài `wwwroot`), validate 5 MB | 4 h |
| 2.8 | **Đánh giá có kiểm duyệt**: chỉ người đã mua, 1 đánh giá/đơn/sản phẩm, admin duyệt, phản hồi của shop | 4 h |
| 2.9 | **Dashboard đúng**: doanh thu theo trạng thái, AOV, tỉ lệ huỷ, top ngành hàng, 1 query/tổng hợp thay vì 24 query, cache 60 s | 4 h |
| 2.10 | **Hoá đơn PDF** + thông tin xuất VAT | 2 h |

**DoD GĐ 2:** đặt hàng → thanh toán sandbox → email → admin đổi trạng thái → khách thấy timeline; có ảnh hưởng ứng; dashboard không còn N+1.

### Giai đoạn 3 — "Vận hành & trình diễn" (Tuần 7–8, ~30 giờ)

| ID | Việc | Ước lượng |
|---|---|---|
| 3.1 | Deploy thật (Azure App Service + Azure SQL, hoặc VPS Docker + Caddy/nginx + TLS) với 2 môi trường staging/prod | 6 h |
| 3.2 | Health check `/health` (DB + storage), Serilog sink tập trung, cảnh báo lỗi 5xx | 3 h |
| 3.3 | Backup/restore drill: dump → restore staging → kiểm tra đơn hàng | 2 h |
| 3.4 | Load test k6: 200 VU, p95 < 500 ms, error < 1%; ghi lại before/after | 4 h |
| 3.5 | OWASP ZAP baseline + `dotnet list package --vulnerable`; sửa hết High | 3 h |
| 3.6 | Lighthouse ≥ 90 (mobile), ảnh LCP, font, preload; a11y: alt, label, focus, contrast | 4 h |
| 3.7 | SEO: title/meta động, `sitemap.xml`, `robots.txt`, JSON-LD `Product`, GA4 + sự kiện `add_to_cart`/`purchase` | 3 h |
| 3.8 | Trang pháp lý: chính sách bảo mật, điều khoản, đổi trả; cookie consent nếu dùng GA4 | 2 h |
| 3.9 | Quay video demo 3–5 phút + 6 screenshot + sơ đồ kiến trúc + README có ảnh | 3 h |

**DoD GĐ 3:** có URL công khai (hoặc video + hướng dẫn), pipeline deploy tự động, bộ số liệu before/after, repo README mở là hiểu.

### 5.1 Nếu chỉ có 7 ngày (bản tối thiểu để đưa vào CV)

Làm đúng 9 việc, bỏ qua phần còn lại: **0.1, 0.2, 0.3, 0.4, 0.6, 0.7, 0.9, 0.10, 0.12**.
Kết quả: không còn lỗ hổng Critical, đơn hàng đúng trạng thái/giá, clone là chạy, có 6 test. Đây là mức tối thiểu để không bị "bắt bài" khi phỏng vấn.

---

## 6. Kiến trúc đích

### 6.1 Cấu trúc solution đề xuất

```
HTNLShop.sln
├─ src/
│  ├─ HTNLShop.Domain          # Entity, enum, value object, domain service, invariant
│  ├─ HTNLShop.Application     # Use case, DTO, validator, interface (IPayment, IEmail…)
│  ├─ HTNLShop.Infrastructure  # EF Core, repository, VNPay/MoMo, SMTP, storage, cache
│  └─ HTNLShop.Web             # MVC controller mỏng, filter, view component, area Admin
├─ tests/
│  ├─ HTNLShop.UnitTests
│  ├─ HTNLShop.IntegrationTests
│  └─ HTNLShop.E2E             # Playwright
├─ docs/ (ADR, sơ đồ, runbook)
├─ docker-compose.yml
└─ .github/workflows/ci.yml
```

Luồng phụ thuộc: `Web → Application → Domain`; `Infrastructure → Application/Domain`; **không bao giờ ngược lại**.

> **Phương án tiết kiệm thời gian (nếu không kịp tách 4 project):** giữ 1 project nhưng chia thư mục `Features/Checkout/{Controller,Service,Validator,ViewModel}`, controller chỉ điều phối. Điểm quan trọng nhất để chấm là **controller mỏng + logic có test**, không phải số project.

### 6.2 Mô hình trạng thái đơn hàng

```
           ┌──────────────┐   admin xác nhận   ┌──────────────┐
  đặt hàng │ Chờ xác nhận │ ─────────────────▶ │ Đã xác nhận  │
           └──────┬───────┘                    └──────┬───────┘
                  │ khách huỷ / quá 30'              │ đóng gói
                  ▼                                  ▼
           ┌──────────────┐                    ┌──────────────┐
           │   Đã huỷ     │◀─── huỷ ───────────│  Đang giao   │
           └──────────────┘                    └──────┬───────┘
                                                     │ giao thành công
                                                     ▼
                                              ┌──────────────┐
                                              │   Đã giao    │──▶ Hoàn tất (sau 7 ngày)
                                              └──────┬───────┘
                                                     │ trả hàng
                                                     ▼
                                              ┌──────────────┐
                                              │  Trả hàng    │
                                              └──────────────┘
```
Quy tắc: mỗi chuyển trạng thái là **một method domain** kiểm tra hợp lệ + ghi audit; thanh toán là trạng thái **độc lập** (`Chưa trả / Đã trả / Hoàn tiền`).

Tồn kho: **giữ chỗ** khi đơn `Chờ xác nhận`, **trừ thật** khi `Đã xác nhận`, **hoàn** khi huỷ/hết hạn. Dùng `RowVersion` + transaction để chống oversell.

### 6.3 Bảng ánh xạ anti-pattern → pattern (dùng để trả lời phỏng vấn)

| Hiện tại | Nên thay bằng | Vì sao |
|---|---|---|
| `CustomerController` gọi `CartController` | `ICartService` | controller không phải service; dễ test |
| Logic trong action 200 dòng | Command/Query handler nhỏ | đọc, test, review được |
| `double` cho tiền | `decimal` + `Money` value object | chính xác kế toán |
| `Status` là chuỗi tiếng Việt | enum + state machine | không còn magic string, dashboard đúng |
| Seed 480 sản phẩm trong `OnModelCreating` | `IEntityTypeConfiguration` + seeder/CSV | migration nhẹ, dữ liệu thật tách khỏi schema |
| `if (User.Identity != null)` rải rác | policy + `[Authorize]` + current-user service | an toàn mặc định |
| `Include` tràn lan + `ToList()` | projection `Select` + `AsNoTracking` + phân trang | hiệu năng |
| `Console.WriteLine(ex.Message)` | `ILogger` + Serilog + correlation id | quan sát được |
| Try/catch trong controller | exception middleware + ProblemDetails | nhất quán |

---

## 7. Kế hoạch kiểm thử

| Tầng | Công cụ | Phạm vi | Mục tiêu |
|---|---|---|---|
| Unit | xUnit + FluentAssertions | domain (tổng tiền, giảm giá, chuyển trạng thái), validator | ≥ 80% domain |
| Integration | `WebApplicationFactory` + Testcontainers SQL Server + Respawn | đăng ký/login, **ma trận phân quyền**, checkout, tồn kho, CSRF | 100% endpoint có test quyền |
| E2E | Playwright | 5 kịch bản: tìm → chi tiết → giỏ → đặt hàng → lịch sử; admin đổi trạng thái; khách bị chặn admin | chạy trên CI |
| Phi chức năng | k6, ZAP baseline, Lighthouse CI | tải, bảo mật, hiệu năng | ngưỡng ở mục 8 |
| Tĩnh | `dotnet format`, analyzers, `dotnet list package --vulnerable`, CodeQL | toàn repo | 0 warning, 0 gói lỗi thời |

**Ma trận phân quyền tối thiểu** (12 endpoint × 3 vai trò = 36 case, viết dạng `[Theory]`):

| Endpoint | Ẩn danh | Customer | Admin |
|---|---|---|---|
| `GET /Admin/QuanLySanPham/Index` | 302 login | 403 | 200 |
| `POST /Admin/QuanLySanPham/DeleteProduct` | 302/400 | 403 | 200 |
| `GET /Order/Index` | 302 login | 200 (chỉ đơn mình) | 200 |
| `GET /Order/FilterOrders?status=...` | 302 login | 200 (không thấy đơn người khác) | 200 |
| `POST /Cart/CheckOut` | 302 login | 200 | 200 |

Đây cũng chính là **bằng chứng mạnh nhất trong CV**: "36 test phân quyền, bắt được 5 lỗi leo thang đặc quyền".

---

## 8. CI/CD, hạ tầng, vận hành

### 8.1 Pipeline tối thiểu (GitHub Actions)

```yaml
name: ci
on: [push, pull_request]
jobs:
  build-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: '8.0.x' }
      - run: dotnet restore
      - run: dotnet build --no-restore -warnaserror
      - run: dotnet test --no-build --collect:"XPlat Code Coverage" --results-directory ./coverage
      - run: dotnet format --verify-no-changes
      - run: dotnet list package --vulnerable --include-transitive
      - uses: actions/upload-artifact@v4
        with: { name: coverage, path: ./coverage }
  docker:
    needs: build-test
    if: github.ref == 'refs/heads/main'
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: docker build -t htnlshop:${{ github.sha }} .
      # push + deploy (App Service / VPS) — cấu hình secret ở repo
```

### 8.2 Hạ tầng

- **Container**: `Dockerfile` multi-stage (`sdk` → `aspnet`), chạy non-root, `HEALTHCHECK`.
- **Compose local**: web + SQL Server 2022 + MailHog (test email).
- **Môi trường**: `Development` / `Staging` / `Production`; cấu hình qua env, **không** commit chuỗi kết nối.
- **Bí mật**: `dotnet user-secrets` khi dev; Key Vault/App Settings khi deploy.
- **Data Protection**: persist key ring (Redis/Blob) để không logout khi restart và scale-out.

### 8.3 Quan sát

| Thành phần | Công cụ | Ngưỡng cảnh báo |
|---|---|---|
| Log | Serilog → file + sink (Seq/App Insights) | lỗi 5xx > 5/phút |
| Health | `/health` (DB, storage) | 2 lần fail liên tiếp |
| Metric | request rate, p95, tỉ lệ lỗi, queue email | p95 > 500 ms trong 5 phút |
| Audit | bảng `AuditLog` (ai, làm gì, khi nào) | thao tác xoá/đổi quyền |

### 8.4 Ngưỡng hiệu năng & chất lượng (định nghĩa "đạt")

| Chỉ số | Hiện tại (ước tính) | Mục tiêu |
|---|---|---|
| Dung lượng ảnh | 109 MB | < 15 MB (WebP, 3 cỡ) |
| Tổng dung lượng trang chủ | ~30–60 MB tiềm tàng | < 1,5 MB |
| LCP (mobile 4G) | > 4 s | < 2,5 s |
| p95 trang danh sách | ~1 s+ (nhiều ảnh, không cache) | < 300 ms |
| Số query trang chủ | 5 + 2 vòng lặp dashboard | ≤ 5, không N+1 |
| Lighthouse performance | chưa đo | ≥ 90 |
| Coverage | 0% | ≥ 60% |
| Cảnh báo build | 75 | 0 (`-warnaserror`) |

---

## 9. Định nghĩa "hoàn thành" — checklist nghiệm thu

Đây là danh sách dùng để tự chấm và để trả lời "dự án của bạn đã production-ready chưa?".

**Bảo mật & dữ liệu**
- [ ] Không còn mật khẩu plaintext; có policy độ mạnh; khoá sau 5 lần sai.
- [ ] 100% action POST có CSRF; 100% area Admin có `[Authorize]`; có test ma trận quyền.
- [ ] Không có IDOR: mọi truy vấn đơn/sản phẩm đều lọc theo chủ sở hữu hoặc role.
- [ ] Security headers (HSTS, CSP, X-Content-Type-Options, Referrer-Policy), HTTPS bắt buộc.
- [ ] Rate limit cho login/đăng ký/quên mật khẩu/checkout.
- [ ] Tiền là `decimal`; tồn kho có `RowVersion`; không oversell trong test tải.
- [ ] Bí mật không nằm trong repo; `.env.example` đầy đủ.

**Chức năng**
- [ ] Đơn hàng đi đủ 6 trạng thái, có audit, huỷ/hoàn kho đúng.
- [ ] Thanh toán sandbox chạy end-to-end, callback idempotent, có đối soát.
- [ ] Email xác nhận + đổi trạng thái + quên mật khẩu hoạt động (MailHog khi dev).
- [ ] Admin: đổi trạng thái đơn, duyệt đánh giá, quản lý tồn kho có lý do.
- [ ] Khách: sổ địa chỉ, theo dõi đơn, huỷ đơn, đánh giá chỉ khi đã mua.

**Chất lượng & vận hành**
- [ ] `git clone` → 3 lệnh → chạy (README có ảnh/Video).
- [ ] `dotnet test` xanh, coverage ≥ 60%, CI xanh trên PR.
- [ ] `docker compose up` chạy web + DB + mail.
- [ ] Health check, log có correlation id, cảnh báo 5xx.
- [ ] Backup/restore đã thử ít nhất 1 lần.
- [ ] k6 + ZAP + Lighthouse đạt ngưỡng, có báo cáo lưu trong `docs/`.
- [ ] Staging và production tách biệt; deploy bằng pipeline, không thao tác tay.

**Trình diễn**
- [ ] Video 3–5 phút: khách mua → thanh toán → admin xử lý → email.
- [ ] Sơ đồ kiến trúc + 6 screenshot + bộ số liệu before/after.
- [ ] README mở ra là hiểu dự án trong 60 giây.

---

## 10. Bảy ngày đầu — lịch hành động

| Ngày | Việc | Kết quả |
|---|---|---|
| 1 | 0.1 + 0.2 + 0.3 (role, authorize, hash) | không còn leo thang quyền, không còn plaintext |
| 2 | 0.4 + 0.5 (CSRF, IDOR, authorize order) | ẩn danh không chạm được dữ liệu |
| 3 | 0.6 + 0.7 + 0.8 (trạng thái, SalePrice, địa chỉ) | đơn hàng đúng nghiệp vụ |
| 4 | 0.9 (migrations, README, seed, user-secrets) | clone là chạy |
| 5 | 0.10 + 0.11 (trang lỗi, ảnh, dọn template) | không lộ stack trace, web nhẹ |
| 6 | 0.12 (6 test đầu) + chạy thử toàn luồng | có bằng chứng chất lượng |
| 7 | Quay video demo + viết README có sơ đồ | có tài sản để đưa vào CV |

---

## 11. Phương án cắt giảm (nếu thiếu thời gian)

**Giữ (không thể bỏ):** hash mật khẩu, authorize admin, CSRF, sửa role, trạng thái đơn, SalePrice, README + migrations, 6 test.
**Hoãn được:** thanh toán thật (dùng "COD + chuyển khoản" và mô tả rõ), coupon, wishlist, biến thể sản phẩm, đa ngôn ngữ, Redis.
**Cắt hẳn:** so sánh sản phẩm, chat trực tuyến, đa chi nhánh, mobile app.
**Tuyệt đối không:** deploy production với mật khẩu plaintext hoặc admin không được bảo vệ — rủi ro pháp lý và uy tín lớn hơn nhiều so với việc thiếu tính năng.

---

## 12. Rủi ro & câu hỏi cần quyết

| Rủi ro | Ảnh hưởng | Giảm thiểu |
|---|---|---|
| Đổi sang Identity làm vỡ luồng login cũ | cao | làm ở GĐ 1, giữ claim tương thích, test migration dữ liệu |
| Migrate `double → decimal` sai dữ liệu | cao | script chuyển đổi + đối chiếu tổng tiền trước/sau |
| Thanh toán sandbox khác production | trung bình | tách `IPaymentGateway`, test cả callback thất bại/trùng |
| Thời gian 8 tuần không đủ | cao | bám DoD từng giai đoạn, cắt theo mục 11 |
| Ảnh/asset bản quyền template | trung bình | thay bằng ảnh tự chụp/CC0 trước khi public |

**Cần bạn quyết:**
1. Mục tiêu là **đồ án để chấm** hay **sản phẩm để tuyển dụng**? (ảnh hưởng thứ tự ưu tiên: chấm điểm cần đủ chức năng + báo cáo; tuyển dụng cần bảo mật + test + deploy).
2. Có được dùng **ASP.NET Identity** không, hay phải giữ bảng `Users` tự viết?
3. Có tài khoản **VNPay/MoMo sandbox** chưa? Nếu chưa, tạm dùng COD + mô tả thiết kế thanh toán.
4. Có **domain/hosting** để demo công khai không? (Azure for Students / VPS ~100k/tháng).
5. Thời gian thực tế mỗi tuần bao nhiêu giờ? (lộ trình đang giả định 20 h/tuần).

---

## Phụ lục A — Mẫu sửa nhanh (dán được)

### A.1 Bảo vệ cả area Admin bằng một chỗ

```csharp
// Program.cs
builder.Services.AddControllersWithViews(options =>
{
    // CSRF toàn cục: mọi POST/PUT/DELETE đều phải có token
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
})
.AddRazorPages()
.AddMvcOptions(o => o.Filters.Add(new AuthorizeFilter(
    new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())));
```

```csharp
// Areas/Admin/Controllers/AdminBaseController.cs
[Area("Admin")]
[Authorize(Roles = "Admin")]
public abstract class AdminBaseController : Controller { }
// rồi cho 4 controller admin kế thừa AdminBaseController
```

### A.2 Hash mật khẩu (bước đệm trước khi lên Identity)

```csharp
public interface IPasswordService { string Hash(string raw); bool Verify(string raw, string hash); }

public sealed class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new();
    public string Hash(string raw) => _hasher.HashPassword(new object(), raw);
    public bool Verify(string raw, string hash) =>
        _hasher.VerifyHashedPassword(new object(), hash, raw) != PasswordVerificationResult.Failed;
}
```

```csharp
// Login: KHÔNG so sánh trong LINQ nữa
var user = await _context.Users.Include(u => u.Role)
                              .FirstOrDefaultAsync(u => u.Username == model.Username);
if (user is null || !_password.Verify(model.Password, user.Password))
{
    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
    return View(model);
}
```

### A.3 Trạng thái đơn + giá bán đúng

```csharp
public enum OrderStatus { Pending = 0, Confirmed = 1, Shipping = 2, Delivered = 3, Completed = 4, Cancelled = 5, Returned = 6 }

// trong transaction checkout
var order = new Order {
    UserId = userId,
    OrderDate = DateTime.UtcNow,
    Status = OrderStatus.Pending.ToString(),   // KHÔNG phải "Đã giao hàng"
    TotalPrice = cart.CartItems.Sum(i => i.Quantity * i.Product.Price),
    ShippingAddress = model.Address!,
    ReceiverName = model.FullName!,
    ReceiverPhone = model.Phone!
};
foreach (var item in cart.CartItems)
{
    order.OrderItems.Add(new OrderItem {
        ProductId = item.ProductId,
        Quantity  = item.Quantity,
        SalePrice = item.Product.Price          // BẮT BUỘC
    });
    item.Product.StockQuantity -= item.Quantity;
}
```

### A.4 Chống oversell

```csharp
public class Product {
    // ...
    [Timestamp] public byte[]? RowVersion { get; set; }   // SQL rowversion
}
// Checkout: bắt DbUpdateConcurrencyException → rollback + thông báo "sản phẩm vừa hết hàng"
```

### A.5 Tiền là decimal

```csharp
public decimal Price { get; set; }                 // Product
public decimal? TotalPrice { get; set; }           // Order
public decimal SalePrice { get; set; }             // OrderItem
// Fluent API
entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
```

### A.6 Security headers + HTTPS

```csharp
app.UseHsts();
app.UseHttpsRedirection();
app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
    ctx.Response.Headers["Referrer-Policy"]      = "strict-origin-when-cross-origin";
    ctx.Response.Headers["X-Frame-Options"]      = "DENY";
    ctx.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; img-src 'self' data:; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; font-src https://fonts.gstatic.com";
    await next();
});
```

### A.7 Cookie an toàn + Data Protection bền

```csharp
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/var/keys"))
    .SetApplicationName("HTNLShop");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o => {
        o.Cookie.Name = "__Host-htnl";
        o.Cookie.HttpOnly = true;
        o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        o.Cookie.SameSite = SameSiteMode.Lax;
        o.LoginPath = "/Customer/Login";
        o.AccessDeniedPath = "/Customer/AccessDenied";
    });
```

> Ghi chú: `__Host-` yêu cầu HTTPS + `Path=/`; khi dev HTTP hãy dùng tên khác.

---

## Phụ lục B — Bộ số liệu cần đo để đưa vào CV

Đo **trước** khi sửa, ghi vào bảng, đo **lại** sau. Không có số thì đừng ghi số.

| Chỉ số | Cách đo | Trước | Sau |
|---|---|---|---|
| Số lỗ hổng Critical/High | checklist + ZAP baseline | 5 Critical | 0 |
| Coverage | `dotnet test --collect` | 0% | ? |
| Số test | đếm test case | 0 | ? |
| Dung lượng ảnh | `du -sh wwwroot` | 109 MB | ? |
| p95 trang danh sách | k6 (200 VU) | ? | ? |
| LCP mobile | Lighthouse | ? | ? |
| Thời gian deploy | bấm pipeline → live | thủ công | ? |
| Số query trang chủ | EF logging | ? | ? |

---

*Tài liệu liên quan: [`CV_VA_DEMO.md`](CV_VA_DEMO.md) — mô tả dự án, bullet CV tiếng Việt/Anh, kịch bản demo, câu hỏi phỏng vấn. [`backlog.csv`](backlog.csv) — backlog dạng bảng để import vào Jira/GitHub Issues.*
