using System.Net.Mime;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Data.Models.Orders;

public sealed class OrderItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PictureUrl { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal Discount { get; set; }
    public decimal Total => Quantity * UnitPrice;
}