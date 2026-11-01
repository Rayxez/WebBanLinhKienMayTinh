# HTNL Shop — Gói CV, demo và phỏng vấn

> Dùng cùng với [`BAO_CAO_DANH_GIA_VA_LO_TRINH.md`](BAO_CAO_DANH_GIA_VA_LO_TRINH.md).
> Nguyên tắc: **chỉ ghi những gì đã làm và đo được**. Các ô `[…]` là chỗ điền số thật sau khi hoàn thành giai đoạn tương ứng.

---

## 1. Mô tả dự án — bản ngắn (để đầu CV)

### Tiếng Việt

> **HTNL Shop** — Website thương mại điện tử bán lẻ máy tính & linh kiện (ASP.NET Core 8 MVC, EF Core, SQL Server).
> Xây dựng từ đầu: catalogue 480 sản phẩm / 16 danh mục, giỏ hàng cho khách vãng lai (session) tự động gộp vào giỏ lưu DB khi đăng nhập, đặt hàng có transaction và kiểm soát tồn kho, đánh giá sản phẩm, khu quản trị có dashboard doanh thu và CRUD sản phẩm/người dùng/đơn hàng.
> Vai trò: **một mình** — phân tích nghiệp vụ, thiết kế CSDL, backend, giao diện, kiểm thử, triển khai.

### English

> **HTNL Shop** — E-commerce storefront for PC hardware and accessories (ASP.NET Core 8 MVC, EF Core, SQL Server).
> Built end-to-end: 480-product catalog across 16 categories, guest cart (session) that merges into a per-user database cart on sign-in, transactional checkout with stock validation, product reviews, and an admin area with revenue dashboard plus product/user/order management.
> **Solo developer** — requirements, database design, backend, UI, testing, deployment.

---

## 2. Tech stack — từ khoá để ATS quét

`C#` · `.NET 8` · `ASP.NET Core MVC` · `Razor` · `Entity Framework Core 9` · `LINQ` · `SQL Server` · `T-SQL` · `Migrations` · `Cookie Authentication` · `Authorization / RBAC` · `Dependency Injection` · `AutoMapper` · `X.PagedList` · `Bootstrap 5` · `jQuery` · `Git` · `xUnit` · `Docker` · `GitHub Actions` · `REST` · `OWASP Top 10` · `Serilog` · `k6` · `Playwright`

> Chỉ để những từ **thực sự đã dùng**. Nếu chưa làm Docker/CI thì đừng đưa vào; làm xong GĐ 1 rồi thêm.

---

## 3. Bullet thành tích — công thức XYZ (Accomplished X, measured by Y, by doing Z)

### 3.1 Bản tiếng Việt (dùng 4–6 bullet)

- **Vá 5 lỗ hổng bảo mật mức Critical** (leo thang đặc quyền qua đăng ký, thiếu phân quyền toàn bộ area Admin, mật khẩu plaintext, IDOR đơn hàng, thiếu CSRF) trước khi phát hành; thêm **15 test phân quyền** (5 URL × 3 vai trò) trong tổng **44 test**.
- **Chuẩn hoá vòng đời đơn hàng** 6 trạng thái có luật chuyển; sửa lỗi đơn mới bị gán "Đã giao hàng", giá bán 0đ, địa chỉ người nhận sai; **chống oversell** bằng transaction + `rowversion`, huỷ đơn tự hoàn kho.
- **Chuẩn hoá tiền tệ**: chuyển toàn bộ cột tiền từ `double` sang `decimal(18,2)` kèm migration dữ liệu, loại bỏ sai số làm tròn khi tính tổng đơn.
- **Tăng độ phủ kiểm thử từ 0% lên 60,8% dòng** (44 test: luật trạng thái, integration `WebApplicationFactory` + SQL Server, checkout end-to-end, CSRF, sitemap) — đã loại mã sinh tự động.
- **Giảm 79% dung lượng ảnh** (108,8 MB → 23,1 MB, 485 ảnh), cache tĩnh 7 ngày, `sitemap.xml` động 480 URL, title/meta/Open Graph theo từng sản phẩm.
- **Tự động hoá build & test**: GitHub Actions (build → 44 test + coverage → quét gói lỗ hổng → Docker build), Dockerfile multi-stage chạy non-root, `docker-compose` web + SQL Server, health check `/health`.

