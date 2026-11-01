using System;
using System.Collections.Generic;

namespace HTNLShop.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    /// <summary>Trạng thái đơn — xem <see cref="OrderStatus"/>.</summary>
    public string Status { get; set; } = OrderStatus.Pending;

    public decimal? TotalPrice { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string? ReceiverName { get; set; }

    public string? ReceiverPhone { get; set; }

    public string? Note { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
