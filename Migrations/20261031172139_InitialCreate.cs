using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HTNLShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__19093A0BA9FE0B52", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Carts__51BCD7B7AB710385", x => x.CartId);
                    table.ForeignKey(
                        name: "FK__Carts__UserId__29572725",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    ShippingAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceiverPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__C3905BCFBA652293", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK__Orders__UserId__38996AB5",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductDetail = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__B40CC6CD32BFF0F6", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK__Products__Catego__2F10007B",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CartItemsId", x => new { x.CartId, x.ProductId });
                    table.ForeignKey(
                        name: "FK__CartItems__CartI__32E0915F",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId");
                    table.ForeignKey(
                        name: "FK__CartItems__Produ__33D4B598",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SalePrice = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("OrderItemsId", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK__OrderItem__Order__3B75D760",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK__OrderItem__Produ__3C69FB99",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "tb_Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_tb_Review_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_Review_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Laptop" },
                    { 2, "PC Gaming" },
                    { 3, "Màn hình" },
                    { 4, "Loa" },
                    { 5, "Chuột" },
                    { 6, "Tai nghe" },
                    { 7, "Bàn phím" },
                    { 8, "Nguồn" },
                    { 9, "Main" },
                    { 10, "Ghế" },
                    { 11, "CPU" },
                    { 12, "VGA" },
                    { 13, "RAM" },
                    { 14, "Ổ cứng" },
                    { 15, "Tản nhiệt" },
                    { 16, "Phụ kiện khác" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "ImageUrl", "Price", "ProductDetail", "ProductName", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, "/Assets/img/Products/1/1.jpg", 18390000.0, "Laptop gaming mạnh mẽ, CPU Intel Gen 13, card RTX 4050, tản nhiệt kép.", "Laptop Acer Nitro V ANV15 51 500A", 10 },
                    { 2, 1, "/Assets/img/Products/1/2.jpg", 11990000.0, "Thiết kế mỏng nhẹ, vỏ nhôm cao cấp, hiệu năng mạnh mẽ với Intel Core i5 Gen 11, pin bền, phù hợp cho sinh viên và dân văn phòng.", "Laptop Acer Swift 3 SF314 511 55QE", 8 },
                    { 3, 1, "/Assets/img/Products/1/3.jpg", 14990000.0, "Laptop giá tốt, hiệu năng ổn định, màn hình 14 inch Full HD, thiết kế thanh lịch – lựa chọn lý tưởng cho nhu cầu học tập và làm việc cơ bản.", "Laptop Acer Aspire Lite 14 AL14 71P 55P9", 6 },
                    { 4, 1, "/Assets/img/Products/1/4.jpg", 33990000.0, "Hiệu năng cao với chip Intel Gen 12 và GPU RTX, đáp ứng tốt nhu cầu đồ họa, lập trình và sáng tạo nội dung trong thiết kế gọn nhẹ.", "Laptop Acer Swift X14 SFX14 72G 77F9", 5 },
                    { 5, 1, "/Assets/img/Products/1/5.jpg", 18390000.0, "Mỏng nhẹ chỉ khoảng 1,2kg, pin lâu, cấu hình mạnh mẽ, phù hợp người dùng thường xuyên di chuyển và làm việc ngoài văn phòng.", "Laptop ACER Swift Lite 14 SFL14 51M 56HS", 10 },
                    { 6, 1, "/Assets/img/Products/1/6.jpg", 28990000.0, "Trang bị CPU Intel Gen 14 mới nhất, màn OLED sắc nét, hiệu năng mạnh mẽ và thiết kế sang trọng – tối ưu cho công việc lẫn giải trí.", "Laptop Acer Swift Go SFG14 74 58FJ", 8 },
                    { 7, 1, "/Assets/img/Products/1/7.jpg", 18290000.0, "Thiết kế mỏng nhẹ, hiệu năng cao, phù hợp cho học tập và làm việc.", "Laptop ASUS Vivobook K3605VC RP431W", 8 },
                    { 8, 1, "/Assets/img/Products/1/8.jpg", 20490000.0, "Thiết kế mỏng nhẹ, màn hình OLED 14 inch, CPU Intel Core Ultra 5, pin bền và hiệu năng ổn định cho học tập và làm việc.", "Laptop ASUS Vivobook S14 S3407CA LY095WS", 6 },
                    { 9, 1, "/Assets/img/Products/1/9.jpg", 22990000.0, "Trang bị chip Intel Core Ultra 7, màn hình OLED sắc nét, thiết kế hiện đại, hiệu năng mạnh cho người dùng văn phòng và sáng tạo nội dung.", "Laptop ASUS Vivobook S14 S3407CA LY096WS", 9 },
                    { 10, 1, "/Assets/img/Products/1/10.jpg", 34990000.0, "Ultrabook cao cấp với màn OLED 2.8K, chip Intel Core Ultra 7, siêu mỏng nhẹ, pin lâu – lựa chọn hoàn hảo cho doanh nhân di chuyển nhiều.", "Laptop ASUS Zenbook 14 UX3405CA PZ204WS", 7 },
                    { 11, 1, "/Assets/img/Products/1/11.jpg", 12190000.0, "Laptop doanh nhân bền bỉ, CPU AMD Ryzen 5, RAM 16GB, hiệu năng ổn định, phù hợp làm việc văn phòng và học tập chuyên nghiệp.", "Laptop ASUS ExpertBook B1 BM1403CDA S60974W", 5 },
                    { 12, 1, "/Assets/img/Products/1/12.jpg", 17990000.0, "Hiệu năng mạnh mẽ với Intel Core i7 Gen 13, thiết kế chắc chắn, bảo mật cao – tối ưu cho môi trường doanh nghiệp.", "Laptop ASUS Expertbook P1403CVA-i716-50W", 4 },
                    { 13, 1, "/Assets/img/Products/1/13.jpg", 20990000.0, "Hiệu năng mạnh với Intel Core i7 Gen 13, RAM 16GB, SSD 512GB, thiết kế hiện đại – phù hợp làm việc và giải trí.", "Laptop Dell 15 DC15250 i7U161W11SLU", 12 },
                    { 14, 1, "/Assets/img/Products/1/14.jpg", 16990000.0, "Laptop mỏng nhẹ, CPU Intel Core i5 Gen 13, RAM 16GB, SSD 512GB, màn 14 FHD – lý tưởng cho sinh viên và dân văn phòng.", "Laptop Dell Inspiron 5440-PUS i5-1334U/512GB/16GB DDR5/14/FHD/W11/Carbon Black", 10 },
                    { 15, 1, "/Assets/img/Products/1/15.jpg", 15990000.0, "Thiết kế tinh tế, chip i5 Gen 13, hiệu năng ổn định, pin tốt – đáp ứng nhu cầu học tập và làm việc hằng ngày.", "Laptop Dell Inspirion N3530 i5U165W11SLU (1334U)", 8 },
                    { 16, 1, "/Assets/img/Products/1/16.jpg", 54990000.0, "Ultrabook cao cấp, vỏ nhôm nguyên khối, màn hình sắc nét, chip i7, RAM 16GB – dành cho người dùng chuyên nghiệp và sáng tạo.", "Laptop Dell XPS 9350 XPS9350-U5IA165W11GR-FP", 7 },
                    { 17, 1, "/Assets/img/Products/1/17.jpg", 29490000.0, "Laptop gaming hiệu năng cao, CPU Intel Core i7 Gen 13, GPU RTX 4050, màn 16.1 – chơi game và làm đồ họa mượt mà.", "Laptop gaming HP Victus 16-r0369TX AY8Y2PA", 13 },
                    { 18, 1, "/Assets/img/Products/1/18.jpg", 19990000.0, "Trang bị Ryzen 7 7840HS và RTX 4060, hiệu năng mạnh, tản nhiệt tốt – phù hợp game thủ và nhà sáng tạo nội dung.", "Laptop gaming HP Victus 16-s1149AX AZ0D4PA", 11 },
                    { 19, 1, "/Assets/img/Products/1/19.jpg", 20990000.0, "Thiết kế trẻ trung, chip Ryzen 5 7535HS, GPU RTX 2050, hiệu năng ổn định, chơi game và học tập tốt.", "Laptop gaming HP VICTUS 15 fb3116AX BX8U4PA", 9 },
                    { 20, 1, "/Assets/img/Products/1/20.jpg", 31990000.0, "Dòng cao cấp, CPU i7 Gen 13, GPU RTX 4070, màn QHD 165Hz, bàn phím RGB – trải nghiệm gaming đỉnh cao.", "Laptop gaming HP OMEN 16-am0180TX BX8Y6PA", 8 },
                    { 21, 1, "/Assets/img/Products/1/21.jpg", 37490000.0, "Hiệu năng mạnh với Intel Core i7 Gen 13, GPU RTX 4060, tản nhiệt hiệu quả – chiến mượt mọi tựa game nặng.", "Laptop gaming Lenovo Legion 5 15IRX10 83LY00A7VN", 14 },
                    { 22, 1, "/Assets/img/Products/1/22.jpg", 22290000.0, "Trang bị Ryzen 7 7840HS, RTX 4050, thiết kế gaming hiện đại, bàn phím RGB – hiệu suất cao cho game và đồ họa.", "Laptop gaming Lenovo LOQ 15ARP9 83JC00M3VN", 12 },
                    { 23, 1, "/Assets/img/Products/1/23.jpg", 36990000.0, "Sức mạnh từ Ryzen 7 8845HS và RTX 4070, màn 165Hz mượt mà – tối ưu cho game thủ chuyên nghiệp.", "Laptop gaming Lenovo Legion 5 15AHP10 83M0002YVN", 10 },
                    { 24, 1, "/Assets/img/Products/1/24.jpg", 30790000.0, "Laptop gaming tầm trung, CPU Ryzen 5 8645HS, GPU RTX 3050, hiệu năng ổn định – cân tốt các game phổ biến.", "Laptop gaming Lenovo LOQ 15AHP10 83JG0047VN", 9 },
                    { 25, 1, "/Assets/img/Products/1/25.jpg", 22490000.0, "Ultrabook mỏng nhẹ, CPU Intel Core i7 Gen 13, RAM 16GB, pin lâu, tối ưu cho sáng tạo nội dung và văn phòng.", "Laptop MSI Prestige 14 Evo B13M 401VN", 7 },
                    { 26, 1, "/Assets/img/Products/1/26.jpg", 18290000.0, "Laptop mỏng nhẹ, chip Intel Core i5, RAM 8GB, SSD 512GB – hiệu năng ổn định cho học tập và làm việc cơ bản.", "Laptop MSI Prestige 14 AI+ Evo C2VMG 020VN", 8 },
                    { 27, 1, "/Assets/img/Products/1/27.jpg", 25490000.0, "Ultrabook cao cấp, CPU Intel Core i7 Gen 13, GPU RTX 4060, màn 16 sắc nét – dành cho thiết kế đồ họa và multimedia.", "Laptop MSI Modern 14 F13MG 466VN", 7 },
                    { 28, 1, "/Assets/img/Products/1/28.jpg", 38990000.0, "Hiệu năng đỉnh cao với Core i9 và RTX 4080, chuyên game và đồ họa.", "Laptop MSI Prestige 16 AI+ Mercedes AMG B2VMG 088VN", 3 },
                    { 29, 1, "/Assets/img/Products/1/29.jpg", 17390000.0, "Laptop gaming mạnh mẽ, CPU Intel Gen 13, card RTX 4050, tản nhiệt kép.", "Laptop MSI Modern 15 H C13M 216VN", 10 },
                    { 30, 2, "/Assets/img/Products/2/30.jpg", 10490000.0, "Cấu hình khủng với i3-12100F VGA RX 6500XT, chơi mọi tựa game ở 4K.", "PC GVN Intel i3-12100F/ VGA RX 6500XT (Powered by ASUS)", 3 },
                    { 31, 2, "/Assets/img/Products/2/31.jpg", 12390000.0, "Máy tính chơi game tầm trung, hiệu năng ổn định, tiết kiệm điện.", "PC GVN Intel i3-12700F/ VGA RX 6500XT (Powered by ASUS)", 10 },
                    { 32, 2, "/Assets/img/Products/2/32.jpg", 31690000.0, "Cấu hình mạnh mẽ, chiến tốt mọi game Esport, hỗ trợ làm đồ họa.", "PC GVN Homework i3 12100", 7 },
                    { 33, 2, "/Assets/img/Products/2/33.jpg", 135990000.0, "Cấu hình siêu cấp dành cho streamer và gamer chuyên nghiệp.", "PC GVN Homework I3 14100", 12 },
                    { 34, 2, "/Assets/img/Products/2/34.jpg", 162990000.0, "Cấu hình khủng với i3 - GT, chơi mọi tựa game ở 4K.", "PC GVN Homework i3 - GT", 3 },
                    { 35, 2, "/Assets/img/Products/2/35.jpg", 17490000.0, "Máy tính chơi game tầm trung, hiệu năng ổn định, tiết kiệm điện.", "PC GVN INTEL I5-12400F/VGA RTX 5050", 9 },
                    { 36, 2, "/Assets/img/Products/2/36.jpg", 28990000.0, "Cấu hình mạnh mẽ, chiến tốt mọi game Esport, hỗ trợ làm đồ họa.", "PC GVN x Corsair iCUE (Intel i5-14400F/ VGA RTX 5060)", 6 },
                    { 37, 2, "/Assets/img/Products/2/37.jpg", 18490000.0, "Cấu hình siêu cấp dành cho streamer và gamer chuyên nghiệp.", "PC GVN Intel i5-12400F/ VGA RTX 5060 (Main H)", 5 },
                    { 38, 2, "/Assets/img/Products/2/38.jpg", 17990000.0, "Cấu hình khủng với Ryzen 9 và RTX 4060, chơi mọi tựa game ở 4K.", "PC GVN Intel i5-12400F/ VGA RTX 4060", 2 },
                    { 39, 2, "/Assets/img/Products/2/39.jpg", 30990000.0, "Máy tính chơi game tầm trung, hiệu năng ổn định, tiết kiệm điện.", "PC GVN x MSI PROJECT ZERO WHITE (Intel i5-14400F/ VGA RTX 5060)", 8 },
                    { 40, 2, "/Assets/img/Products/2/40.jpg", 30790000.0, "Máy tính chơi game tầm trung, hiệu năng ổn định, tiết kiệm điện.", "PC GVN Intel i5-14400F/ VGA RTX 5060 Ti", 4 },
                    { 41, 2, "/Assets/img/Products/2/41.jpg", 31690000.0, "Cấu hình mạnh mẽ, chiến tốt mọi game Esport, hỗ trợ làm đồ họa.", "PC GVN Intel i7-14700F/ VGA RTX 3050", 7 },
                    { 42, 2, "/Assets/img/Products/2/42.jpg", 235990000.0, "Cấu hình siêu cấp dành cho streamer và gamer chuyên nghiệp.", "PC GVN Intel i7-14700F/ VGA RTX 3060", 12 },
                    { 43, 2, "/Assets/img/Products/2/43.jpg", 462990000.0, "Cấu hình khủng với i7-14700F và VGA RTX 4060, chơi mọi tựa game ở 4K.", "PC GVN Intel i7-14700F/ VGA RTX 4060", 3 },
                    { 44, 2, "/Assets/img/Products/2/44.jpg", 58390000.0, "Máy tính chơi game tầm trung, hiệu năng ổn định, tiết kiệm điện.", "PC GVN Intel i7-14700F/ VGA RTX 5080", 10 },
                    { 45, 2, "/Assets/img/Products/2/45.jpg", 51690000.0, "Cấu hình mạnh mẽ, chiến tốt mọi game Esport, hỗ trợ làm đồ họa.", "PC GVN Intel i7-14700F/ VGA RTX 5060", 7 },
                    { 46, 2, "/Assets/img/Products/2/46.jpg", 135000000.0, "Cấu hình siêu cấp dành cho streamer và gamer chuyên nghiệp.", "PC GVN x AORUS XTREME ICE (Intel i9-14900K/ VGA RTX 4080 Super)", 2 },
                    { 47, 2, "/Assets/img/Products/2/47.jpg", 52990000.0, "Cấu hình siêu cấp dành cho streamer và gamer chuyên nghiệp ko chuyên.", "PC GVN Intel i9-14900K/ VGA RTX 4060 Ti", 3 },
                    { 48, 2, "/Assets/img/Products/2/48.jpg", 38990000.0, "Cấu hình khủng với core Ultra 7 và VGA RTX 5060, chơi mọi tựa game ở 4K.", "PC GVN Intel Core Ultra 7 265KF/ VGA RTX 5060", 1 },
                    { 49, 2, "/Assets/img/Products/2/49.jpg", 75990000.0, "Máy tính chơi game tầm cao, hiệu năng đỉnh, tiết kiệm điện.", "PC GVN Intel Core Ultra 7 265KF/ VGA RTX 5080 (Powered by MSI)", 4 },
                    { 50, 2, "/Assets/img/Products/2/50.jpg", 71190000.0, "Cấu hình mạnh mẽ, chiến tốt mọi game Esport, hỗ trợ làm đồ họa.", "PC GVN Intel Core Ultra 7 265KF/ VGA RTX 5080", 6 },
                    { 51, 2, "/Assets/img/Products/2/51.jpg", 50490000.0, "Cấu hình mạnh mẽ, chiến tốt mọi game Esport, hỗ trợ làm đồ họa.", "PC GVN Intel Core Ultra 7 265KF/ VGA RTX 5070", 5 },
                    { 52, 2, "/Assets/img/Products/2/52.jpg", 58990000.0, "Cấu hình siêu cấp dành cho streamer và gamer chuyên nghiệp.", "PC GVN Intel Core Ultra 7 265KF/ VGA RTX 5070 Ti (Powered by MSI)", 2 },
                    { 53, 2, "/Assets/img/Products/2/53.jpg", 162990000.0, "Cấu hình khủng với Ultra core i9 và VGA RTX 5090, chơi mọi tựa game ở 4K.", "PC GVN INT x MSI Dragon ACE (Intel Core Ultra 9 285K/ VGA RTX 5090) (Powered by MSI)", 3 },
                    { 54, 2, "/Assets/img/Products/2/54.jpg", 39990000.0, "Máy tính chơi game tầm trung, hiệu năng ổn định, tiết kiệm điện.", "PC GVN AMD R9-7900/ VGA RTX 3060", 4 },
                    { 55, 2, "/Assets/img/Products/2/55.jpg", 138000000.0, "Cấu hình mạnh mẽ, chiến tốt mọi game Esport, hỗ trợ làm đồ họa.", "PC GVN AMD R9-9950X/ VGA RTX 4090", 6 },
                    { 56, 3, "/Assets/img/Products/3/56.jpg", 3890000.0, "Màn hình 27 inch, độ phân giải 2K, tấm nền IPS cho màu sắc trung thực, tần số quét 100Hz, thiết kế trắng hiện đại, phù hợp làm việc và giải trí.", "Màn hình Acer SA272U-E White 27\" IPS 2K 100Hz", 15 },
                    { 57, 3, "/Assets/img/Products/3/57.jpg", 2550000.0, "Màn hình 27 inch IPS, tần số quét 120Hz mượt mà, hiển thị sắc nét, thích hợp chơi game và làm việc văn phòng.", "Màn hình Acer KA272 G0 27\" IPS 120Hz", 20 },
                    { 58, 3, "/Assets/img/Products/3/58.jpg", 2290000.0, "Màn hình 25 inch, IPS 120Hz, thời gian phản hồi nhanh, lý tưởng cho game và giải trí hàng ngày.", "Màn hình Acer EK251Q G 25\" IPS 120Hz", 8 },
                    { 59, 3, "/Assets/img/Products/3/59.jpg", 41990000.0, "Màn hình 27 inch, độ phân giải 2K, IPS, tần số quét siêu cao 180Hz, tối ưu cho game thủ, hình ảnh mượt mà và sắc nét.", "Màn hình Acer VG271U M3 27\" IPS 2K 180Hz chuyên game", 10 },
                    { 60, 3, "/Assets/img/Products/3/60.jpg", 17900000.0, "Màn hình 22 inch, IPS, tần số quét 100Hz, hiển thị màu sắc chính xác, gọn nhẹ, phù hợp học tập và làm việc văn phòng.", "Màn hình ACER EK221Q E3 22\" IPS 100Hz", 10 },
                    { 61, 3, "/Assets/img/Products/3/61.jpg", 7290000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình cong AOC C27G4Z 27\" 280Hz Adaptive Sync chuyên game", 15 },
                    { 62, 3, "/Assets/img/Products/3/62.jpg", 4990000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình AOC Q27G40E 27\" Fast IPS 2K 180Hz chuyên game", 20 },
                    { 63, 3, "/Assets/img/Products/3/63.jpg", 5290000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình AOC 25G4K 25\" Fast IPS 420Hz chuyên game", 8 },
                    { 64, 3, "/Assets/img/Products/3/64.jpg", 33990000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình AOC 24G11ZE 24\" Fast IPS 240Hz chuyên game", 10 },
                    { 65, 3, "/Assets/img/Products/3/65.jpg", 33990000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình AOC Agon Pro AG276QSD 27\" OLED 2K 360Hz chuyên game", 10 },
                    { 66, 3, "/Assets/img/Products/3/66.jpg", 2990000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình di động Asus ZenScreen MB169CK 16\" IPS FHD USBC", 15 },
                    { 67, 3, "/Assets/img/Products/3/67.jpg", 7690000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình ASUS ProArt PA278QV 27\" IPS 2K 75Hz chuyên đồ họa", 20 },
                    { 68, 3, "/Assets/img/Products/3/68.jpg", 20500000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình ASUS ProArt PA27JCV 27\" IPS 5K USBC chuyên đồ họa", 8 },
                    { 69, 3, "/Assets/img/Products/3/69.jpg", 2350000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình ASUS VZ249HG 24\" IPS 120Hz viền mỏng", 10 },
                    { 70, 3, "/Assets/img/Products/3/70.jpg", 40990000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình cong Asus ROG Swift PG39WCDM 39\" WOLED 2K 240Hz USBC chuyên game", 10 },
                    { 71, 3, "/Assets/img/Products/3/71.jpg", 6890000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình Dell Pro Plus P2725D 27\" IPS 2K 100Hz", 15 },
                    { 72, 3, "/Assets/img/Products/3/72.jpg", 16490000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình Dell UltraSharp U2725QE 27\" IPS 4K 120Hz USBC chuyên đồ họa", 20 },
                    { 73, 3, "/Assets/img/Products/3/73.jpg", 6490000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình Dell UltraSharp U2424HE 24\" IPS 120Hz USBC", 8 },
                    { 74, 3, "/Assets/img/Products/3/74.jpg", 23790000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình cong Dell Alienware AW3423DW 34\" QD-OLED 2K 175Hz G-Sync Ultimate", 10 },
                    { 75, 3, "/Assets/img/Products/3/75.jpg", 23990000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình Dell UltraSharp U4323QE 43\" IPS 4K chuyên đồ họa", 10 },
                    { 76, 3, "/Assets/img/Products/3/76.jpg", 2990000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình GIGABYTE G25F2 25\" IPS 200Hz chuyên game", 15 },
                    { 77, 3, "/Assets/img/Products/3/77.jpg", 16490000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình GIGABYTE MO27Q28G 27\" WOLED 2K 280Hz chuyên game", 20 },
                    { 78, 3, "/Assets/img/Products/3/78.jpg", 33990000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình GIGABYTE FO32U2P 32\" OLED 4K 240Hz chuyên game", 8 },
                    { 79, 3, "/Assets/img/Products/3/79.jpg", 33990000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình GIGABYTE FO27Q5P 27\" OLED 2K 500Hz chuyên game", 10 },
                    { 80, 3, "/Assets/img/Products/3/80.jpg", 7590000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình cong GIGABYTE G34WQC2 34\" 2K 200Hz chuyên game", 10 },
                    { 81, 3, "/Assets/img/Products/3/81.jpg", 6690000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình cong HKC MG34H18Q 34\" 2K 165Hz USBC", 15 },
                    { 82, 3, "/Assets/img/Products/3/82.jpg", 1790000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình HKC MB24V9-U 24\" IPS 100Hz", 20 },
                    { 83, 3, "/Assets/img/Products/3/83.jpg", 3090000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình HKC MG27H7F 27\" Fast IPS 165Hz Gsync", 8 },
                    { 84, 3, "/Assets/img/Products/3/84.jpg", 4890000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình HKC MG27S9QS 27\" Fast IPS 2K 155Hz Gsync", 10 },
                    { 85, 3, "/Assets/img/Products/3/85.jpg", 2190000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình HKC M27A9X-W Black 75Hz", 10 },
                    { 86, 3, "/Assets/img/Products/3/86.jpg", 5390000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình LG 27US500-W Ultrafine 27\" IPS 4K HDR10", 15 },
                    { 87, 3, "/Assets/img/Products/3/87.jpg", 2090000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình LG 22U401A-B 22\" 100Hz HDR10", 20 },
                    { 88, 3, "/Assets/img/Products/3/88.jpg", 6090000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình LG 27GR75Q-B UltraGear 27\" IPS 2K 165Hz Gsync chuyên game", 8 },
                    { 89, 3, "/Assets/img/Products/3/89.jpg", 2790000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình LG 27U411A-B 27\" IPS 120Hz HDR10 siêu mỏng", 10 },
                    { 90, 3, "/Assets/img/Products/3/90.jpg", 4690000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình LG 27U631A-B 27\" IPS 2K 100Hz HDR10", 10 },
                    { 91, 3, "/Assets/img/Products/3/91.jpg", 3490000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình cong MSI MAG 275CF X24 27\" 240Hz chuyên game", 15 },
                    { 92, 3, "/Assets/img/Products/3/92.jpg", 3990000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình MSI MAG 272F X24 27\" Rapid IPS 240Hz chuyên game", 20 },
                    { 93, 3, "/Assets/img/Products/3/93.jpg", 30590000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình MSI MPG 321URXW QD-OLED 32\" QD-LED 4K 240Hz chuyên game", 8 },
                    { 94, 3, "/Assets/img/Products/3/94.jpg", 4290000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình MSI MAG 255XF 25\" Rapid IPS 300Hz FreeSync Premium chuyên game", 10 },
                    { 95, 3, "/Assets/img/Products/3/95.jpg", 2190000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình MSI PRO MP251 E2 25\" IPS 120Hz", 10 },
                    { 96, 3, "/Assets/img/Products/3/96.jpg", 7450000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình cong Samsung LC34G55 34\" 2K 165Hz", 15 },
                    { 97, 3, "/Assets/img/Products/3/97.jpg", 28490000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình cong Samsung Odyssey G9 LS49CG934 49\" OLED 2K 240Hz", 20 },
                    { 98, 3, "/Assets/img/Products/3/98.jpg", 30990000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình Samsung Odyssey G8 LS32FG812SEXXV 32\" OLED 4K 240Hz", 8 },
                    { 99, 3, "/Assets/img/Products/3/99.jpg", 36990000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình Samsung Odyssey 3D LS27FG900XEXXV 27\" IPS 4K 165Hz", 10 },
                    { 100, 3, "/Assets/img/Products/3/100.jpg", 36890000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình cong Samsung Odyssey G9 LS49DG930 49\" OLED 2K 240Hz USBC", 10 },
                    { 101, 3, "/Assets/img/Products/3/101.jpg", 3890000.0, "Màn hình gaming tốc độ cao, 300Hz, độ phân giải 2K, hỗ trợ G-Sync.", "Màn hình ViewSonic VX2480-2K-SHD-2 24\" IPS 2K 100Hz", 15 },
                    { 102, 3, "/Assets/img/Products/3/102.jpg", 5290000.0, "Màn hình siêu rộng, hỗ trợ HDR10, phù hợp làm việc đa nhiệm.", "Màn hình ViewSonic ColorPro VP2456A 24\" IPS 120Hz USBC chuyên đồ hoạ", 20 },
                    { 103, 3, "/Assets/img/Products/3/103.jpg", 1990000.0, "Chuẩn màu cao, độ chính xác Delta E < 2, dành cho thiết kế đồ họa.", "Màn hình Viewsonic VA2432-H 24\" IPS 100Hz viền mỏng", 8 },
                    { 104, 3, "/Assets/img/Products/3/104.jpg", 1990000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình Viewsonic VA2432-H-2 24\" IPS 100Hz viền mỏng", 10 },
                    { 105, 3, "/Assets/img/Products/3/105.jpg", 3290000.0, "Màn hình OLED 4K 240Hz, màu sắc rực rỡ, độ tương phản tuyệt vời.", "Màn hình ViewSonic VX2779-HD-PRO 27\" IPS 180Hz chuyên game", 10 },
                    { 106, 4, "/Assets/img/Products/4/106.jpg", 890000.0, "Âm thanh cân bằng, phù hợp nghe nhạc và làm việc tại nhà.", "Loa Edifier Bluetooth ES20 Black", 18 },
                    { 107, 4, "/Assets/img/Products/4/107.jpg", 41900000.0, "Loa gaming RGB đồng bộ ánh sáng theo trò chơi.", "Loa Edifier Bluetooth ES300 Ivory", 25 },
                    { 108, 4, "/Assets/img/Products/4/108.jpg", 1790000.0, "Thiết kế độc đáo, âm thanh vòm 3D chân thực.", "Loa máy tính Edifier MR4 White", 7 },
                    { 109, 4, "/Assets/img/Products/4/109.jpg", 1790000.0, "Loa nhỏ gọn, công suất ổn, giá rẻ phù hợp học sinh sinh viên.", "Loa máy tính Edifier MR4", 30 },
                    { 110, 4, "/Assets/img/Products/4/110.jpg", 940000.0, "Âm thanh cân bằng, phù hợp nghe nhạc và làm việc tại nhà.", "Loa Edifier Bluetooth ES20 Ivory", 28 },
                    { 111, 4, "/Assets/img/Products/4/111.jpg", 3890000.0, "Thiết kế độc đáo, âm thanh vòm 3D chân thực.", "Loa Logitech G560", 26 },
                    { 112, 4, "/Assets/img/Products/4/112.jpg", 5790000.0, "Loa nhỏ gọn, công suất ổn, giá rẻ phù hợp học sinh sinh viên.", "Loa Razer Leviathan V2", 17 },
                    { 113, 4, "/Assets/img/Products/4/113.jpg", 2490000.0, "Loa gaming RGB đồng bộ ánh sáng theo trò chơi.", "Loa Razer Leviathan V2 X", 14 },
                    { 114, 4, "/Assets/img/Products/4/114.jpg", 3490000.0, "Loa nhỏ gọn, công suất ổn, giá rẻ phù hợp học sinh sinh viên.", "Loa Razer Nommo V2 X", 24 },
                    { 115, 4, "/Assets/img/Products/4/115.jpg", 6100000.0, "Thiết kế độc đáo, âm thanh vòm 3D chân thực.", "Loa Razer Nommo V2", 23 },
                    { 116, 4, "/Assets/img/Products/4/116.jpg", 490000.0, "Thiết kế độc đáo, âm thanh vòm 3D chân thực.", "Loa SoundMax SB201 Grey", 13 },
                    { 117, 4, "/Assets/img/Products/4/117.jpg", 5790000.0, "Loa nhỏ gọn, công suất ổn, giá rẻ phù hợp học sinh sinh viên.", "Loa Razer Leviathan V2", 17 },
                    { 118, 4, "/Assets/img/Products/4/118.jpg", 2490000.0, "Loa gaming RGB đồng bộ ánh sáng theo trò chơi.", "Loa Razer Leviathan V2 X", 14 },
                    { 119, 4, "/Assets/img/Products/4/119.jpg", 3490000.0, "Loa nhỏ gọn, công suất ổn, giá rẻ phù hợp học sinh sinh viên.", "Loa Razer Nommo V2 X", 24 },
                    { 120, 5, "/Assets/img/Products/5/120.jpg", 405000.0, "Chuột gaming Logitech G102 LightSync Black – cảm biến chính xác, thiết kế RGB nổi bật.", "Chuột Logitech G102 LightSync Black", 25 },
                    { 121, 5, "/Assets/img/Products/5/121.jpg", 1090000.0, "Chuột không dây ASUS ROG Strix Impact III – độ trễ thấp, hiệu năng chơi game cao.", "Chuột ASUS ROG Strix Impact III Wireless", 25 },
                    { 122, 5, "/Assets/img/Products/5/122.jpg", 400000.0, "Chuột Razer DeathAdder Essential – huyền thoại gaming với thiết kế công thái học thoải mái.", "Chuột Razer DeathAdder Essential", 25 },
                    { 123, 5, "/Assets/img/Products/5/123.jpg", 1090000.0, "Chuột ASUS ROG Strix Impact III Wireless White – phong cách trắng tinh tế, hiệu năng cao.", "Chuột ASUS ROG Strix Impact III Wireless White", 25 },
                    { 124, 5, "/Assets/img/Products/5/124.jpg", 790000.0, "Chuột Corsair Katar Pro Wireless – kết nối không dây nhanh, cảm biến chính xác.", "Chuột Corsair Katar Pro Wireless", 25 },
                    { 125, 5, "/Assets/img/Products/5/125.jpg", 410000.0, "Chuột Razer DeathAdder Essential White – chuột gaming huyền thoại với cảm biến chính xác.", "Chuột Razer DeathAdder Essential White", 25 },
                    { 126, 5, "/Assets/img/Products/5/126.jpg", 750000.0, "Chuột không dây Logitech G304 – hiệu năng cao, tuổi thọ pin dài.", "Chuột Logitech G304 Wireless", 25 },
                    { 127, 5, "/Assets/img/Products/5/127.jpg", 990000.0, "Chuột Razer Basilisk V3 – cảm biến cao cấp, nút tùy chỉnh linh hoạt.", "Chuột Razer Basilisk V3", 25 },
                    { 128, 5, "/Assets/img/Products/5/128.jpg", 3690000.0, "Chuột Razer Viper V3 Pro Đen – siêu nhẹ, hiệu suất chuyên nghiệp.", "Chuột Razer Không dây Viper V3 Pro Đen", 25 },
                    { 129, 5, "/Assets/img/Products/5/129.jpg", 790000.0, "Chuột Logitech G502 Hero – độ chính xác cao, thiết kế công thái học.", "Chuột Logitech G502 Hero Gaming", 25 },
                    { 130, 5, "/Assets/img/Products/5/130.jpg", 380000.0, "Chuột DareU EM911X RGB Đen – giá rẻ, hiệu năng tốt, thiết kế đẹp.", "Chuột DareU Không dây EM911X RGB Đen", 25 },
                    { 131, 5, "/Assets/img/Products/5/131.jpg", 400000.0, "Chuột Asus TUF Gaming M3 Gen II – bền bỉ, thiết kế chuẩn gaming.", "Chuột Asus TUF Gaming M3 Gen II", 25 },
                    { 132, 5, "/Assets/img/Products/5/132.jpg", 330000.0, "Chuột Logitech M331 Silent – yên tĩnh, thoải mái, tiết kiệm năng lượng.", "Chuột không dây Logitech M331 Silent Black", 25 },
                    { 133, 5, "/Assets/img/Products/5/133.jpg", 415000.0, "Chuột Logitech G102 LightSync White – đèn RGB rực rỡ, độ chính xác cao.", "Chuột Logitech G102 LightSync White", 25 },
                    { 134, 5, "/Assets/img/Products/5/134.jpg", 3290000.0, "Chuột Logitech G Pro X Superlight 2 – siêu nhẹ, tốc độ và chính xác cao.", "Chuột Logitech G Pro X Superlight 2 Dex Wireless Black", 25 },
                    { 135, 5, "/Assets/img/Products/5/135.jpg", 750000.0, "Chuột Logitech G304 White – hiệu năng mạnh mẽ trong thiết kế nhỏ gọn.", "Chuột Logitech G304 Wireless White", 25 },
                    { 136, 5, "/Assets/img/Products/5/136.jpg", 2390000.0, "Chuột Logitech MX Master 3S – công thái học đỉnh cao, cuộn nhanh siêu êm.", "Chuột Logitech MX Master 3S Graphite", 25 },
                    { 137, 5, "/Assets/img/Products/5/137.jpg", 2950000.0, "Chuột Logitech G Pro X Superlight 2 White – trọng lượng nhẹ, phản hồi nhanh.", "Chuột Logitech G Pro X Superlight 2 Dex Wireless White", 25 },
                    { 138, 5, "/Assets/img/Products/5/138.jpg", 450000.0, "Chuột Logitech Pebble M350S Graphite – nhỏ gọn, yên tĩnh, hiện đại.", "Chuột Logitech Pebble Mouse 2 M350S Graphite", 25 },
                    { 139, 5, "/Assets/img/Products/5/139.jpg", 380000.0, "Chuột DareU EM911X RGB Trắng – phong cách trẻ trung, pin bền lâu.", "Chuột DareU Không dây EM911X RGB Trắng", 25 },
                    { 140, 5, "/Assets/img/Products/5/140.jpg", 990000.0, "Chuột Razer Cobra – tốc độ và độ chính xác cao cho game thủ.", "Chuột Razer Cobra", 25 },
                    { 141, 5, "/Assets/img/Products/5/141.jpg", 1620000.0, "Chuột Razer Viper V3 HyperSpeed – kết nối không dây siêu nhanh.", "Chuột Razer Viper V3 HyperSpeed", 25 },
                    { 142, 5, "/Assets/img/Products/5/142.jpg", 3290000.0, "Chuột Logitech G Pro X Superlight 2 Black – chuẩn eSports chuyên nghiệp.", "Chuột Logitech G Pro X Superlight 2 Black", 25 },
                    { 143, 5, "/Assets/img/Products/5/143.jpg", 450000.0, "Chuột Logitech Pebble M350S Rose – màu hồng dễ thương, yên tĩnh.", "Chuột Logitech Pebble Mouse 2 M350S Rose", 25 },
                    { 144, 5, "/Assets/img/Products/5/144.jpg", 3650000.0, "Chuột Razer Viper V3 Pro Trắng – siêu nhẹ, hiệu năng cực đỉnh.", "Chuột Razer Không dây Viper V3 Pro Trắng", 25 },
                    { 145, 5, "/Assets/img/Products/5/145.jpg", 990000.0, "Chuột ASUS ROG Strix Impact III – thiết kế nhỏ gọn, độ nhạy cao.", "Chuột ASUS ROG Strix Impact III", 25 },
                    { 146, 5, "/Assets/img/Products/5/146.jpg", 1590000.0, "Chuột Logitech MX Anywhere 3S – linh hoạt, kết nối đa thiết bị.", "Chuột Logitech MX Anywhere 3S Graphite", 25 },
                    { 147, 5, "/Assets/img/Products/5/147.jpg", 150000.0, "Chuột DareU LM115G Black – giá rẻ, phù hợp văn phòng, pin lâu.", "Chuột không dây DareU LM115G Black", 25 },
                    { 148, 5, "/Assets/img/Products/5/148.jpg", 1250000.0, "Chuột công thái học Logitech Lift Vertical – giảm căng thẳng cổ tay, thiết kế sang trọng.", "Chuột công thái học Logitech Lift Vertical", 25 },
                    { 149, 5, "/Assets/img/Products/5/149.jpg", 250000.0, "Chuột Logitech M190 Black – yên tĩnh, thoải mái, pin lâu.", "Chuột không dây Logitech M190 Black", 25 },
                    { 150, 6, "/Assets/img/Products/6/150.jpg", 450000.0, "Tai nghe gaming Rapoo, thiết kế hiện đại, âm thanh sống động cho trải nghiệm chơi game thoải mái.", "Tai nghe Gaming Rapoo VH160S Black", 32 },
                    { 151, 6, "/Assets/img/Products/6/151.jpg", 8390000.0, "Tai nghe không dây HyperX Alpha 2, chất lượng âm thanh mạnh mẽ, phù hợp cho game thủ chuyên nghiệp.", "Tai nghe HP HyperX Alpha 2 Wireless Black", 18 },
                    { 152, 6, "/Assets/img/Products/6/152.jpg", 640000.0, "Tai nghe Onikuma T302 màu hồng, kết nối Bluetooth TWS với hiệu ứng RGB độc đáo.", "Tai nghe Onikuma In-ear T302 TWS RGB Bluetooth Hồng", 27 },
                    { 153, 6, "/Assets/img/Products/6/153.jpg", 640000.0, "Tai nghe Onikuma T302 phiên bản xám trắng, âm thanh rõ ràng, thiết kế hiện đại.", "Tai nghe Onikuma In-ear T302 TWS RGB Bluetooth Xám Trắng", 19 },
                    { 154, 6, "/Assets/img/Products/6/154.jpg", 540000.0, "Tai nghe Onikuma T301 trắng, nhỏ gọn, âm thanh trong trẻo, kết nối Bluetooth ổn định.", "Tai nghe Onikuma In-ear T301 TWS Bluetooth Trắng", 46 },
                    { 155, 6, "/Assets/img/Products/6/155.jpg", 590000.0, "Tai nghe Onikuma T209 hỗ trợ dịch trực tiếp, thiết kế màu đen sang trọng.", "Tai nghe Onikuma In-ear T209 Bluetooth Live Translation Đen", 24 },
                    { 156, 6, "/Assets/img/Products/6/156.jpg", 640000.0, "Tai nghe Onikuma T20 trắng, công nghệ Bluetooth TWS cho âm thanh không dây ổn định.", "Tai nghe Onikuma In-ear T20 TWS Bluetooth Trắng", 34 },
                    { 157, 6, "/Assets/img/Products/6/157.jpg", 640000.0, "Tai nghe Onikuma T20 đen, âm thanh chất lượng, thiết kế tiện lợi.", "Tai nghe Onikuma In-ear T20 TWS Bluetooth Đen", 21 },
                    { 158, 6, "/Assets/img/Products/6/158.jpg", 590000.0, "Tai nghe Onikuma T18 trắng, tích hợp ENC giảm ồn, âm thanh rõ ràng.", "Tai nghe Onikuma In-ear T18 ENC Bluetooth Trắng", 48 },
                    { 159, 6, "/Assets/img/Products/6/159.jpg", 590000.0, "Tai nghe Onikuma T18 đen, thiết kế sang trọng, công nghệ ENC chống ồn hiệu quả.", "Tai nghe Onikuma In-ear T18 ENC Bluetooth Đen", 42 },
                    { 160, 6, "/Assets/img/Products/6/160.jpg", 990000.0, "Tai nghe Onikuma B5 tai mèo trắng, hỗ trợ 3 chế độ kết nối và hiệu ứng RGB.", "Tai nghe Onikuma Tai Mèo B5 RGB Tri Mode Trắng", 36 },
                    { 161, 6, "/Assets/img/Products/6/161.jpg", 990000.0, "Tai nghe Onikuma B5 tai mèo hồng, thiết kế đáng yêu, kết nối đa năng và RGB rực rỡ.", "Tai nghe Onikuma Tai Mèo B5 RGB Tri Mode Hồng", 28 },
                    { 162, 6, "/Assets/img/Products/6/162.jpg", 2290000.0, "Tai nghe Razer Kraken Kitty V2 Quartz, thiết kế tai mèo màu hồng, kết nối Bluetooth hiện đại.", "Tai nghe Razer Kraken Kitty V2 BT Quartz", 14 },
                    { 163, 6, "/Assets/img/Products/6/163.jpg", 2290000.0, "Tai nghe Razer Kraken Kitty V2 màu trắng, âm thanh sống động và thiết kế đẹp mắt.", "Tai nghe Razer Kraken Kitty V2 BT White", 38 },
                    { 164, 6, "/Assets/img/Products/6/164.jpg", 2290000.0, "Tai nghe Razer Kraken Kitty V2 màu đen, âm thanh mạnh mẽ và pin bền bỉ.", "Tai nghe Razer Kraken Kitty V2 BT Black", 20 },
                    { 165, 6, "/Assets/img/Products/6/165.jpg", 890000.0, "Tai nghe DareU EH925 MAX, màu đen đỏ thể thao, chất lượng âm thanh nổi bật.", "Tai nghe DareU EH925 MAX Black Red", 30 },
                    { 166, 6, "/Assets/img/Products/6/166.jpg", 1790000.0, "Tai nghe Razer Kraken Kitty V3 X màu trắng, thiết kế thời trang và âm thanh cao cấp.", "Tai nghe Razer Kraken Kitty V3 X White", 25 },
                    { 167, 6, "/Assets/img/Products/6/167.jpg", 1790000.0, "Tai nghe Razer Kraken Kitty V3 X màu hồng Quartz, phong cách và hiệu suất vượt trội.", "Tai nghe Razer Kraken Kitty V3 X Quartz", 13 },
                    { 168, 6, "/Assets/img/Products/6/168.jpg", 1790000.0, "Tai nghe Razer Kraken Kitty V3 X màu đen, thiết kế đẹp và âm thanh sống động.", "Tai nghe Razer Kraken Kitty V3 X Black", 44 },
                    { 169, 6, "/Assets/img/Products/6/169.jpg", 390000.0, "Tai nghe DareU EH732 RGB màu đỏ, hiệu ứng ánh sáng đẹp mắt và âm thanh tốt.", "Tai nghe DareU EH732 RGB Red", 17 },
                    { 170, 6, "/Assets/img/Products/6/170.jpg", 390000.0, "Tai nghe DareU EH732 RGB màu vàng, thiết kế bền và đeo thoải mái.", "Tai nghe DareU EH732 RGB Yellow", 23 },
                    { 171, 6, "/Assets/img/Products/6/171.jpg", 2100000.0, "Tai nghe Logitech G Pro X SE cao cấp, dành cho game thủ chuyên nghiệp.", "Tai nghe Logitech G Pro X SE", 31 },
                    { 172, 6, "/Assets/img/Products/6/172.jpg", 2990000.0, "Tai nghe Razer BlackShark V3 X HyperSpeed, kết nối không dây tốc độ cao.", "Tai nghe Razer BlackShark V3 X HyperSpeed", 27 },
                    { 173, 6, "/Assets/img/Products/6/173.jpg", 4790000.0, "Tai nghe Razer BlackShark V3, chất lượng âm thanh đỉnh cao và thiết kế chắc chắn.", "Tai nghe Razer BlackShark V3", 16 },
                    { 174, 6, "/Assets/img/Products/6/174.jpg", 7890000.0, "Tai nghe Razer BlackShark V3 Pro màu trắng, công nghệ âm thanh cao cấp.", "Tai nghe Razer BlackShark V3 Pro White", 43 },
                    { 175, 6, "/Assets/img/Products/6/175.jpg", 7890000.0, "Tai nghe Razer BlackShark V3 Pro màu đen, âm thanh mạnh mẽ và sang trọng.", "Tai nghe Razer BlackShark V3 Pro Black", 41 },
                    { 176, 6, "/Assets/img/Products/6/176.jpg", 1050000.0, "Tai nghe Razer BlackShark V2 X dành cho PlayStation, hiệu suất và sự thoải mái tuyệt vời.", "Tai nghe Razer BlackShark V2 X For PlayStation", 37 },
                    { 177, 6, "/Assets/img/Products/6/177.jpg", 3490000.0, "Tai nghe Razer Barracuda X Chroma Black, thiết kế hiện đại và đèn RGB nổi bật.", "Tai nghe Razer Barracuda X Chroma Black", 15 },
                    { 178, 6, "/Assets/img/Products/6/178.jpg", 4090000.0, "Tai nghe HyperX Cloud III S Wireless Black, không dây, âm thanh đắm chìm.", "Tai nghe HP HyperX Cloud III S Wireless Black", 26 },
                    { 179, 6, "/Assets/img/Products/6/179.jpg", 4090000.0, "Tai nghe HyperX Cloud III S Wireless Black Red, phong cách, pin lâu và âm thanh mạnh mẽ.", "Tai nghe HP HyperX Cloud III S Wireless Black Red", 33 },
                    { 180, 6, "/Assets/img/Products/6/180.jpg", 2090000.0, "sản phẩm nổi bật trong dòng bàn phím cơ gaming với thiết kế hiện đại và nhiều tính năng ưu việt.", "Bàn phím Leobog Hi86 TM", 20 },
                    { 181, 7, "/Assets/img/Products/7/181.jpg", 170000.0, "Bàn phím giả cơ giá rẻ, phù hợp cho văn phòng và học tập. Thiết kế full-size, phím bấm êm, hành trình phím tốt và được khắc laser chống mờ. Có khả năng chống tràn nước hiệu quả.", "Bàn phím Rapoo NK1900 màu đen", 20 },
                    { 182, 7, "/Assets/img/Products/7/182.jpg", 1290000.0, "Bàn phím cơ layout 75% nhỏ gọn, kết nối đa dạng với 3 chế độ: USB-C, Bluetooth 5.0, và 2.4Ghz. Sử dụng Red Switch cho cảm giác gõ mượt mà, lý tưởng cho cả gaming và gõ văn bản.", "Bàn phím cơ E-Dra không dây EK375 Pro Beta Red Switch", 15 },
                    { 183, 7, "/Assets/img/Products/7/183.jpg", 2090000.0, "Thiết kế TKL (Tenkeyless) chuyên nghiệp cho game thủ. Thân phím hoàn toàn bằng nhôm máy bay chắc chắn, trang bị switch HyperX Aqua (Tactile) cho phản hồi nhanh và chính xác. LED RGB rực rỡ tùy chỉnh qua phần mềm NGENUITY.", "Bàn phím cơ HyperX Alloy Origins Core TKL RGB Aqua Switch", 20 },
                    { 184, 7, "/Assets/img/Products/7/184.jpg", 690000.0, "Phiên bản nâng cấp của E-Dra EK375, layout 75% tối ưu không gian. Sử dụng Beta Linear switch cho trải nghiệm gõ trơn tru, mượt mà. Keycap PBT double-shot bền bỉ, LED RGB 16.8 triệu màu.", "Bàn phím cơ gaming E-Dra EK375v2 Beta Linear switch", 16 },
                    { 185, 7, "/Assets/img/Products/7/185.jpg", 550000.0, "Bàn phím TKL (87 phím) giá cả phải chăng, sử dụng Dream switch độc quyền của DareU cho cảm giác gõ êm ái. Keycap ABS Doubleshot và hệ thống LED Rainbow tạo điểm nhấn.", "Bàn phím cơ Gaming DAREU EK87 v2 Gray Black Dream Switch", 11 },
                    { 186, 7, "/Assets/img/Products/7/186.jpg", 1790000.0, "Phiên bản đặc biệt với chủ đề 'World Tour Việt Nam', thiết kế đậm chất văn hóa Việt. Layout 75% nhỏ gọn, kết cấu Gasket Mount êm ái, hỗ trợ 3 chế độ kết nối và tính năng Hotswap tiện lợi.", "Bàn phím AKKO 5075B Plus Red World Tour VIET NAM", 15 },
                    { 187, 7, "/Assets/img/Products/7/187.jpg", 690000.0, "Tương tự phiên bản Beta, E-Dra EK375v2 Alpha mang đến trải nghiệm gõ Linear mượt mà, không khấc. Layout 75%, keycap PBT bền bỉ và LED RGB sống động, là lựa chọn tuyệt vời trong phân khúc.", "Bàn phím cơ gaming E-Dra EK375v2 Alpha Linear switch", 10 },
                    { 188, 7, "/Assets/img/Products/7/188.jpg", 2490000.0, "Bàn phím full-size cao cấp với 3 chế độ kết nối (Tri-mode). Trang bị CuCl2 Silent Switch siêu yên tĩnh, lý tưởng cho môi trường làm việc chung. Thiết kế Gasket Mount và keycap PBT chất lượng cao.", "Bàn phím SKYLOONG GK104 PRO Twilight Tri-mode CuCl2 Silent Switch", 25 },
                    { 189, 7, "/Assets/img/Products/7/189.jpg", 1290000.0, "Thiết kế TKL 87 phím với tông màu Prunus Lannesiana (Hoa Anh Đào) nghệ thuật. Kết nối 3 chế độ, sử dụng Stellar Rose switch (Linear) cho cảm giác gõ nhẹ nhàng và êm ái.", "Bàn phím AKKO TAC87 3 MODE Prunus Lannesiana Stellar Rose switch", 20 },
                    { 190, 7, "/Assets/img/Products/7/190.jpg", 1490000.0, "Layout 75% với phối màu Chicago cổ điển. Sử dụng switch AKKO V3 Pro (Piano) cao cấp, kết cấu Gasket Mount và foam tiêu âm cho âm thanh gõ đặc trưng, thỏa mãn người dùng khó tính.", "Bàn phím cơ AKKO 5075B Plus TM Chicago Piano V3 Pro Switch", 20 },
                    { 191, 7, "/Assets/img/Products/7/191.jpg", 2490000.0, "Bàn phím nhỏ gọn (68 phím) với kết cấu Gasket Mount. Ice Crystal switch trong suốt độc đáo cho cảm giác gõ mượt mà và âm thanh trong trẻo. Hỗ trợ 3 chế độ kết nối, keycap PBT double-shot.", "Bàn phím AULA F68 TM (Xanh lá/ Ice Crystal switch ) F68G", 11 },
                    { 192, 7, "/Assets/img/Products/7/192.jpg", 750000.0, "Bàn phím full-size 100 phím nhỏ gọn, tiết kiệm không gian. Kết nối 3 chế độ tiện lợi, sử dụng Brown switch cho phản hồi xúc giác rõ ràng, cân bằng giữa chơi game và làm việc.", "Bàn phím AULA S100 PRO TM (Xanh dương+trắng+tím đậm/ Brown switch) S100PRO02", 10 },
                    { 193, 7, "/Assets/img/Products/7/193.jpg", 2090000.0, "Thiết kế 65% độc đáo với vỏ nhôm CNC cao cấp. Jadeite switch mang lại trải nghiệm gõ đầm tay và âm thanh nổ giòn. Kết nối 3 chế độ, LED RGB và núm xoay kim loại đa chức năng.", "Bàn phím Leobog AMG65 TM (Tím đen/ Jadeite switch) AMG6501", 15 },
                    { 194, 7, "/Assets/img/Products/7/194.jpg", 690000.0, "Bàn phím cơ có dây full-size với thiết kế hầm hố và kê tay đi kèm. Red switch mang lại lợi thế trong game nhờ tốc độ phản hồi nhanh. LED RGB Rainbow nhiều hiệu ứng đẹp mắt.", "Bàn phím AULA F2058 có dây (Đen Red switch) F205802", 18 },
                    { 195, 7, "/Assets/img/Products/7/195.jpg", 1490000.0, "Layout 75% kèm núm xoay tiện lợi, kết cấu Gasket Mount êm ái. Phối màu Desert Grey Blue, sử dụng Ice Pink switch (Linear) cho cảm giác gõ nhẹ và mượt, hỗ trợ 3 chế độ kết nối.", "Bàn phím FL-Esports CMK75 Không dây Desert Grey Blue FLCMMK Ice Pink", 20 },
                    { 196, 7, "/Assets/img/Products/7/196.jpg", 2090000.0, "Phiên bản màu trắng tinh khôi của dòng AMG65 vỏ nhôm. Layout 65% nhỏ gọn, Jadeite switch cho âm thanh gõ độc đáo. Kết nối 3 chế độ, núm xoay kim loại và LED RGB.", "Bàn phím Leobog AMG65 TM (Trắng/ Jadeite switch) AMG6502", 19 },
                    { 197, 7, "/Assets/img/Products/7/197.jpg", 1290000.0, "Lấy cảm hứng từ Trà xanh Đậu đỏ, chiếc bàn phím TKL này có phối màu độc đáo. Kết nối 3 chế độ linh hoạt, Stellar Rose switch (Linear) nhẹ nhàng, phù hợp cho người dùng yêu thẩm mỹ.", "Bàn phím AKKO TAC87 3 MODE Matcha Red Bean Stellar Rose switch", 23 },
                    { 198, 7, "/Assets/img/Products/7/198.jpg", 1290000.0, "Bàn phím TKL với thiết kế Horizon độc đáo, kết nối 3 chế độ. Sử dụng Mirror switch (Linear) cho hành trình phím mượt mà, là một lựa chọn cân bằng giữa công việc và giải trí.", "Bàn phím AKKO TAC87 3 MODE Horizon Mirror switch", 26 },
                    { 199, 7, "/Assets/img/Products/7/199.jpg", 2580000.0, "Bàn phím full-size cao cấp dành cho người dùng chuyên nghiệp. Kết nối 3 chế độ, CuCl2 Silent Switch siêu tĩnh, kết cấu Gasket Mount và nhiều lớp foam tiêu âm cho trải nghiệm gõ yên tĩnh tuyệt đối.", "Bàn phím SKYLOONG GK104 PRO Quantum Mech 2 Tri-mode CuCl2 Silent Switch", 16 },
                    { 200, 7, "/Assets/img/Products/7/200.jpg", 590000.0, "Layout 98 phím độc đáo, giữ lại cụm numpad nhưng vẫn nhỏ gọn hơn full-size. Dream switch độc quyền của DareU cho cảm giác gõ êm, phù hợp cho cả game và văn phòng.", "Bàn phím cơ DareU EK98L Grey Black Dream switch", 28 },
                    { 201, 7, "/Assets/img/Products/7/1201.jpg", 2700000.0, "Bàn phím low-profile siêu mỏng, kết nối không dây LIGHTSPEED tốc độ cao. Thiết kế TKL, switch GL Tactile cho phản hồi nhanh và chính xác. LED RGB LIGHTSYNC tùy chỉnh chuyên nghiệp.", "Bàn phím Logitech G515 TKL Lightspeed Wireless RGB Đen", 30 },
                    { 202, 7, "/Assets/img/Products/7/202.jpg", 1990000.0, "Layout 75% có núm xoay, kết cấu Gasket Mount. Sử dụng Purple Fire V2 switch (Tactile) cho cảm giác gõ có khấc rõ ràng. Hỗ trợ 3 chế độ kết nối và keycap PBT chất lượng cao.", "Bàn phím AULA M75 TM (Xanh + Trắng + Tím/ Purple Fire V2 Switch) M7503", 15 },
                    { 203, 7, "/Assets/img/Products/7/203.jpg", 1990000.0, "Phiên bản màu Lake Placid Blue của CMK75, layout 75% với núm xoay đa năng. Kết cấu Gasket mount, kết nối 3 chế độ, và switch Ice Pink (Linear) mang lại trải nghiệm gõ mượt mà.", "Bàn phím FL-Esports CMK75 Không dây Lake Placid Blue FLCMMK Ice Pink", 25 },
                    { 204, 7, "/Assets/img/Products/7/204.jpg", 1490000.0, "Phiên bản Desert Grey Blue của CMK75 sử dụng Ice Violet switch (Tactile), mang lại phản hồi xúc giác rõ rệt khi gõ. Bàn phím có layout 75%, núm xoay và kết nối 3 chế độ.", "Bàn phím FL-Esports CMK75 Không dây Desert Grey Blue FLCMMK Ice Violet", 20 },
                    { 205, 7, "/Assets/img/Products/7/205.jpg", 2580000.0, "Một phiên bản khác của dòng GK104 Pro, thiết kế full-size chuyên nghiệp, kết nối 3 chế độ và switch CuCl2 siêu tĩnh. Lý tưởng cho người cần sự yên tĩnh tối đa khi làm việc.", "Bàn phím SKYLOONG GK104 PRO Quantum Mech 5 Tri-mode CuCl2 Silent Switch", 10 },
                    { 206, 7, "/Assets/img/Products/7/206.jpg", 1650000.0, "Bàn phím layout 75% vỏ nhôm cao cấp, kết cấu Gasket Mount. Turquoise switch (Linear) cho cảm giác gõ êm và mượt. Hỗ trợ 3 chế độ kết nối, núm xoay và LED RGB.", "Bàn phím Leobog Hi75C PRO TM (Đen contour/ Turquoise switch) HI7509", 15 },
                    { 207, 7, "/Assets/img/Products/7/207.jpg", 1990000.0, "Layout 75% độc đáo, kết nối 3 chế độ và kết cấu Gasket Mount. Crystal switch (Linear) trong suốt cho hành trình phím mượt mà, kết hợp với LED RGB tạo hiệu ứng ánh sáng ấn tượng.", "Bàn phím AULA F81 TM (Tím/ Crystal switch) F8104", 7 },
                    { 208, 7, "/Assets/img/Products/7/208.jpg", 2490000.0, "Phiên bản màu tím của AULA F68, layout 68 phím nhỏ gọn. Sử dụng Ice Crystal switch (Linear) trong suốt, kết cấu Gasket Mount, kết nối 3 chế độ và keycap PBT bền bỉ.", "Bàn phím AULA F68 TM (Tím/ Ice Crystal switch ) F68V", 9 },
                    { 209, 7, "/Assets/img/Products/7/209.jpg", 1750000.0, "Bàn phím gaming chuyên nghiệp với công nghệ Rapid Trigger, cho phép kích hoạt và reset phím ở bất kỳ điểm nào trên hành trình phím, mang lại lợi thế tốc độ vượt trội trong các game đối kháng.", "Bàn phím cơ DareU EK75 Rapid Trigger Black", 5 },
                    { 210, 8, "/Assets/img/Products/8/210.jpg", 24490000.0, "Nguồn máy tính cao cấp ASUS ROG THOR, công suất 1600W, chuẩn Titanium.", "Nguồn ASUS ROG THOR 1600T3", 25 },
                    { 211, 8, "/Assets/img/Products/8/211.jpg", 13990000.0, "Nguồn ASUS ROG THOR P3, công suất 1200W, chuẩn Platinum.", "Nguồn Máy Tính ASUS ROG THOR 1200 P3 1200W", 15 },
                    { 212, 8, "/Assets/img/Products/8/212.jpg", 7890000.0, "Nguồn Cooler Master V series, hiệu suất cao và ổn định.", "Nguồn máy tính Cooler Master V", 18 },
                    { 213, 8, "/Assets/img/Products/8/213.jpg", 13990000.0, "Nguồn MSI MEG AI series, 1600W, chuẩn Titanium, hỗ trợ PCIe 5.0.", "Nguồn máy tính MSI MEG AI1600T PCIE5", 7 },
                    { 214, 8, "/Assets/img/Products/8/214.jpg", 5490000.0, "Nguồn Cooler Master MWE Gold V2, 1250W, chuẩn 80 Plus Gold, tương thích ATX 3.1.", "Nguồn máy tính Cooler Master MWE GOLD 1250W V2 ATX3.1", 22 },
                    { 215, 8, "/Assets/img/Products/8/215.jpg", 4490000.0, "Nguồn Cooler Master MWE Gold V2, 1050W, chuẩn 80 Plus Gold, tương thích ATX 3.1.", "Nguồn máy tính Cooler Master MWE GOLD 1050W V2 ATX3.1", 28 },
                    { 216, 8, "/Assets/img/Products/8/216.jpg", 1390000.0, "Nguồn Cooler Master MWE 650W, hiệu suất ổn định cho hệ thống tầm trung.", "Nguồn máy tính Cooler Master MWE 650", 30 },
                    { 217, 8, "/Assets/img/Products/8/217.jpg", 6150000.0, "Nguồn Corsair RM1200x SHIFT, 1200W, màu trắng, ATX 3.1, thiết kế cắm cạnh hông.", "Nguồn máy tính Corsair RM1200x SHIFT White ATX 3.1", 11 },
                    { 218, 8, "/Assets/img/Products/8/218.jpg", 2990000.0, "Nguồn Cooler Master MWE Gold V2, 850W, chuẩn 80 Plus Gold, hỗ trợ ATX 3.1.", "Nguồn máy tính Cooler Master MWE GOLD 850W V2 ATX3.1", 19 },
                    { 219, 8, "/Assets/img/Products/8/219.jpg", 790000.0, "Nguồn MSI MAG A500N-H, công suất 500W, trang bị Active PFC.", "Nguồn máy tính MSI MAG A500N-H - Active PFC (500W)", 26 },
                    { 220, 8, "/Assets/img/Products/8/220.jpg", 1690000.0, "Nguồn Cooler Master MWE 750W, lựa chọn phổ thông đáng tin cậy.", "Nguồn máy tính Cooler Master MWE 750", 14 },
                    { 221, 8, "/Assets/img/Products/8/221.jpg", 8990000.0, "Nguồn ASUS ROG Thor P2, công suất 1000W, chuẩn Platinum, hiệu suất cao.", "Nguồn máy tính ASUS ROG Thor 1000P2", 9 },
                    { 222, 8, "/Assets/img/Products/8/222.jpg", 16990000.0, "Nguồn ASUS ROG Thor 1600T, 1600W, chuẩn Titanium, tối ưu cho gaming.", "Nguồn máy tính ASUS ROG Thor 1600T GAMING", 6 },
                    { 223, 8, "/Assets/img/Products/8/223.jpg", 4690000.0, "Nguồn Lian Li EDGE 1000W màu đen, thiết kế đầu cắm L-Shape độc đáo.", "Nguồn máy tính Lian Li EDGE 1000W Black L-Shape", 17 },
                    { 224, 8, "/Assets/img/Products/8/224.jpg", 4990000.0, "Nguồn Lian Li EDGE 1000W màu trắng, thiết kế đầu cắm L-Shape tiện lợi.", "Nguồn máy tính Lian Li EDGE 1000W White L-Shape", 23 },
                    { 225, 8, "/Assets/img/Products/8/225.jpg", 2790000.0, "Nguồn Thermaltake TOUGHPOWER GT 850W, phiên bản Snow màu trắng.", "Nguồn máy tính Thermaltake TOUGHPOWER GT 850W SNOW ", 20 },
                    { 226, 8, "/Assets/img/Products/8/226.jpg", 5990000.0, "Nguồn Lian Li EDGE 1300W màu trắng, công suất lớn, thiết kế L-Shape.", "Nguồn máy tính Lian Li EDGE 1300W White L-Shape", 12 },
                    { 227, 8, "/Assets/img/Products/8/227.jpg", 2690000.0, "Nguồn Thermaltake TOUGHPOWER GT, công suất 850W, hiệu suất cao.", "Nguồn máy tính Thermaltake TOUGHPOWER GT 850W", 29 },
                    { 228, 8, "/Assets/img/Products/8/228.jpg", 5490000.0, "Nguồn Lian Li EDGE 1300W màu đen, công suất lớn, thiết kế L-Shape.", "guồn máy tính Lian Li EDGE 1300W Black L-Shape", 8 },
                    { 229, 8, "/Assets/img/Products/8/229.jpg", 6490000.0, "Nguồn GIGABYTE AORUS ELITE, 1000W, hỗ trợ chuẩn PCIe 5.0.", "Nguồn máy tính GIGABYTE AORUS ELITE P1000W PCIe 5.0", 16 },
                    { 230, 8, "/Assets/img/Products/8/230.jpg", 1290000.0, "Nguồn GIGABYTE P650SS, 650W, phiên bản ICE màu trắng.", "Nguồn máy tính GIGABYTE P650SS ICE ", 24 },
                    { 231, 8, "/Assets/img/Products/8/231.jpg", 990000.0, "Nguồn GIGABYTE P550SS, công suất 550W, phù hợp cho PC cơ bản.", "Nguồn máy tính GIGABYTE P550SS", 27 },
                    { 232, 8, "/Assets/img/Products/8/232.jpg", 6590000.0, "Nguồn GIGABYTE AORUS ELITE 1000W, bản ICE màu trắng, hỗ trợ PCIe 5.0.", "Nguồn máy tính GIGABYTE AORUS ELITE P1000W PCIe 5.0 ICE", 13 },
                    { 233, 8, "/Assets/img/Products/8/233.jpg", 5490000.0, "Nguồn MSI MAG A1250GL, 1250W, chuẩn 80 Plus Gold, hỗ trợ PCIe 5.0.", "Nguồn máy tính MSI MAG A1250GL PCIE5", 10 },
                    { 234, 8, "/Assets/img/Products/8/234.jpg", 2890000.0, "Nguồn Deepcool PN850M, 850W, chuẩn 80 Plus Gold, tương thích ATX 3.1.", "Nguồn máy tính Deepcool PN850M - 80 Plus Gold - ATX 3.1", 21 },
                    { 235, 8, "/Assets/img/Products/8/235.jpg", 3490000.0, "Nguồn ASUS Prime 850W, chuẩn Gold, thiết kế đồng bộ với bo mạch chủ Prime.", "Nguồn máy tính ASUS Prime 850W", 18 },
                    { 236, 8, "/Assets/img/Products/8/236.jpg", 7590000.0, "Nguồn Corsair HX1200i, 1200W, chuẩn 80 Plus Platinum, có giám sát phần mềm.", "Nguồn máy tính Corsair HX1200i - 80 Plus Platinum", 5 },
                    { 237, 8, "/Assets/img/Products/8/237.jpg", 2990000.0, "Nguồn GIGABYTE UD850GM, 850W, chuẩn Gold, hỗ trợ PCIe 5.0 (PG5).", "Nguồn máy tính GIGABYTE UD850GM PG5", 25 },
                    { 238, 8, "/Assets/img/Products/8/238.jpg", 6990000.0, "Nguồn ASUS ROG Strix 1000W, phiên bản màu trắng, hỗ trợ AURA Sync RGB.", "Nguồn máy tính ASUS ROG Strix 1000W AURA White Edition", 12 },
                    { 239, 8, "/Assets/img/Products/8/239.jpg", 2690000.0, "Nguồn Thermaltake TOUGHPOWER GF A3, 750W, chuẩn Gold, tương thích ATX 3.0.", "Nguồn máy tính Thermaltake TOUGHPOWER GF A3 750W", 23 },
                    { 240, 9, "/Assets/img/Products/9/240.jpg", 2290000.0, "Bo mạch chủ ASUS H610M-F WIFI DDR4 hỗ trợ RAM DDR4, kết nối Wi-Fi ổn định, hiệu năng bền bỉ.", "Bo mạch chủ ASUS H610M-F WIFI DDR4", 25 },
                    { 241, 9, "/Assets/img/Products/9/241.jpg", 31990000.0, "Bo mạch chủ cao cấp ASUS ROG CROSSHAIR X870E EXTREME hỗ trợ DDR5, hiệu suất mạnh mẽ cho game thủ và nhà sáng tạo.", "Bo mạch chủ ASUS ROG CROSSHAIR X870E EXTREME (DDR5)", 10 },
                    { 242, 9, "/Assets/img/Products/9/242.jpg", 20290000.0, "Bo mạch chủ ASUS ROG MAXIMUS Z890 APEX DDR5 mang lại hiệu năng ép xung và độ ổn định vượt trội.", "Bo mạch chủ ASUS ROG MAXIMUS Z890 APEX (DDR5)", 15 },
                    { 243, 9, "/Assets/img/Products/9/243.jpg", 9690000.0, "Bo mạch chủ ASUS ROG Strix B850-F Gaming Wifi thiết kế cho game thủ, hỗ trợ Wi-Fi tốc độ cao.", "Bo mạch chủ ASUS ROG Strix B850-F Gaming Wifi", 20 },
                    { 244, 9, "/Assets/img/Products/9/244.jpg", 2990000.0, "Bo mạch chủ ASUS B760M-AYW WIFI D4 hiệu năng ổn định, phù hợp cho máy tính văn phòng và gaming tầm trung.", "Bo mạch chủ ASUS B760M-AYW WIFI D4", 28 },
                    { 245, 9, "/Assets/img/Products/9/245.jpg", 28990000.0, "Bo mạch chủ ASUS ROG MAXIMUS Z890 EXTREME DDR5 đẳng cấp, hỗ trợ đa GPU, ép xung mạnh mẽ.", "Bo mạch chủ ASUS ROG MAXIMUS Z890 EXTREME (DDR5)", 8 },
                    { 246, 9, "/Assets/img/Products/9/246.jpg", 3690000.0, "Bo mạch chủ GIGABYTE B760M GAMING WIFI PLUS DDR5 mang lại trải nghiệm chơi game mượt mà, giá tốt.", "Bo mạch chủ GIGABYTE B760M GAMING WIFI PLUS DDR5", 30 },
                    { 247, 9, "/Assets/img/Products/9/247.jpg", 7990000.0, "Bo mạch chủ MSI PRO Z890-S WIFI DDR5 tối ưu cho công việc và giải trí, hỗ trợ Wi-Fi tốc độ cao.", "Bo mạch chủ MSI PRO Z890-S WIFI (DDR5)", 22 },
                    { 248, 9, "/Assets/img/Products/9/248.jpg", 12290000.0, "Bo mạch chủ GIGABYTE X870E AORUS PRO ICE DDR5 thiết kế cao cấp, tản nhiệt hiệu quả, hiệu năng mạnh.", "Bo mạch chủ GIGABYTE X870E AORUS PRO ICE (DDR5)", 12 },
                    { 249, 9, "/Assets/img/Products/9/249.jpg", 34990000.0, "Bo mạch chủ ASUS Pro WS W790E-SAGE SE dành cho workstation, hỗ trợ nhiều GPU, hiệu năng cực cao.", "Bo mạch chủ ASUS Pro WS W790E-SAGE SE", 5 },
                    { 250, 9, "/Assets/img/Products/9/250.jpg", 3990000.0, "Bo mạch chủ MSI B760M GAMING PLUS WIFI DDR5 thiết kế mạnh mẽ, hỗ trợ Wi-Fi và RAM DDR5.", "Bo mạch chủ MSI B760M GAMING PLUS WIFI DDR5", 26 },
                    { 251, 9, "/Assets/img/Products/9/251.jpg", 1890000.0, "Bo mạch chủ GIGABYTE H610M-H V3 DDR4 giá rẻ, phù hợp máy tính phổ thông.", "Bo mạch chủ GIGABYTE H610M-H V3 (DDR4)", 35 },
                    { 252, 9, "/Assets/img/Products/9/252.jpg", 14990000.0, "Bo mạch chủ ASUS ProArt Z890-CREATOR WIFI DDR5 dành cho nhà sáng tạo nội dung, hỗ trợ Wi-Fi nhanh.", "Bo mạch chủ ASUS ProArt Z890-CREATOR WIFI (DDR5)", 14 },
                    { 253, 9, "/Assets/img/Products/9/253.jpg", 19990000.0, "Bo mạch chủ ASUS ROG MAXIMUS Z890 HERO DDR5 cao cấp, dành cho game thủ chuyên nghiệp.", "Bo mạch chủ ASUS ROG MAXIMUS Z890 HERO (DDR5)", 10 },
                    { 254, 9, "/Assets/img/Products/9/254.jpg", 15790000.0, "Bo mạch chủ ASUS ROG Strix Z890-E GAMING WIFI DDR5 hiệu năng cao, thiết kế đậm chất gaming.", "Bo mạch chủ ASUS ROG Strix Z890-E GAMING WIFI (DDR5)", 16 },
                    { 255, 9, "/Assets/img/Products/9/255.jpg", 13990000.0, "Bo mạch chủ ASUS ROG Strix Z890-F GAMING WIFI DDR5, hỗ trợ ép xung và Wi-Fi tốc độ cao.", "Bo mạch chủ ASUS ROG Strix Z890-F GAMING WIFI (DDR5)", 18 },
                    { 256, 9, "/Assets/img/Products/9/256.jpg", 13490000.0, "Bo mạch chủ ASUS ROG Strix Z890-A GAMING WIFI DDR5 với thiết kế tinh tế, hiệu năng mạnh mẽ.", "Bo mạch chủ ASUS ROG Strix Z890-A GAMING WIFI (DDR5)", 20 },
                    { 257, 9, "/Assets/img/Products/9/257.jpg", 14990000.0, "Bo mạch chủ mini ITX ASUS ROG Strix Z890-I GAMING WIFI DDR5 nhỏ gọn, mạnh mẽ.", "Bo mạch chủ ASUS ROG Strix Z890-I GAMING WIFI (DDR5)", 12 },
                    { 258, 9, "/Assets/img/Products/9/258.jpg", 10490000.0, "Bo mạch chủ ASUS TUF Gaming Z890-PRO WIFI DDR5 bền bỉ, thiết kế tối ưu cho game thủ.", "Bo mạch chủ ASUS TUF Gaming Z890-PRO WIFI (DDR5)", 22 },
                    { 259, 9, "/Assets/img/Products/9/259.jpg", 9990000.0, "Bo mạch chủ ASUS TUF Gaming Z890-PLUS WIFI DDR5 độ bền cao, hiệu suất ổn định.", "Bo mạch chủ ASUS TUF Gaming Z890-PLUS WIFI (DDR5)", 23 },
                    { 260, 9, "/Assets/img/Products/9/260.jpg", 8990000.0, "Bo mạch chủ ASUS PRIME Z890-P WIFI CSM DDR5 phù hợp người dùng phổ thông, hỗ trợ Wi-Fi.", "Bo mạch chủ ASUS PRIME Z890-P WIFI CSM (DDR5)", 24 },
                    { 261, 9, "/Assets/img/Products/9/261.jpg", 16990000.0, "Bo mạch chủ MSI MPG Z890 CARBON WIFI DDR5 hiệu năng cao, tản nhiệt tốt, dành cho gamer.", "Bo mạch chủ MSI MPG Z890 CARBON WIFI (DDR5)", 13 },
                    { 262, 9, "/Assets/img/Products/9/262.jpg", 27990000.0, "Bo mạch chủ Asrock Z890 Taichi AQUA DDR5 cao cấp, hỗ trợ tản nước và hiệu năng mạnh mẽ.", "Bo mạch chủ Asrock Z890 Taichi AQUA (DDR5)", 9 },
                    { 263, 9, "/Assets/img/Products/9/263.jpg", 6990000.0, "Bo mạch chủ GIGABYTE Z890 UD WIFI6E DDR5 ổn định, hỗ trợ Wi-Fi 6E tốc độ cao.", "Bo mạch chủ GIGABYTE Z890 UD WIFI6E (DDR5)", 27 },
                    { 264, 9, "/Assets/img/Products/9/264.jpg", 9490000.0, "Bo mạch chủ GIGABYTE X870 AORUS ELITE WIFI7 DDR5, hỗ trợ Wi-Fi 7 cực nhanh.", "Bo mạch chủ GIGABYTE X870 AORUS ELITE WIFI7 (DDR5)", 19 },
                    { 265, 9, "/Assets/img/Products/9/265.jpg", 1990000.0, "Bo mạch chủ Gigabyte B450M DS3H V3 giá rẻ, ổn định, phù hợp máy tính phổ thông.", "Bo mạch chủ Gigabyte B450M DS3H V3", 34 },
                    { 266, 9, "/Assets/img/Products/9/266.jpg", 1690000.0, "Bo mạch chủ MSI B450M-A PRO MAX II hiệu năng tốt, giá rẻ cho hệ thống AMD.", "Bo mạch chủ MSI B450M-A PRO MAX II", 36 },
                    { 267, 9, "/Assets/img/Products/9/267.jpg", 8390000.0, "Bo mạch chủ MSI PRO Z890-P WIFI DDR5 thiết kế tinh gọn, ổn định cho công việc và gaming.", "Bo mạch chủ MSI PRO Z890-P WIFI (DDR5)", 21 },
                    { 268, 9, "/Assets/img/Products/9/268.jpg", 9490000.0, "Bo mạch chủ MSI PRO Z890-A WIFI DDR5 hỗ trợ kết nối mạnh mẽ và hiệu suất cao.", "Bo mạch chủ MSI PRO Z890-A WIFI (DDR5)", 22 },
                    { 269, 9, "/Assets/img/Products/9/269.jpg", 10490000.0, "Bo mạch chủ MSI MAG Z890 TOMAHAWK WIFI DDR5 bền bỉ, hiệu năng mạnh cho gaming.", "Bo mạch chủ MSI MAG Z890 TOMAHAWK WIFI (DDR5)", 20 },
                    { 270, 10, "/Assets/img/Products/10/270.jpg", 2590000.0, "Ghế công thái học WEC502 màu xám, thiết kế hiện đại, hỗ trợ cột sống tối ưu.", "Ghế Ergonomic Warrior Hero series WEC502 Grey", 15 },
                    { 271, 10, "/Assets/img/Products/10/271.jpg", 2790000.0, "Ghế E-Dra EEC225 với thiết kế công thái học, phù hợp cho người làm việc văn phòng.", "Ghế công thái học E-Dra EEC225", 22 },
                    { 272, 10, "/Assets/img/Products/10/272.jpg", 3590000.0, "Ghế gaming E-Dra EGC231 WISDOM màu đen, mang lại cảm giác ngồi thoải mái khi chơi game.", "Ghế game E-Dra EGC231 WISDOM Black", 8 },
                    { 273, 10, "/Assets/img/Products/10/273.jpg", 2590000.0, "Ghế công thái học WEC502 màu đen, kiểu dáng mạnh mẽ, bền bỉ theo thời gian.", "Ghế Ergonomic Warrior Hero series WEC502 Black", 30 },
                    { 274, 10, "/Assets/img/Products/10/274.jpg", 2990000.0, "Phiên bản nâng cấp WEC501 Plus màu đen, tăng cường sự thoải mái cho người dùng.", "Ghế công thái học WARRIOR Hero Series WEC501 Plus Black", 12 },
                    { 275, 10, "/Assets/img/Products/10/275.jpg", 2990000.0, "Ghế Sihoo M102C, lựa chọn phổ biến cho không gian làm việc chuyên nghiệp.", "Ghế công thái học Sihoo M102C", 25 },
                    { 276, 10, "/Assets/img/Products/10/276.jpg", 3790000.0, "Phiên bản V2.0 của ghế WEC506 màu đen, cải tiến về chất liệu và tính năng.", "Ghế Ergonomic Warrior Hero series WEC506 Black V2.0", 18 },
                    { 277, 10, "/Assets/img/Products/10/277.jpg", 3290000.0, "Ghế gaming WGC225 thuộc dòng Immortal Series, dành cho game thủ chuyên nghiệp.", "Ghế chơi game Warrior lmmortal Series WGC225 Black", 11 },
                    { 278, 10, "/Assets/img/Products/10/278.jpg", 2690000.0, "Ghế gaming WGC207 màu đen, thiết kế hầm hố, giá cả phải chăng.", "Ghế chơi game Warrior Raider Series WGC207 Black", 33 },
                    { 279, 10, "/Assets/img/Products/10/279.jpg", 2690000.0, "Ghế E-Dra EGC229 phối màu đen đỏ, phong cách thể thao và cá tính.", "Ghế game E-Dra Level E - EGC229 Black Red", 24 },
                    { 280, 10, "/Assets/img/Products/10/280.jpg", 4590000.0, "Ghế Sihoo M57B tích hợp kê chân tiện lợi, giúp thư giãn tối đa.", "Ghế công thái học Sihoo M57B (có kê chân)", 9 },
                    { 281, 10, "/Assets/img/Products/10/281.jpg", 3690000.0, "Ghế gaming E-Dra EGC235 màu đen, đệm dày và êm ái.", "Ghế Gaming E-Dra EGC235 - Throw (Black)", 14 },
                    { 282, 10, "/Assets/img/Products/10/282.jpg", 1890000.0, "Ghế E-Dra Citizen EGC236, lựa chọn tuyệt vời cho game thủ với ngân sách hạn hẹp.", "Ghế Gaming E-Dra Citizen EGC236", 35 },
                    { 283, 10, "/Assets/img/Products/10/283.jpg", 3390000.0, "Ghế công thái học WEC509 phối màu đen/đỏ, tạo điểm nhấn cho không gian.", "Ghế Ergonomic Warrior Hero series WEC509 Black/Red", 19 },
                    { 284, 10, "/Assets/img/Products/10/284.jpg", 1590000.0, "Ghế WEC105 thuộc dòng Pawn Series, giá tốt, chất lượng đảm bảo.", "Ghế công thái học Ergonomic Warrior Pawn Series WEC105 Black", 28 },
                    { 285, 10, "/Assets/img/Products/10/285.jpg", 2690000.0, "Ghế E-Dra EGC229 phối màu đen xám, lịch lãm và hiện đại.", "Ghế game E-Dra Level E - EGC229 Black Grey", 17 },
                    { 286, 10, "/Assets/img/Products/10/286.jpg", 3690000.0, "Phiên bản V2.0 của ghế WEC506 màu đen/xám, sang trọng và thoải mái.", "Ghế Ergonomic Warrior Hero series WEC506 Black/Gray V2.0", 21 },
                    { 287, 10, "/Assets/img/Products/10/287.jpg", 11400000.0, "Ghế gaming cao cấp Corsair TC500 LUXE phiên bản Sherwood, chất liệu sang trọng.", "Ghế Gaming Corsair TC500 LUXE - Sherwood", 7 },
                    { 288, 10, "/Assets/img/Products/10/288.jpg", 2690000.0, "Ghế WGC206 phối màu trắng/hồng, phù hợp với các bạn nữ game thủ.", "Ghế Warrior Raider Series WGC206 White/Pink", 13 },
                    { 289, 10, "/Assets/img/Products/10/289.jpg", 7490000.0, "Ghế gaming Razer Iskur V2 X bọc vải, thoáng khí và bền bỉ.", "Ghế gaming Razer Iskur V2 X Fabric - Black Grey (RZ38-05310100-R3UA)", 6 },
                    { 290, 10, "/Assets/img/Products/10/290.jpg", 1990000.0, "Ghế gaming WGC203 màu đen/đỏ, sản phẩm bán chạy trong phân khúc.", "Ghế Warrior Raider Series WGC203 Black/Red", 31 },
                    { 291, 10, "/Assets/img/Products/10/291.jpg", 3990000.0, "Ghế công thái học WEC509 màu đen/xám, thiết kế tinh tế và chắc chắn.", "Ghế Ergonomic Warrior Hero series WEC509 Black/Gray", 16 },
                    { 292, 10, "/Assets/img/Products/10/292.jpg", 3990000.0, "Ghế công thái học WEC509 phiên bản full đen, đơn giản và sang trọng.", "Ghế Ergonomic Warrior Hero series WEC509 Black", 20 },
                    { 293, 10, "/Assets/img/Products/10/293.jpg", 6000000.0, "Ghế gaming E-Dra EGC2025 phiên bản cao cấp LUX FORTUNE.", "Ghế gaming E-Dra Champion EGC2025 LUX FORTUNE", 10 },
                    { 294, 10, "/Assets/img/Products/10/294.jpg", 11400000.0, "Ghế gaming cao cấp Corsair TC500 LUXE phiên bản Frost, màu sắc tinh khôi.", "Ghế Gaming Corsair TC500 LUXE - Frost", 5 },
                    { 295, 10, "/Assets/img/Products/10/295.jpg", 4090000.0, "Ghế Sihoo M57, mẫu ghế công thái học được ưa chuộng nhất của Sihoo.", "Ghế công thái học Sihoo M57", 29 },
                    { 296, 10, "/Assets/img/Products/10/296.jpg", 7100000.0, "Ghế Corsair T3 RUSH 2023 màu xám/trắng, bọc vải mềm mại.", "Ghế Corsair T3 RUSH Gray/White 2023 (CF-9010058-WW)", 9 },
                    { 297, 10, "/Assets/img/Products/10/297.jpg", 7100000.0, "Ghế Corsair T3 RUSH 2023 màu than, mạnh mẽ và nam tính.", "Ghế Corsair T3 RUSH Charcoal 2023", 14 },
                    { 298, 10, "/Assets/img/Products/10/298.jpg", 4090000.0, "Ghế Sihoo M57 phiên bản màu đen xám, dễ dàng phối hợp với mọi không gian.", "Ghế công thái học Sihoo M57-BLACK GRAY", 26 },
                    { 299, 10, "/Assets/img/Products/10/299.jpg", 14990000.0, "Ghế gaming Razer Iskur V2 phiên bản cao cấp nhất, full đen, hỗ trợ lưng tuyệt đối.", "Ghế gaming Razer Iskur V2 - Full Black", 7 },
                    { 300, 10, "/Assets/img/Products/10/300.jpg", 11400000.0, "Ghế gaming cao cấp Corsair TC500 LUXE phiên bản Shadow, thiết kế bí ẩn.", "Ghế Gaming Corsair TC500 LUXE - Shadow", 6 },
                    { 301, 11, "/Assets/img/Products/11/301.jpg", 2390000.0, "CPU Intel Pentium Gold G7400 hiệu năng ổn định, phù hợp cho học tập và văn phòng.", "Bộ vi xử lý Intel Pentium Gold G7400 / 3.7 GHz / 2 nhân 4 luồng / 6MB / LGA 1700", 34 },
                    { 302, 11, "/Assets/img/Products/11/302.jpg", 1990000.0, "CPU Intel Pentium Gold G6405 tốc độ cao, tối ưu cho các hệ thống cơ bản.", "Bộ vi xử lý Intel Pentium Gold G6405 / 4.1 GHz / 2 nhân 4 luồng / 4MB / LGA 1200", 17 },
                    { 303, 11, "/Assets/img/Products/11/303.jpg", 17490000.0, "CPU Intel Core Ultra 9 285K mạnh mẽ cho gaming và đồ họa chuyên nghiệp.", "Bộ vi xử lý Intel Core Ultra 9 285K / Turbo up to 5.7GHz / 24 Nhân 24 Luồng / 36MB / LGA 1851", 48 },
                    { 304, 11, "/Assets/img/Products/11/304.jpg", 16990000.0, "CPU Intel Core Ultra 9 285 hiệu suất cao, hỗ trợ socket LGA 1851 thế hệ mới.", "Bộ vi xử lý Intel Core Ultra 9 285 / Turbo up to 5.6GHz / 24 Nhân 24 Luồng / 36MB / LGA 1851", 23 },
                    { 305, 11, "/Assets/img/Products/11/305.jpg", 8990000.0, "CPU Intel Core Ultra 7 265KF tối ưu cho hiệu năng đa nhiệm và chơi game.", "Bộ vi xử lý Intel Core Ultra 7 265KF / Turbo up to 5.5GHz / 20 Nhân 20 Luồng / 30MB / LGA 1851", 31 },
                    { 306, 11, "/Assets/img/Products/11/306.jpg", 11790000.0, "CPU Intel Core Ultra 7 265K mạnh mẽ, hiệu suất vượt trội cho người dùng cao cấp.", "Bộ vi xử lý Intel Core Ultra 7 265K / Turbo up to 5.5GHz / 20 Nhân 20 Luồng / 30MB / LGA 1851", 44 },
                    { 307, 11, "/Assets/img/Products/11/307.jpg", 10490000.0, "CPU Intel Core Ultra 7 265F tiết kiệm điện, phù hợp cho công việc và giải trí.", "Bộ vi xử lý Intel Core Ultra 7 265F / Turbo up to 5.3GHz / 20 Nhân 20 Luồng / 30MB / LGA 1851", 15 },
                    { 308, 11, "/Assets/img/Products/11/308.jpg", 10990000.0, "CPU Intel Core Ultra 7 265 hiệu năng ổn định, hỗ trợ socket LGA 1851 mới nhất.", "Bộ vi xử lý Intel Core Ultra 7 265 / Turbo up to 5.3GHz / 20 Nhân 20 Luồng / 30MB / LGA 1851", 22 },
                    { 309, 11, "/Assets/img/Products/11/309.jpg", 8290000.0, "CPU Intel Core Ultra 5 245KF mang lại hiệu suất tuyệt vời cho game và công việc.", "Bộ vi xử lý Intel Core Ultra 5 245KF / Turbo up to 5.2GHz / 14 Nhân 14 Luồng / 24MB / LGA 1851", 35 },
                    { 310, 11, "/Assets/img/Products/11/310.jpg", 8690000.0, "CPU Intel Core Ultra 5 245K cân bằng giữa hiệu năng và giá thành.", "Bộ vi xử lý Intel Core Ultra 5 245K / Turbo up to 5.2GHz / 14 Nhân 14 Luồng / 24MB / LGA 1851", 19 },
                    { 311, 11, "/Assets/img/Products/11/311.jpg", 8490000.0, "CPU Intel Core Ultra 5 245 dành cho người dùng tầm trung với hiệu năng ổn định.", "Bộ vi xử lý Intel Core Ultra 5 245 / Turbo up to 5.1GHz / 14 Nhân 14 Luồng / 24MB / LGA 1851", 49 },
                    { 312, 11, "/Assets/img/Products/11/312.jpg", 7990000.0, "CPU Intel Core Ultra 5 235 hiệu năng cao cho hệ thống desktop hiện đại.", "Bộ vi xử lý Intel Core Ultra 5 235 / Turbo up to 5.0 GHz / 14 Nhân 14 Luồng / 24MB / LGA 1851", 29 },
                    { 313, 11, "/Assets/img/Products/11/313.jpg", 6990000.0, "CPU Intel Core Ultra 5 225F phù hợp cho làm việc văn phòng và game nhẹ.", "Bộ vi xử lý Intel Core Ultra 5 225F / Turbo up to 4.9 GHz / 10 Nhân 10 Luồng / 20MB / LGA 1851", 40 },
                    { 314, 11, "/Assets/img/Products/11/314.jpg", 7490000.0, "CPU Intel Core Ultra 5 225 mang lại hiệu năng tốt trong tầm giá hợp lý.", "Bộ vi xử lý Intel Core Ultra 5 225 / Turbo up to 4.9 GHz / 10 Nhân 10 Luồng / 20MB / LGA 1851", 25 },
                    { 315, 11, "/Assets/img/Products/11/315.jpg", 13190000.0, "CPU Intel Core i9 14900KF – hiệu năng cực đỉnh cho game thủ và dân sáng tạo.", "Bộ vi xử lý Intel Core i9 14900KF / Turbo up to 6.0GHz / 24 Nhân 32 Luồng / 36MB / LGA 1700", 44 },
                    { 316, 11, "/Assets/img/Products/11/316.jpg", 13690000.0, "CPU Intel Core i9 14900K dành cho hiệu suất tối đa trong mọi tác vụ nặng.", "Bộ vi xử lý Intel Core i9 14900K / Turbo up to 6.0GHz / 24 Nhân 32 Luồng / 36MB / LGA 1700", 21 },
                    { 317, 11, "/Assets/img/Products/11/317.jpg", 14990000.0, "CPU Intel Core i9 14900F mạnh mẽ, đáp ứng nhu cầu cao cấp của người dùng.", "Bộ vi xử lý Intel Core i9 14900F / Turbo up to 5.8GHz / 24 Nhân 32 Luồng / 36MB / LGA 1700", 37 },
                    { 318, 11, "/Assets/img/Products/11/318.jpg", 15690000.0, "CPU Intel Core i9 14900 tối ưu cho làm việc chuyên nghiệp và gaming cao cấp.", "Bộ vi xử lý Intel Core i9 14900 / Turbo up to 5.8GHz / 24 Nhân 32 Luồng / 36MB / LGA 1700", 33 },
                    { 319, 11, "/Assets/img/Products/11/319.jpg", 9490000.0, "CPU Intel Core i7 14700KF hiệu suất mạnh, hỗ trợ ép xung tối ưu.", "Bộ vi xử lý Intel Core i7 14700KF / Turbo up to 5.6GHz / 20 Nhân 28 Luồng / 33MB / LGA 1700", 18 },
                    { 320, 11, "/Assets/img/Products/11/320.jpg", 10690000.0, "CPU Intel Core i7 14700K – lựa chọn lý tưởng cho dân sáng tạo và streamer.", "Bộ vi xử lý Intel Core i7 14700K / Turbo up to 5.6GHz / 20 Nhân 28 Luồng / 33MB / LGA 1700", 49 },
                    { 321, 11, "/Assets/img/Products/11/321.jpg", 8490000.0, "CPU Intel Core i7 14700F – lựa chọn tối ưu cho hiệu năng và tiết kiệm chi phí.", "Bộ vi xử lý Intel Core i7 14700F / Turbo up to 5.4GHz / 20 Nhân 28 Luồng / 33MB / LGA 1700", 15 },
                    { 322, 11, "/Assets/img/Products/11/322.jpg", 9290000.0, "CPU Intel Core i7 14700 xử lý đa nhiệm nhanh, phù hợp cho mọi nhu cầu.", "Bộ vi xử lý Intel Core i7 14700 / Turbo up to 5.4GHz / 20 Nhân 28 Luồng / 33MB / LGA 1700", 38 },
                    { 323, 11, "/Assets/img/Products/11/323.jpg", 8490000.0, "CPU Intel Core i7 13700F – lựa chọn mạnh mẽ cho hệ thống gaming tầm cao.", "Bộ vi xử lý Intel Core i7 13700F / 2.1GHz Turbo 5.2GHz / 16 Nhân 24 Luồng / 30MB / LGA 1700", 28 },
                    { 324, 11, "/Assets/img/Products/11/324.jpg", 10490000.0, "CPU Intel Core i7 13700 – hiệu năng ổn định, hỗ trợ socket LGA 1700.", "Bộ vi xử lý Intel Core i7 13700 / 2.1GHz Turbo 5.2GHz / 16 Nhân 24 Luồng / 30MB / LGA 1700", 45 },
                    { 325, 11, "/Assets/img/Products/11/325.jpg", 8990000.0, "CPU Intel Core i7 12700KF hỗ trợ ép xung, hiệu năng mạnh mẽ cho game thủ.", "Bộ vi xử lý Intel Core i7 12700KF / 3.6GHz Turbo 5.0GHz / 12 Nhân 20 Luồng / 25MB / LGA 1700", 14 },
                    { 326, 11, "/Assets/img/Products/11/326.jpg", 7890000.0, "CPU Intel Core i7 12700K – lựa chọn tối ưu giữa giá và hiệu năng.", "Bộ vi xử lý Intel Core i7 12700K / 3.6GHz Turbo 5.0GHz / 12 Nhân 20 Luồng / 25MB / LGA 1700", 32 },
                    { 327, 11, "/Assets/img/Products/11/327.jpg", 7890000.0, "CPU Intel Core i7 12700F tiết kiệm điện năng nhưng vẫn mạnh mẽ.", "Bộ vi xử lý Intel Core i7 12700F / 2.1GHz Turbo 4.9GHz / 12 Nhân 20 Luồng / 25MB / LGA 1700", 47 },
                    { 328, 11, "/Assets/img/Products/11/328.jpg", 8490000.0, "CPU Intel Core i7 12700 – hiệu năng bền bỉ, phù hợp đa tác vụ.", "Bộ vi xử lý Intel Core i7 12700 / 2.1GHz Turbo 4.9GHz / 12 Nhân 20 Luồng / 25MB / LGA 1700", 20 },
                    { 329, 11, "/Assets/img/Products/11/329.jpg", 6190000.0, "CPU Intel Core i5 14600KF – hiệu năng mạnh, giá hợp lý cho game thủ.", "Bộ vi xử lý Intel Core i5 14600KF / Turbo up to 5.3GHz / 14 Nhân 20 Luồng / 24MB / LGA 1700", 27 },
                    { 330, 11, "/Assets/img/Products/11/330.jpg", 6190000.0, "CPU Intel Core i5 14600K cân bằng giữa hiệu suất và chi phí.", "Bộ vi xử lý Intel Core i5 14600K / Turbo up to 5.3GHz / 14 Nhân 20 Luồng / 24MB / LGA 1700", 43 },
                    { 331, 12, "/Assets/img/Products/12/331.jpg", 32490000.0, "Card đồ họa cao cấp RTX 5070 Ti mang đến hiệu năng mạnh mẽ và thiết kế trắng sang trọng.", "Card màn hình MSI GeForce RTX 5070 Ti 16G GAMING TRIO OC White", 47 },
                    { 332, 12, "/Assets/img/Products/12/332.jpg", 9690000.0, "Card đồ họa tầm trung RTX 5060 với hệ thống làm mát Windforce hiệu quả.", "Card màn hình Gigabyte GeForce RTX 5060 Windforce OC 8GB", 55 },
                    { 333, 12, "/Assets/img/Products/12/333.jpg", 10490000.0, "RTX 5060 Eagle Ice thiết kế độc đáo, hiệu năng ổn định cho game và đồ họa.", "Card màn hình Gigabyte GeForce RTX 5060 Eagle Ice OC 8GB", 63 },
                    { 334, 12, "/Assets/img/Products/12/334.jpg", 9990000.0, "RTX 5060 bản Windforce 8GB, tối ưu làm mát và hiệu năng cao.", "Card màn hình Gigabyte GeForce RTX 5060 Windforce 8GB", 29 },
                    { 335, 12, "/Assets/img/Products/12/335.jpg", 19490000.0, "RTX 4070 SUPER với tản Windforce, hiệu năng gaming cao cấp.", "Card màn hình GIGABYTE GeForce RTX 4070 SUPER WINDFORCE OC 12G", 73 },
                    { 336, 12, "/Assets/img/Products/12/336.jpg", 38990000.0, "Card RTX 5080 phiên bản trắng, hiệu suất vượt trội và tản nhiệt tối ưu.", "Card màn hình MSI GeForce RTX 5080 16G VENTUS 3X OC White", 60 },
                    { 337, 12, "/Assets/img/Products/12/337.jpg", 19990000.0, "Card Radeon RX 9070 với thiết kế Gaming OC, mạnh mẽ cho mọi nhu cầu.", "Card màn hình GIGABYTE Radeon RX 9070 GAMING OC 16G", 37 },
                    { 338, 12, "/Assets/img/Products/12/338.jpg", 4790000.0, "Card RTX 3050 giá tốt, phù hợp chơi game eSport và đồ họa nhẹ.", "Card màn hình MSI GeForce RTX 3050 VENTUS 2X E 6G OC", 69 },
                    { 339, 12, "/Assets/img/Products/12/339.jpg", 79990000.0, "Siêu card RTX 5090 với 32GB VRAM, hiệu năng đỉnh cao cho 4K gaming.", "Card màn hình MSI GeForce RTX 5090 32G VENTUS 3X OC", 42 },
                    { 340, 12, "/Assets/img/Products/12/340.jpg", 20490000.0, "Phiên bản đặc biệt Wukong của RTX 4070 SUPER, mạnh mẽ và độc đáo.", "Card màn hình MSI GeForce RTX 4070 SUPER 12G GAMING SLIM WUKONG EDITION", 64 },
                    { 341, 12, "/Assets/img/Products/12/341.jpg", 62990000.0, "Siêu card RTX 4090 dòng TUF Gaming, hiệu năng và độ bền hàng đầu.", "Card màn hình ASUS TUF Gaming GeForce RTX 4090 24GB GDDR6X OG OC Edition", 32 },
                    { 342, 12, "/Assets/img/Products/12/342.jpg", 12990000.0, "Card RTX 4060 Ti dòng PRIME, cân bằng giữa hiệu năng và giá trị.", "Card màn hình ASUS PRIME GeForce RTX 4060 Ti 8GB GDDR6 OC Edition", 78 },
                    { 343, 12, "/Assets/img/Products/12/343.jpg", 7790000.0, "RTX 3060 bản Windforce V2, phù hợp chơi game Full HD và đồ họa 3D.", "Card màn hình GIGABYTE GeForce RTX 3060 WINDFORCE OC 12G V2", 59 },
                    { 344, 12, "/Assets/img/Products/12/344.jpg", 3790000.0, "Card đồ họa RX 6500 XT tiết kiệm, đáp ứng tốt nhu cầu cơ bản.", "Card màn hình ASUS Dual Radeon RX 6500 XT V2 OC Edition 4GB", 46 },
                    { 345, 12, "/Assets/img/Products/12/345.jpg", 8490000.0, "RTX 4060 phiên bản Dual OC, thiết kế nhỏ gọn và hiệu năng ổn định.", "Card màn hình ASUS Dual GeForce RTX 4060 V2 OC Edition 8GB", 74 },
                    { 346, 12, "/Assets/img/Products/12/346.jpg", 14990000.0, "Card đồ họa RTX 4060 Ti EVO OC, tối ưu cho hiệu năng gaming và sáng tạo.", "Card màn hình ASUS Dual GeForce RTX 4060 Ti EVO OC Edition 16GB", 57 },
                    { 347, 12, "/Assets/img/Products/12/347.jpg", 5490000.0, "Radeon RX 6600 hiệu quả cho game Full HD, giá thành hợp lý.", "Card màn hình ASUS Dual Radeon RX 6600 V3 8GB GDDR6", 30 },
                    { 348, 12, "/Assets/img/Products/12/348.jpg", 7790000.0, "Card RX 7600 OC phiên bản mới, hiệu năng mạnh và tản tốt.", "Card màn hình ASUS Dual Radeon RX 7600 V2 OC Edition 8GB", 62 },
                    { 349, 12, "/Assets/img/Products/12/349.jpg", 13990000.0, "RTX 4060 Ti bản Ventus 3X, hiệu năng cao, tản êm.", "Card màn hình MSI GeForce RTX 4060 Ti VENTUS 3X E 8G OC", 48 },
                    { 350, 12, "/Assets/img/Products/12/350.jpg", 29490000.0, "RTX 4070 Ti SUPER BTF White – thiết kế cao cấp, hiệu suất cực mạnh.", "Card màn hình ASUS TUF Gaming GeForce RTX 4070 Ti SUPER BTF White OC Edition 16GB", 71 },
                    { 351, 12, "/Assets/img/Products/12/351.jpg", 39990000.0, "AORUS RTX 4080 SUPER XTREME ICE, thiết kế độc đáo và hiệu năng đỉnh cao.", "Card màn hình GIGABYTE AORUS GeForce RTX 4080 SUPER XTREME ICE 16G", 33 },
                    { 352, 12, "/Assets/img/Products/12/352.jpg", 4790000.0, "Card RTX 3050 Windforce OC, hiệu năng ổn định và tiết kiệm điện.", "Card màn hình GIGABYTE GeForce RTX 3050 WINDFORCE OC 6G", 66 },
                    { 353, 12, "/Assets/img/Products/12/353.jpg", 29190000.0, "RTX 4070 Ti SUPER dòng ProArt, chuyên biệt cho đồ họa và sáng tạo.", "Card màn hình ASUS ProArt GeForce RTX 4070 Ti SUPER 16GB GDDR6X OC Edition", 43 },
                    { 354, 12, "/Assets/img/Products/12/354.jpg", 4790000.0, "Card RTX 3050 Dual 6GB, phù hợp cho hệ thống gaming tầm trung.", "Card màn hình ASUS Dual GeForce RTX 3050 6GB GDDR6", 68 },
                    { 355, 12, "/Assets/img/Products/12/355.jpg", 4790000.0, "Card RTX 3050 OC, hiệu năng tốt cho chơi game nhẹ và học tập.", "Card màn hình ASUS Dual GeForce RTX 3050 OC Edition 6GB", 51 },
                    { 356, 12, "/Assets/img/Products/12/356.jpg", 4790000.0, "Card RTX 3050 Ventus 2X 6G OC, thiết kế nhỏ gọn và hiệu quả.", "Card màn hình MSI GeForce RTX 3050 VENTUS 2X 6G OC", 59 },
                    { 357, 12, "/Assets/img/Products/12/357.jpg", 5090000.0, "RTX 3050 Eagle OC, hiệu năng ổn định và thiết kế hiện đại.", "Card màn hình GIGABYTE GeForce RTX 3050 EAGLE OC 6G", 72 },
                    { 358, 12, "/Assets/img/Products/12/358.jpg", 17390000.0, "Card RTX 4070 Ventus 3X E, hiệu năng cao và giá hợp lý.", "Card màn hình MSI GeForce RTX 4070 VENTUS 3X E 12G OC", 41 },
                    { 359, 12, "/Assets/img/Products/12/359.jpg", 27990000.0, "RTX 4070 Ti Gaming OC V2, hiệu năng mạnh mẽ cho game thủ chuyên nghiệp.", "Card màn hình GIGABYTE GeForce RTX 4070 Ti GAMING OC V2 12GB", 36 },
                    { 360, 12, "/Assets/img/Products/12/360.jpg", 19490000.0, "Card RTX 4070 SUPER Ventus 2X, hiệu năng cao trong thiết kế nhỏ gọn.", "Card màn hình MSI GeForce RTX 4070 SUPER 12G VENTUS 2X OC", 67 },
                    { 361, 13, "/Assets/img/Products/13/361.jpg", 490000.0, "RAM Kingston Fury 3200MHz hiệu năng ổn định, phù hợp cho PC gaming tầm trung.", "Ram Kingston Fury 1x8GB 3200 Beast", 22 },
                    { 362, 13, "/Assets/img/Products/13/362.jpg", 590000.0, "RAM PNY XLR8 thiết kế nhỏ gọn, tối ưu cho không gian hẹp.", "Ram PNY XLR8 Low Profile 1x8GB 3200 DDR4", 38 },
                    { 363, 13, "/Assets/img/Products/13/363.jpg", 590000.0, "RAM G.Skill Ripjaws V 2800MHz cho hiệu suất ổn định và độ bền cao.", "RAM G.SKILL Ripjaws V 1x8GB 2800 DDR4", 44 },
                    { 364, 13, "/Assets/img/Products/13/364.jpg", 650000.0, "RAM G.Skill Ripjaws V 3600MHz – tốc độ cao, thiết kế mạnh mẽ.", "Ram G.Skill Ripjaws V 1x8GB 3600", 31 },
                    { 365, 13, "/Assets/img/Products/13/365.jpg", 650000.0, "Corsair Vengeance LPX 8GB – lựa chọn phổ biến cho hiệu suất ổn định.", "Ram Corsair Vengeance LPX 8GB 3200", 15 },
                    { 366, 13, "/Assets/img/Products/13/366.jpg", 650000.0, "RAM G.Skill Ripjaws V 3200MHz tối ưu cho gaming và đồ họa.", "Ram G.Skill Ripjaws V 1x8GB 3200", 40 },
                    { 367, 13, "/Assets/img/Products/13/367.jpg", 650000.0, "RAM PNY XLR8 3200MHz mang lại hiệu năng cao cho người dùng phổ thông.", "RAM PNY XLR8 1x8GB 3200MHz DDR4", 28 },
                    { 368, 13, "/Assets/img/Products/13/368.jpg", 750000.0, "Kingston Fury Beast Black – hiệu năng mạnh, độ tin cậy cao.", "RAM Kingston Fury Beast Black 8GB 3200 DDR4", 42 },
                    { 369, 13, "/Assets/img/Products/13/369.jpg", 790000.0, "RAM PNY XLR8 RGB 3200MHz – tỏa sáng cùng dàn PC RGB của bạn.", "Ram PNY XLR8 1x8GB 3200 RGB", 25 },
                    { 370, 13, "/Assets/img/Products/13/370.jpg", 790000.0, "Phiên bản Silver sang trọng, hiệu năng ổn định, tản nhiệt tốt.", "Ram PNY XLR8 Silver 1x8GB 3200 RGB", 37 },
                    { 371, 13, "/Assets/img/Products/13/371.jpg", 850000.0, "RAM RGB mạnh mẽ, cân bằng giữa hiệu năng và thẩm mỹ.", "RAM Kingston Fury Beast 8GB 3200 DDR4 RGB", 18 },
                    { 372, 13, "/Assets/img/Products/13/372.jpg", 890000.0, "RAM V-Color Skywalker 3600MHz – thiết kế đẹp, hiệu suất cao.", "Ram V-Color Skywalker Plus 1x8GB 3600 RGB Silver", 41 },
                    { 373, 13, "/Assets/img/Products/13/373.jpg", 890000.0, "T-Force Delta White 3600MHz – phong cách và tốc độ trong một.", "Ram T-Group T-Force Delta 1x8GB 3600 RGB White", 34 },
                    { 374, 13, "/Assets/img/Products/13/374.jpg", 890000.0, "Phiên bản SE mạnh mẽ với xung nhịp cao 3600MHz.", "RAM Kingston Fury Beast 8GB 3600 DDR4 RGB SE", 21 },
                    { 375, 13, "/Assets/img/Products/13/375.jpg", 890000.0, "RAM T-Force Vulcan Z màu đỏ – bền bỉ và mạnh mẽ.", "RAM TeamGroup T-Force Vulcan Z Red 8GB 3200", 47 },
                    { 376, 13, "/Assets/img/Products/13/376.jpg", 890000.0, "Corsair Vengeance RS RGB mang đến vẻ đẹp ánh sáng rực rỡ.", "Ram Corsair Vengeance RS RGB 1x8GB 3200", 33 },
                    { 377, 13, "/Assets/img/Products/13/377.jpg", 890000.0, "RAM Kingston Fury Beast 3600MHz – hiệu năng cực cao.", "RAM Kingston Fury Beast 1x8GB 3600 DDR4 RGB", 26 },
                    { 378, 13, "/Assets/img/Products/13/378.jpg", 890000.0, "RAM Delta RGB Black 3600MHz – đậm chất gaming.", "Ram T-Group T-Force Delta 1x8GB 3600 RGB Black", 29 },
                    { 379, 13, "/Assets/img/Products/13/379.jpg", 890000.0, "PNY XLR8 Silver 3600MHz – RGB sang trọng, hiệu năng mượt mà.", "Ram PNY XLR8 Silver 1x8GB 3600 RGB", 36 },
                    { 380, 13, "/Assets/img/Products/13/380.jpg", 950000.0, "RAM Kingston Fury 3600MHz – tốc độ cao cho hệ thống gaming.", "RAM Kingston Fury Beast 1x8GB 3600 DDR4 RGB", 15 },
                    { 381, 13, "/Assets/img/Products/13/381.jpg", 990000.0, "Corsair Vengeance RS RGB – đồng bộ ánh sáng tuyệt đẹp.", "Ram Corsair Vengeance RS RGB 1x8GB 3600", 20 },
                    { 382, 13, "/Assets/img/Products/13/382.jpg", 990000.0, "G.Skill Trident Z 3200MHz – thiết kế cao cấp, ánh sáng RGB nổi bật.", "Ram G.Skill Trident Z 1x8GB RGB 3200", 23 },
                    { 383, 13, "/Assets/img/Products/13/383.jpg", 990000.0, "RAM Delta RGB Black 3200MHz – hiệu năng và thẩm mỹ kết hợp.", "Ram T-Group T-Force Delta 1x8GB 3200 RGB Black", 48 },
                    { 384, 13, "/Assets/img/Products/13/384.jpg", 990000.0, "T-Force Delta White 3200MHz – đồng bộ ánh sáng tuyệt vời.", "Ram T-Group T-Force Delta 1x8GB 3200 RGB White", 12 },
                    { 385, 13, "/Assets/img/Products/13/385.jpg", 1190000.0, "RAM Patriot SLP 3200MHz – lựa chọn bền bỉ cho hệ thống văn phòng.", "Ram Patriot SLP 8GB (1x8GB) 3200", 35 },
                    { 386, 13, "/Assets/img/Products/13/386.jpg", 1190000.0, "Kingston HyperX Predator RGB – hiệu năng đỉnh, phong cách mạnh mẽ.", "Ram Kingston HyperX Predator RGB 8GB 3200", 14 },
                    { 387, 13, "/Assets/img/Products/13/387.jpg", 1190000.0, "RAM 16GB Skywalker Plus – dung lượng lớn, tốc độ 3600MHz.", "Ram V-Color Skywalker Plus 1x16GB 3600 RGB Silver", 32 },
                    { 388, 13, "/Assets/img/Products/13/388.jpg", 1190000.0, "RAM T-Force Delta 16GB – hiệu năng vượt trội cho gamer chuyên nghiệp.", "Ram T-Group T-Force Delta 1x16GB 3600 RGB Black", 46 },
                    { 389, 13, "/Assets/img/Products/13/389.jpg", 890000.0, "Corsair LPX 3600MHz – nhỏ gọn, hiệu năng cao, tản nhiệt tốt.", "Ram Corsair Vengeance LPX 1x8GB 3600", 27 },
                    { 390, 13, "/Assets/img/Products/13/390.jpg", 650000.0, "Ripjaws V 3600MHz – lựa chọn hợp lý cho game thủ phổ thông.", "Ram G.Skill Ripjaws V 1x8GB 3600C16", 17 },
                    { 391, 14, "/Assets/img/Products/14/391.jpg", 5990000.0, "Ổ cứng SSD Samsung 9100 PRO NVMe M.2 1TB Gen5 chuẩn PCIe Gen5, tốc độ cực cao, phù hợp cho làm việc và giải trí.", "Ổ cứng SSD Samsung 9100 PRO NVMe M.2 1TB Gen5", 21 },
                    { 392, 14, "/Assets/img/Products/14/392.jpg", 590000.0, "Ổ cứng SSD SSTC Megamouth 256GB Sata3 tốc độ cao, nâng cấp máy tính nhanh chóng, hiệu quả.", "Ổ Cứng SSD SSTC Megamouth 256GB Sata3", 16 },
                    { 393, 14, "/Assets/img/Products/14/393.jpg", 1090000.0, "SSD SSTC Oceanic Whitetip MAX III 512GB chuẩn NVMe Gen3, hiệu năng ổn định, giá hợp lý.", "Ổ Cứng SSD SSTC Oceanic Whitetip MAX III 512GB M.2 NVME GEN3", 11 },
                    { 394, 14, "/Assets/img/Products/14/394.jpg", 5290000.0, "SSD WD Blue SN5000 2TB chuẩn NVMe Gen4, dung lượng lớn, tốc độ cao cho làm việc chuyên nghiệp.", "Ổ Cứng SSD WD Blue SN5000 2TB M.2 NVMe PCIe Gen4", 25 },
                    { 395, 14, "/Assets/img/Products/14/395.jpg", 1490000.0, "Ổ cứng SSD WD Blue SN5000 500GB NVMe Gen4 hiệu suất cao, giá thành hợp lý.", "Ổ Cứng SSD WD Blue SN5000 500GB M.2 NVMe PCIe Gen4", 19 },
                    { 396, 14, "/Assets/img/Products/14/396.jpg", 2290000.0, "SSD WD Blue SN5000 1TB NVMe Gen4 tốc độ cao, độ bền ổn định, phù hợp đa tác vụ.", "Ổ Cứng SSD WD Blue SN5000 1TB M.2 NVMe PCIe Gen4", 12 },
                    { 397, 14, "/Assets/img/Products/14/397.jpg", 3990000.0, "Ổ cứng SSD Kingston NV3 2TB NVMe Gen4, tốc độ đọc ghi mạnh mẽ, tin cậy cho công việc nặng.", "Ổ cứng SSD Kingston NV3 2TB M.2 PCIe NVMe Gen4", 28 },
                    { 398, 14, "/Assets/img/Products/14/398.jpg", 1890000.0, "SSD Kingston NV3 1TB NVMe Gen4, tối ưu hiệu năng máy tính, tốc độ truyền dữ liệu cao.", "Ổ cứng SSD Kingston NV3 1TB M.2 PCIe NVMe Gen4", 13 },
                    { 399, 14, "/Assets/img/Products/14/399.jpg", 1190000.0, "SSD Kingston NV3 500GB NVMe Gen4, dung lượng vừa đủ, tốc độ nhanh, bền bỉ.", "Ổ cứng SSD Kingston NV3 500GB M.2 PCIe NVMe Gen4", 20 },
                    { 400, 14, "/Assets/img/Products/14/400.jpg", 10990000.0, "SSD Samsung 990 PRO 4TB NVMe Gen4, tốc độ cực cao, hiệu năng hàng đầu cho game và đồ họa.", "Ổ Cứng SSD Samsung 990 PRO 4TB M.2 PCIe Gen4 NVMe", 9 },
                    { 401, 14, "/Assets/img/Products/14/401.jpg", 690000.0, "SSD Addlink S68 256GB NVMe Gen3, tốc độ nhanh, giá tốt, nâng cấp máy tính dễ dàng.", "Ổ cứng Addlink S68 M.2 2280 NVMe Gen 3 256GB", 15 },
                    { 402, 14, "/Assets/img/Products/14/402.jpg", 1690000.0, "SSD GIGABYTE AORUS Gen4 5000E 500GB NVMe Gen4, hiệu năng mạnh, thiết kế cao cấp.", "Ổ Cứng SSD GIGABYTE AORUS Gen4 5000E SSD 500GB", 10 },
                    { 403, 14, "/Assets/img/Products/14/403.jpg", 690000.0, "SSD Verbatim Vi3000 256GB NVMe Gen3, tốc độ đọc ghi ổn định, độ bền cao.", "Ổ cứng SSD Verbatim Vi3000 256GB PCIe NVMe Gen 3", 14 },
                    { 404, 14, "/Assets/img/Products/14/404.jpg", 590000.0, "SSD Verbatim Vi550 256GB Sata3, cải thiện tốc độ khởi động và phản hồi hệ thống.", "Ổ Cứng SSD Verbatim Vi550 256GB Sata3", 17 },
                    { 405, 14, "/Assets/img/Products/14/405.jpg", 2990000.0, "SSD Corsair MP600 CORE XT 1TB NVMe Gen4, hiệu năng mạnh mẽ cho gaming và làm việc.", "Ổ cứng SSD Corsair MP600 CORE XT 1TB PCIe 4.0 Gen4", 26 },
                    { 406, 14, "/Assets/img/Products/14/406.jpg", 590000.0, "SSD PNY CS900 256GB Sata3, giải pháp lưu trữ tiết kiệm và nhanh chóng.", "Ổ Cứng SSD PNY CS900 256GB Sata3", 18 },
                    { 407, 14, "/Assets/img/Products/14/407.jpg", 890000.0, "SSD WD Blue SN580 250GB NVMe Gen4, tốc độ ổn định, giá thành hợp lý.", "Ổ Cứng SSD WD Blue SN580 250GB M.2 NVMe PCIe Gen4", 24 },
                    { 408, 14, "/Assets/img/Products/14/408.jpg", 1890000.0, "SSD WD Blue SN580 1TB NVMe Gen4, hiệu suất cao, độ bền tuyệt vời.", "Ổ Cứng SSD WD Blue SN580 1TB M.2 NVMe PCIe Gen4", 11 },
                    { 409, 14, "/Assets/img/Products/14/409.jpg", 1290000.0, "SSD WD Blue SN580 500GB NVMe Gen4, tốc độ đọc ghi cao, tin cậy.", "Ổ Cứng SSD WD Blue SN580 500GB M.2 NVMe PCIe Gen4", 13 },
                    { 410, 14, "/Assets/img/Products/14/410.jpg", 4990000.0, "SSD Corsair MP600 CORE XT 2TB NVMe Gen4, dung lượng lớn, hiệu năng vượt trội.", "Ổ cứng SSD Corsair MP600 CORE XT 2TB PCIe 4.0 Gen4", 22 },
                    { 411, 14, "/Assets/img/Products/14/411.jpg", 1190000.0, "SSD MSI Spatium S270 960GB SATA3, tốc độ ổn định, phù hợp nâng cấp máy tính.", "Ổ cứng SSD MSI Spatium S270 960GB SATA3", 27 },
                    { 412, 14, "/Assets/img/Products/14/412.jpg", 2990000.0, "SSD MSI M480 PRO NVMe Gen4 1TB, hiệu suất cực cao cho gaming và đồ họa.", "Ổ cứng SSD MSI SPATIUM M480 PRO PCIe 4.0 NVMe M.2 1TB", 9 },
                    { 413, 14, "/Assets/img/Products/14/413.jpg", 1190000.0, "SSD MSI Spatium M450 500GB NVMe Gen4, giá tốt, tốc độ cao, bền bỉ.", "Ổ cứng SSD MSI Spatium M450 500GB M.2 PCIe NVMe Gen4", 19 },
                    { 414, 14, "/Assets/img/Products/14/414.jpg", 4990000.0, "SSD WD Black SN850x 2TB NVMe Gen4, hiệu suất hàng đầu, chuyên dụng cho game thủ.", "Ổ Cứng SSD WD Black SN850x 2TB M.2 PCIe NVMe Gen4", 14 },
                    { 415, 14, "/Assets/img/Products/14/415.jpg", 3590000.0, "SSD WD Black SN850x 1TB NVMe Gen4, tốc độ đọc ghi cực nhanh, độ bền cao.", "Ổ Cứng SSD WD Black SN850x 1TB M.2 PCIe NVMe Gen4", 20 },
                    { 416, 14, "/Assets/img/Products/14/416.jpg", 490000.0, "SSD PNY CS900 120GB SATA3, khởi động nhanh, tiết kiệm chi phí, ổn định.", "Ổ Cứng SSD PNY CS900 120GB Sata3", 17 },
                    { 417, 14, "/Assets/img/Products/14/417.jpg", 590000.0, "SSD TeamGroup CX2 256GB SATA3, hiệu suất tốt, giá rẻ, phù hợp văn phòng.", "Ổ cứng SSD TeamGroup CX2 2.5 inch SATA III 256GB", 23 },
                    { 418, 14, "/Assets/img/Products/14/418.jpg", 490000.0, "SSD KLEVV CRAS C710 256GB NVMe Gen3, tốc độ cao, thiết kế đẹp, đáng tin cậy.", "Ổ Cứng SSD KLEVV CRAS C710 256GB Gen3", 18 },
                    { 419, 14, "/Assets/img/Products/14/419.jpg", 590000.0, "SSD PNY CS900 240GB SATA3, dung lượng vừa đủ, tốc độ cao, tiết kiệm điện năng.", "Ổ Cứng SSD PNY CS900 240GB Sata3", 12 },
                    { 420, 14, "/Assets/img/Products/14/420.jpg", 690000.0, "SSD PNY CS1031 256GB NVMe Gen3, thiết kế nhỏ gọn, hiệu năng tốt, phù hợp laptop.", "Ổ cứng PNY SSD CS1031 M.2 2280 NVMe 256GB", 15 },
                    { 421, 15, "/Assets/img/Products/15/421.jpg", 4990000.0, "Tản nhiệt nước AIO Lian Li HydroShift LCD 360TL màu trắng, hiệu suất cao, không kèm fan.", "Tản nhiệt AIO Lian Li HydroShift LCD 360TL White (Non Fan)", 18 },
                    { 422, 15, "/Assets/img/Products/15/422.jpg", 4990000.0, "Tản nhiệt nước AIO Lian Li HydroShift LCD 360TL màu đen, hiệu suất cao, không kèm fan.", "Tản nhiệt AIO Lian Li HydroShift LCD 360TL Black (Non Fan)", 25 },
                    { 423, 15, "/Assets/img/Products/15/423.jpg", 7990000.0, "Tản nhiệt nước AIO TRYX Panorama 360 SE ARGB màu trắng, trang bị LED ARGB rực rỡ.", "Tản nhiệt AIO TRYX Panorama 360 SE ARGB White", 7 },
                    { 424, 15, "/Assets/img/Products/15/424.jpg", 7790000.0, "Tản nhiệt nước AIO TRYX Panorama 360 SE ARGB màu đen, trang bị LED ARGB rực rỡ.", "Tản nhiệt AIO TRYX Panorama 360 SE ARGB Black", 31 },
                    { 425, 15, "/Assets/img/Products/15/425.jpg", 9790000.0, "Tản nhiệt nước AIO TRYX Panorama 360 ARGB màu trắng với hiệu năng đỉnh cao.", "Tản nhiệt AIO TRYX Panorama 360 ARGB White", 14 },
                    { 426, 15, "/Assets/img/Products/15/426.jpg", 9790000.0, "Tản nhiệt nước AIO TRYX Panorama 360 ARGB màu đen với hiệu năng đỉnh cao.", "Tản nhiệt AIO TRYX Panorama 360 ARGB Black", 22 },
                    { 427, 15, "/Assets/img/Products/15/427.jpg", 4990000.0, "Tản nhiệt Jonsbo TF-360 tích hợp màn hình LCD, phiên bản màu trắng, hỗ trợ LED ARGB.", "Tản nhiệt Jonsbo TF-360 LCD White ARGB", 9 },
                    { 428, 15, "/Assets/img/Products/15/428.jpg", 4990000.0, "Tản nhiệt Jonsbo TF-360 tích hợp màn hình LCD, phiên bản màu đen, hỗ trợ LED ARGB.", "Tản nhiệt Jonsbo TF-360 LCD Black ARGB", 34 },
                    { 429, 15, "/Assets/img/Products/15/429.jpg", 1890000.0, "Tản nhiệt Jonsbo TH-360 màu trắng, giải pháp tản nhiệt hiệu quả với LED ARGB.", "Tản nhiệt Jonsbo TH-360 White ARGB", 11 },
                    { 430, 15, "/Assets/img/Products/15/430.jpg", 1890000.0, "Tản nhiệt Jonsbo TH-360 màu đen, giải pháp tản nhiệt hiệu quả với LED ARGB.", "Tản nhiệt Jonsbo TH-360 Black ARGB", 29 },
                    { 431, 15, "/Assets/img/Products/15/431.jpg", 1590000.0, "Tản nhiệt Jonsbo TH-240 màu trắng, kích thước nhỏ gọn, hỗ trợ LED ARGB.", "Tản nhiệt Jonsbo TH-240 White ARGB", 5 },
                    { 432, 15, "/Assets/img/Products/15/432.jpg", 1590000.0, "Tản nhiệt Jonsbo TH-240 màu đen, kích thước nhỏ gọn, hỗ trợ LED ARGB.", "Tản nhiệt Jonsbo TH-240 Black ARGB", 20 },
                    { 433, 15, "/Assets/img/Products/15/433.jpg", 10990000.0, "Tản nhiệt AIO cao cấp ASUS ROG Ryujin III 360 ARGB Extreme, phiên bản màu trắng.", "Tản nhiệt AIO ASUS ROG Ryujin III 360 ARGB Extreme White", 16 },
                    { 434, 15, "/Assets/img/Products/15/434.jpg", 7990000.0, "Tản nhiệt AIO Lian Li HydroShift LCD 360TL màu trắng, đi kèm fan không dây tiện lợi.", "Tản nhiệt AIO Lian Li HydroShift LCD 360TL Wireless Fan White", 32 },
                    { 435, 15, "/Assets/img/Products/15/435.jpg", 7990000.0, "Tản nhiệt AIO Lian Li HydroShift LCD 360TL màu đen, đi kèm fan không dây tiện lợi.", "Tản nhiệt AIO Lian Li HydroShift LCD 360TL Wireless Fan Black", 8 },
                    { 436, 15, "/Assets/img/Products/15/436.jpg", 1590000.0, "Tản nhiệt khí Cooler Master Hyper 612 Apex, làm mát hiệu quả cho CPU.", "Tản nhiệt Cooler Master Hyper 612 Apex", 26 },
                    { 437, 15, "/Assets/img/Products/15/437.jpg", 2790000.0, "Tản nhiệt AIO Cooler Master MasterLiquid 360 Atmos Stealth với thiết kế tàng hình độc đáo.", "Tản nhiệt AIO Cooler Master MasterLiquid 360 Atmos Stealth", 13 },
                    { 438, 15, "/Assets/img/Products/15/438.jpg", 9990000.0, "Tản nhiệt AIO ASUS ROG Ryuo IV SLC 360 ARGB mang lại hiệu năng tản nhiệt vượt trội.", "Tản nhiệt AIO ASUS ROG Ryuo IV SLC 360 ARGB", 19 },
                    { 439, 15, "/Assets/img/Products/15/439.jpg", 2890000.0, "Tản nhiệt AIO MSI MAG CORELIQUID I240, phiên bản màu đen mạnh mẽ.", "Tản nhiệt AIO MSI MAG CORELIQUID I240 BLACK", 35 },
                    { 440, 15, "/Assets/img/Products/15/440.jpg", 2990000.0, "Tản nhiệt AIO MSI MAG CORELIQUID I240, phiên bản màu trắng tinh tế.", "Tản nhiệt AIO MSI MAG CORELIQUID I240 WHITE", 6 },
                    { 441, 15, "/Assets/img/Products/15/441.jpg", 2890000.0, "Tản nhiệt khí cao cấp Noctua NH-D15S phiên bản Chromax Black toàn bộ màu đen.", "Tản nhiệt Noctua NH-D15S Chromax Black", 24 },
                    { 442, 15, "/Assets/img/Products/15/442.jpg", 6490000.0, "Tản nhiệt AIO Corsair iCUE LINK TITAN 360 RX RGB LCD màu trắng, tích hợp màn hình LCD.", "Tản nhiệt AIO Corsair iCUE LINK TITAN 360 RX RGB LCD White ", 17 },
                    { 443, 15, "/Assets/img/Products/15/443.jpg", 6490000.0, "Tản nhiệt AIO Corsair iCUE LINK TITAN 360 RX RGB LCD màu đen, tích hợp màn hình LCD.", "Tản nhiệt AIO Corsair iCUE LINK TITAN 360 RX RGB LCD Black", 30 },
                    { 444, 15, "/Assets/img/Products/15/444.jpg", 2990000.0, "Tản nhiệt nước AIO ASUS TUF Gaming LC II 240 ARGB, bền bỉ và mạnh mẽ.", "Tản nhiệt nước AIO ASUS TUF Gaming LC II 240 ARGB", 10 },
                    { 445, 15, "/Assets/img/Products/15/445.jpg", 7990000.0, "Tản nhiệt AIO GIGABYTE AORUS WATERFORCE II 360 phiên bản ICE màu trắng bạc.", "Tản nhiệt AIO GIGABYTE AORUS WATERFORCE II 360 ICE", 28 },
                    { 446, 15, "/Assets/img/Products/15/446.jpg", 1990000.0, "Tản nhiệt AIO Cooler Master MasterLiquid 360 Core SI ARGB, hiệu quả và giá cả phải chăng.", "Tản nhiệt AIO Cooler Master MasterLiquid 360 Core SI ARGB", 12 },
                    { 447, 15, "/Assets/img/Products/15/447.jpg", 1990000.0, "Tản nhiệt AIO Cooler Master MasterLiquid 360 Core SI ARGB, phiên bản màu trắng.", "Tản nhiệt AIO Cooler Master MasterLiquid 360 Core SI ARGB White", 21 },
                    { 448, 15, "/Assets/img/Products/15/448.jpg", 7990000.0, "Tản nhiệt AIO ASUS ROG STRIX LC III 360 ARGB tích hợp màn hình LCD hiển thị thông số.", "Tản nhiệt AIO ASUS ROG STRIX LC III 360 ARGB LCD", 33 },
                    { 449, 15, "/Assets/img/Products/15/449.jpg", 5790000.0, "Tản nhiệt AIO Cooler Master MasterLiquid 360 ION ARGB với thiết kế pump độc đáo.", "Tản nhiệt AIO Cooler Master MasterLiquid 360 ION ARGB", 15 },
                    { 450, 15, "/Assets/img/Products/15/450.jpg", 10990000.0, "Tản nhiệt AIO cao cấp ASUS ROG Ryujin III 360 ARGB Extreme, phiên bản màu đen.", "Tản nhiệt AIO ASUS ROG Ryujin III 360 ARGB Extreme", 27 },
                    { 451, 16, "/Assets/img/Products/16/451.jpg", 890000.0, "Bộ sạc nhanh Ugreen công nghệ GaN 100W, màu trắng, model CD226.", "Bộ sạc nhanh GaN 100W Ugreen CD226 15336 (Trắng)", 28 },
                    { 452, 16, "/Assets/img/Products/16/452.jpg", 790000.0, "Củ sạc Ugreen công nghệ GaN 100W, màu trắng, model CD226.", "Củ Sạc Ugreen GaN 100W CD226 Wh 15336", 35 },
                    { 453, 16, "/Assets/img/Products/16/453.jpg", 190000.0, "Cáp sạc Belkin Type C to Type C, công suất 60W, dài 1m, màu trắng.", "Cáp sạc Belkin Type C - Type C 60W 1m trắng CAB003", 19 },
                    { 454, 16, "/Assets/img/Products/16/454.jpg", 3190000.0, "Microphone Razer Seiren V3 Chroma, phiên bản màu hồng thạch anh (Quartz).", "Microphone Razer Seiren V3 Chroma - Quartz", 24 },
                    { 455, 16, "/Assets/img/Products/16/455.jpg", 350000.0, "Sạc dự phòng Recci RPB-P12C, màu xanh dương, thiết kế nhỏ gọn.", "Sạc dự phòng Recci RPB-P12C Blue", 39 },
                    { 456, 16, "/Assets/img/Products/16/456.jpg", 350000.0, "Sạc dự phòng Recci RPB-P12C, màu trắng, thiết kế nhỏ gọn.", "Sạc dự phòng Recci RPB-P12C White", 16 },
                    { 457, 16, "/Assets/img/Products/16/457.jpg", 1090000.0, "Bộ chuyển đổi Mazer Infinite.HUB Pro 7-in-1 USB-C đa năng.", "Bộ Chuyển Đổi Mazer Infinite.HUB Pro 7-in-1 USB-C M-UC2MULTI7002-BK", 27 },
                    { 458, 16, "/Assets/img/Products/16/458.jpg", 1490000.0, "Cổng chuyển HyperDrive PRO 8-in-2 Hub dành cho MacBook Pro.", "Cổng chuyển HyperDrive PRO 8-in-2 Hub for USB-C MacBook Pro - GN28D GREY", 20 },
                    { 459, 16, "/Assets/img/Products/16/459.jpg", 290000.0, "Củ sạc Ugreen GaN Nexode 30W, màu trắng, nhỏ gọn và mạnh mẽ.", "Củ Sạc Ugreen GaN Nexode 30W CD319 Wh 15329", 36 },
                    { 460, 16, "/Assets/img/Products/16/460.jpg", 210000.0, "Cáp sạc Belkin Type C to Type C, công suất 60W, dài 2m, màu trắng.", "Cáp sạc Belkin Type C - Type C 60W 2m trắng CAB003", 22 },
                    { 461, 16, "/Assets/img/Products/16/461.jpg", 190000.0, "Củ sạc Ugreen GaN Nexode 30W, màu xám, nhỏ gọn và mạnh mẽ.", "Củ Sạc Ugreen GaN Nexode 30W CD319 Gr 90901", 38 },
                    { 462, 16, "/Assets/img/Products/16/462.jpg", 390000.0, "Dây cáp Mazer Infinite.LINK 3 Pro USB-C to USB-C, 1.25m, 100W.", "Dây Cáp Mazer Infinite.LINK 3 Pro Cable USB-C TO USB-C 1.25m 100W M-PL3Pro-C2C125-BK", 17 },
                    { 463, 16, "/Assets/img/Products/16/463.jpg", 190000.0, "Bộ chuyển đổi Mazer ALU USB-C sang VGA, hỗ trợ 1080P.", "Bộ Chuyển Đổi Mazer ALU USB-C to VGA 1080P Adapter M-USBCAL351-GY", 31 },
                    { 464, 16, "/Assets/img/Products/16/464.jpg", 490000.0, "Dây cáp Mazer Infinite.LINK 3 Pro USB-C to USB-C, 2.5m, 100W.", "Dây Cáp Mazer Infinite.LINK 3 Pro Cable USB-C TO USB-C 2.5m 100W M-PL3Pro-C2C250-BK", 25 },
                    { 465, 16, "/Assets/img/Products/16/465.jpg", 490000.0, "Ổ cắm điện đa năng dạng tháp Upgen, nhiều cổng cắm tiện lợi.", "Ổ cắm điện đa năng Upgen Tower", 33 },
                    { 466, 16, "/Assets/img/Products/16/466.jpg", 100000.0, "Dây cáp HDMI 2.0 Dtech, dài 1.5m, hỗ trợ độ phân giải cao.", "Dây Cáp HDMI Dtech DT-UH0015G 2.0 1.5m", 18 },
                    { 467, 16, "/Assets/img/Products/16/467.jpg", 230000.0, "Dây cáp Displayport Dtech, dài 1.8m, chất lượng cao.", "Dây Cáp Displayport Dtech DT-CU0308 1.8m", 40 },
                    { 468, 16, "/Assets/img/Products/16/468.jpg", 3190000.0, "Microphone Razer Seiren V3 Chroma, phiên bản màu trắng.", "Microphone Razer Seiren V3 Chroma - White", 15 },
                    { 469, 16, "/Assets/img/Products/16/469.jpg", 3190000.0, "Microphone Razer Seiren V3 Chroma, phiên bản màu đen.", "Microphone Razer Seiren V3 Chroma - Black", 26 },
                    { 470, 16, "/Assets/img/Products/16/470.jpg", 1290000.0, "Microphone Razer Seiren V3 Mini, nhỏ gọn, phiên bản màu hồng thạch anh (Quartz).", "Microphone Razer Seiren V3 Mini - Quartz", 32 },
                    { 471, 16, "/Assets/img/Products/16/471.jpg", 1290000.0, "Microphone Razer Seiren V3 Mini, nhỏ gọn, phiên bản màu trắng.", "Microphone Razer Seiren V3 Mini - White", 21 },
                    { 472, 16, "/Assets/img/Products/16/472.jpg", 1290000.0, "Microphone Razer Seiren V3 Mini, nhỏ gọn, phiên bản màu đen.", "Microphone Razer Seiren V3 Mini - Black", 37 },
                    { 473, 16, "/Assets/img/Products/16/473.jpg", 199000.0, "Giá đỡ Laptop N3 bằng nhôm, giúp tản nhiệt và cải thiện tư thế ngồi.", "Phụ kiện giá đỡ Laptop N3", 23 },
                    { 474, 16, "/Assets/img/Products/16/474.jpg", 300000.0, "Cổng chuyển đổi USB C Ugreen 4 trong 1, nhỏ gọn và tiện lợi.", "Cổng chuyển đổi USB C Ugreen 4 in 1 CM219 70336", 30 },
                    { 475, 16, "/Assets/img/Products/16/475.jpg", 320000.0, "Cổng chuyển đổi USB C Ugreen 5 trong 1, mở rộng kết nối cho thiết bị.", "Cổng chuyển đổi USB C Ugreen 5 in 1 CM478 15495", 19 },
                    { 476, 16, "/Assets/img/Products/16/476.jpg", 890000.0, "Cổng chuyển đổi USB C Ugreen 7 trong 1, giải pháp kết nối toàn diện.", "Cổng chuyển đổi USB C Ugreen 7 in 1 CM512 90568", 29 },
                    { 477, 16, "/Assets/img/Products/16/477.jpg", 690000.0, "Cổng chuyển đổi USB C Ugreen 5 trong 1, hỗ trợ nhiều cổng kết nối phổ biến.", "Cổng chuyển đổi USB C Ugreen 5 in 1 CM136 50209", 24 },
                    { 478, 16, "/Assets/img/Products/16/478.jpg", 300000.0, "Cổng chuyển đổi USB C Ugreen 4 trong 1, thiết kế vỏ nhôm cao cấp.", "Cổng chuyển đổi USB C Ugreen 4 in 1 CM219 50985", 34 },
                    { 479, 16, "/Assets/img/Products/16/479.jpg", 530000.0, "Cổng chuyển đổi USB C Ugreen 5 trong 1, đa năng và hiệu quả.", "Cổng chuyển đổi USB C Ugreen 5 in 1 CM511 15596", 18 },
                    { 480, 16, "/Assets/img/Products/16/480.jpg", 190000.0, "Cổng chuyển đổi Ugreen từ USB 2.0 sang cổng LAN RJ45.", "Công chuyển đổi Ugreen USB 2.0 to LAN RJ45 CR110 20254", 28 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "UQ_Cart_Product",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Review_ProductId",
                table: "tb_Review",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Review_UserId",
                table: "tb_Review",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "tb_Review");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