> Nếu chưa làm được hết, giữ 3 bullet mạnh nhất và bỏ phần chưa có số. **Không** viết "tối ưu 90%" khi chưa đo.

### 3.2 English version

- **Fixed 5 critical security flaws** (privilege escalation on sign-up, unprotected admin area, plaintext passwords, order IDOR, missing CSRF) before release; added **15 authorization tests** across a 5-URL × 3-role matrix, part of a **44-test** suite.
- **Modelled a 6-state order lifecycle** with explicit transition rules; fixed orders created as "Delivered", zero-priced items and the wrong receiver address; **oversell protection** via transaction + `rowversion`, stock restored on cancellation.
- **Normalised money types**: migrated all monetary columns from `double` to `decimal(18,2)` with a data migration, removing rounding errors in order totals.
- **Raised line coverage from 0% to 60.8%** on hand-written code (44 tests: transition rules, `WebApplicationFactory` + SQL Server integration, end-to-end checkout, CSRF, sitemap).
- **Cut image payload by 79%** (108.8 MB → 23.1 MB, 485 images), 7-day static caching, dynamic `sitemap.xml` with 480 URLs, per-product title/meta/Open Graph.
- **Automated build & tests**: GitHub Actions (build → 44 tests + coverage → vulnerability scan → Docker build), a multi-stage non-root Docker image, a `docker-compose` stack with SQL Server and a `/health` endpoint.

### 3.3 Bản siêu gọn (3 dòng, cho CV 1 trang)

```
HTNL Shop — E-commerce PC parts (ASP.NET Core 8, EF Core, SQL Server) · Solo
• Hardened auth & admin: 5 critical flaws fixed, 15 authorization tests (44 total)
• Order lifecycle 6 states, decimal money, oversell protection (rowversion)
• GitHub Actions + Docker, coverage 0 → 60.8%, images 108.8 → 23.1 MB (−79%)
```

---

## 4. Nên và không nên ghi

| Nên | Không nên |
|---|---|
| "480 sản phẩm, 16 danh mục" (quy mô dữ liệu thật của dự án) | "Hệ thống phục vụ 10.000 người dùng" khi chưa từng chạy |
| "44 test, coverage 60,8%, ảnh giảm 79%" | "Áp dụng DDD/CQRS/Microservices" khi thực tế là MVC 1 project |
| "Thanh toán VNPay **sandbox**" | "Tích hợp cổng thanh toán" gây hiểu nhầm là production |
| "Tối ưu ảnh 109 MB → 12 MB (đo bằng `du`)" | "Tối ưu hiệu năng vượt trội" không số |
| "Một mình làm toàn bộ" | "Team 5 người" nếu bạn làm một mình |
| Nêu **lỗi đã sửa** — thể hiện khả năng tự review | Giấu lỗi rồi bị hỏi vặn tại phỏng vấn |

---

## 5. Kịch bản demo 5 phút (quay video hoặc live)

Chuẩn bị: DB đã seed, tài khoản `customer@demo` và `admin@demo`, trình duyệt profile sạch, tab thứ hai ở chế độ ẩn danh.

