using HTNLShop.Data;
using HTNLShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// Dữ liệu
// ---------------------------------------------------------------------------
builder.Services.AddDbContext<HtlnshopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HTLNShop")));

// ---------------------------------------------------------------------------
// Xác thực: ASP.NET Core Identity (hash mật khẩu, lockout, role, token)
// ---------------------------------------------------------------------------
builder.Services
    .AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.AllowedForNewUsers = true;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<HtlnshopContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "htnl.auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.LoginPath = "/Customer/Login";
    options.LogoutPath = "/Customer/Logout";
    options.AccessDeniedPath = "/Customer/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// ---------------------------------------------------------------------------
// MVC: chống CSRF cho toàn bộ action không phải GET
// ---------------------------------------------------------------------------
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<ICartMergeService, CartMergeService>();

var app = builder.Build();

// ---------------------------------------------------------------------------
// Pipeline
// ---------------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

// Header bảo mật cơ bản cho mọi response (kể cả tệp tĩnh)
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "DENY";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    await next();
});

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        // Ảnh/CSS/JS ít đổi → cho trình duyệt cache 7 ngày.
        context.Context.Response.Headers.CacheControl = "public,max-age=604800";
    }
});
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Admin đã đăng nhập thì vào thẳng khu quản trị khi mở trang chủ
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLowerInvariant();

    if ((path == "/" || path == "/trangchu" || path == "/trangchu/index") &&
        context.User.Identity?.IsAuthenticated == true &&
        context.User.IsInRole("Admin"))
    {
        context.Response.Redirect("/admin");
        return;
    }

    await next();
});

await DbSeeder.InitializeAsync(app.Services, app.Environment);

// Health check tối giản cho Docker/uptime monitor.
app.MapGet("/health", async (HtlnshopContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect
        ? Results.Text("ok")
        : Results.Text("degraded", statusCode: StatusCodes.Status503ServiceUnavailable);
}).AllowAnonymous();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}");

app.Run();

/// <summary>Marker cho WebApplicationFactory trong project test.</summary>
public partial class Program
{
}
