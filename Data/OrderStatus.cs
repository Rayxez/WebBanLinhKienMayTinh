namespace HTNLShop.Data;

/// <summary>
/// Vòng đời đơn hàng và luật chuyển trạng thái. Mọi chuyển trạng thái phải đi qua
/// <see cref="CanTransition"/> để tránh nhảy cóc (ví dụ "Chờ xác nhận" → "Đã giao").
/// </summary>
public static class OrderStatus
{
    public const string Pending = "Chờ xác nhận";
    public const string Confirmed = "Đã xác nhận";
    public const string Shipping = "Đang giao";
    public const string Delivered = "Đã giao";
    public const string Completed = "Hoàn tất";
    public const string Cancelled = "Đã huỷ";

    public static readonly string[] All =
    {
        Pending, Confirmed, Shipping, Delivered, Completed, Cancelled
    };

    /// <summary>Chỉ các trạng thái này mới được tính vào doanh thu.</summary>
    public static readonly string[] Revenue =
    {
        Delivered, Completed
    };

    private static readonly Dictionary<string, string[]> Transitions = new()
    {
        [Pending] = new[] { Confirmed, Cancelled },
        [Confirmed] = new[] { Shipping, Cancelled },
        [Shipping] = new[] { Delivered },
        [Delivered] = new[] { Completed },
        [Completed] = Array.Empty<string>(),
        [Cancelled] = Array.Empty<string>()
    };

    public static bool CanTransition(string? from, string? to) =>
        from is not null &&
        to is not null &&
        Transitions.TryGetValue(from, out var next) &&
        next.Contains(to);

    public static IReadOnlyList<string> Next(string? from) =>
        from is not null && Transitions.TryGetValue(from, out var next)
            ? next
            : Array.Empty<string>();

    /// <summary>Huỷ đơn phải hoàn lại tồn kho đã trừ lúc đặt hàng.</summary>
    public static bool RestoresStock(string to) => to == Cancelled;
}