| Thời lượng | Thao tác | Điều muốn người xem thấy |
|---|---|---|
| 0:00–0:30 | Trang chủ → danh mục → tìm "RTX 4060" → lọc/sắp xếp | catalogue, tìm kiếm, UI |
| 0:30–1:15 | Thêm 2 sản phẩm vào giỏ **khi chưa đăng nhập** → đăng nhập → giỏ được gộp | điểm kỹ thuật đáng giá nhất của dự án |
| 1:15–2:15 | Checkout: nhập **người nhận khác**, ghi chú → đặt hàng → trang chi tiết đơn | luồng đặt hàng + lưu đúng giá/người nhận |
| 2:15–2:45 | Mở "Đơn của tôi" của `customer` và của `tester` → mỗi người chỉ thấy đơn của mình | chống IDOR bằng dữ liệu thật |
| 2:45–3:30 | Đăng nhập admin → đổi trạng thái đơn (xác nhận → giao) → huỷ một đơn khác và xem tồn kho được hoàn | vòng đời đơn + toàn vẹn tồn kho |
| 3:30–4:15 | Mở tab ẩn danh: vào `/Admin/QuanLySanPham` → bị chặn; thử POST thiếu CSRF → 400 | **bảo mật là điểm bán hàng** |
| 4:15–5:00 | Màn hình CI xanh + coverage + k6/ZAP/Lighthouse report + sơ đồ kiến trúc | bằng chứng kỹ thuật, không chỉ lời nói |

> Mẹo: quay 1 lần liền mạch, không cắt cảnh trong luồng thanh toán — người xem cần thấy nó thật.

---

## 6. Bộ tài sản cần có trước khi nộp CV

- [ ] Repo public, README có: ảnh chụp, sơ đồ kiến trúc, 3 lệnh chạy, mô tả tính năng, phần "Security" và "Testing".
- [ ] `docs/` chứa: báo cáo này, ADR (3–5 quyết định), ảnh sơ đồ, kết quả k6/ZAP/Lighthouse.
- [ ] Video demo 3–5 phút (YouTube unlisted hoặc Loom), có phụ đề tiếng Việt/Anh.
- [ ] 6 screenshot chất lượng (trang chủ, chi tiết, giỏ, checkout, dashboard, timeline đơn).
- [ ] Pipeline xanh + badge coverage trong README.
- [ ] URL demo (nếu có) + tài khoản demo chỉ-đọc, ghi rõ trong README.
- [ ] File `docs/CV.pdf` bản 1 trang, và bản 2 trang có phần dự án này.

---

## 7. Câu hỏi phỏng vấn sẽ gặp — và cách trả lời dựa trên chính dự án

**1. "Vì sao em không dùng ASP.NET Identity ngay từ đầu?"**
> "Ban đầu em tự viết bảng `Users` để hiểu cơ chế cookie auth và claim. Khi rà bảo mật em phát hiện mật khẩu lưu plaintext và role gán sai, nên đã migrate sang Identity ở giai đoạn 1. Bài học: không tự viết phần xác thực nếu không có lý do đặc biệt."

**2. "Làm sao chống bán vượt tồn kho?"**
> "Hai lớp: kiểm tra và trừ kho trong cùng transaction; thêm cột `rowversion` trên `Product` để EF đưa vào WHERE khi UPDATE, nên nếu người khác vừa sửa tồn kho thì `SaveChanges` ném `DbUpdateConcurrencyException` và đơn được rollback. Em có test end-to-end xác nhận tồn kho giảm đúng và test huỷ đơn hoàn kho."

**3. "Vì sao dùng `decimal` thay vì `double` cho tiền?"**
> "`double` là số nhị phân nên không biểu diễn chính xác 0.1; cộng nhiều dòng hàng sẽ lệch vài đồng và không đối soát được với cổng thanh toán. Em đã chuyển toàn bộ cột tiền sang `decimal(18,2)` bằng migration và thêm test kiểm tra thành tiền."

**4. "Webhook thanh toán bị gọi 2 lần thì sao?"**
> "Bảng `PaymentTransaction` có unique index trên `ExternalId`; xử lý trong transaction, nếu đã tồn tại thì trả 200 luôn (idempotent), không cộng tiền lần hai. Em cũng verify chữ ký trước khi ghi."

**5. "Em test phân quyền thế nào?"**
> "Viết `[Theory]` với ma trận 5 URL quản trị × 3 vai trò (ẩn danh, Customer, Admin), dùng `WebApplicationFactory` và seed sẵn 3 client. Test này bắt đúng lỗi admin area không có `[Authorize]`: trước khi sửa thì đỏ, sau khi sửa thì xanh và chạy trong CI."

