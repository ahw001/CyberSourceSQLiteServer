namespace CybsClass.Cybersource.Models.DTOs;


public partial class OrderDetailDto
{

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public short Quantity { get; set; }

    public float Discount { get; set; }

    public string? Error { get; set; }

}