**6. "Số liệu hiệu năng em đo bằng gì?"**
> "Với dung lượng, em đo trực tiếp trên thư mục `wwwroot`: 121,2 MB → 35,5 MB sau khi nén 485 ảnh (108,8 → 23,1 MB). Với tốc độ, em dự kiến dùng k6 cho p95, Lighthouse cho LCP và EF logging để đếm query — đo baseline trước rồi so sánh sau từng thay đổi."

**7. "Nếu làm lại từ đầu, em đổi gì?"**
> "Ba thứ: dùng Identity ngay, tách lớp từ đầu thay vì nhồi vào controller, và thiết kế schema tiền/tồn kho đúng ngay từ migration đầu tiên. Ngoài ra em sẽ viết test luồng đặt hàng trước khi làm UI."

**8. "Rủi ro lớn nhất khi đưa lên production?"**
> "Đối soát thanh toán và dữ liệu cũ. Nên em ưu tiên: idempotency, job đối soát hằng ngày, backup có thử restore, và cảnh báo khi lệch trạng thái đơn với giao dịch."

---

## 8. Hai câu chuyện STAR (chuẩn bị sẵn, nói 90 giây mỗi câu)

### 8.1 Bảo mật

- **Situation:** chuẩn bị đưa đồ án đi demo, tự rà theo OWASP.
- **Task:** đảm bảo không ai có thể leo thang quyền hoặc sửa dữ liệu khi chưa đăng nhập.
- **Action:** lập checklist 12 điểm; phát hiện 5 lỗi Critical; sửa theo thứ tự ưu tiên; viết 15 test phân quyền để chống tái phát; thêm CSRF toàn cục và security headers.
- **Result:** ẩn danh bị chặn khỏi toàn bộ khu quản trị, Customer nhận 403, POST thiếu CSRF trả 400; test phân quyền chạy trong CI.

### 8.2 Hiệu năng

- **Situation:** trang danh sách tải chậm, ảnh nặng 109 MB.
- **Task:** đưa LCP < 2,5 s trên mobile 4G.
- **Action:** đo baseline; nén/WebP + `srcset`; `AsNoTracking` + projection; index cho cột tìm kiếm; cache 60 s cho danh mục; bỏ 2 vòng lặp 24 query ở dashboard.
- **Result:** [..] MB → [..] MB, LCP [..] → [..] s, p95 [..] → [..] ms; ghi lại before/after trong repo.

---

## 9. Bảng "trước / sau" để đặt cuối CV hoặc slide

| Hạng mục | Trước | Sau |
|---|---|---|
| Lỗ hổng Critical | 5 | 0 |
| Test | 0 | 44 |
| Coverage (mã tự viết) | 0% | 60,8% dòng |
| Kiểu dữ liệu tiền | `double` | `decimal(18,2)` |
| Dung lượng ảnh sản phẩm | 108,8 MB | 23,1 MB (−79%) |
| Dung lượng `wwwroot` | 121,2 MB | 35,5 MB |
| Trạng thái đơn | 1 bước | 6 bước + luật chuyển |
| Chống oversell | không | rowversion + transaction |
| Thời gian deploy | thủ công | 1 pipeline (CI + Docker) |

---

## 10. Ghi chú trung thực (đọc trước khi viết CV)

1. **Đừng gọi là "sản phẩm thương mại"** khi chưa có thanh toán thật, email thật, pháp lý, và người dùng thật. Gọi đúng: *"đồ án thương mại điện tử hoàn chỉnh ở mức MVP+, có kiểm thử và triển khai"*.
2. **Số liệu phải tự đo được** — người phỏng vấn senior sẽ hỏi "đo bằng công cụ gì, trên tải bao nhiêu".
3. **Nêu lỗi đã sửa là điểm cộng**, miễn là bạn giải thích được nguyên nhân gốc và cách phòng ngừa.
4. **Không commit `bin/`, `obj/`, chuỗi kết nối, mật khẩu** — nhà tuyển dụng có thể xem repo.
5. Nếu dự án dùng template/ảnh của bên thứ ba, ghi rõ nguồn và giấy phép trong README.
