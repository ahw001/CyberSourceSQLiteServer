using CybsClass.EntityModels;
using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.DTOs;

public class DBProductDto
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = null!;

    [JsonPropertyName("supplierId")]
    public int? SupplierId { get; set; }

    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }

    [JsonPropertyName("quantityPerUnit")]
    public string? QuantityPerUnit { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal? UnitPrice { get; set; }

    [JsonPropertyName("unitsInStock")]
    public short? UnitsInStock { get; set; }

    [JsonPropertyName("unitsOnOrder")]
    public short? UnitsOnOrder { get; set; }

    [JsonPropertyName("reorderLevel")]
    public short? ReorderLevel { get; set; }

    [JsonPropertyName("discontinued")]
    public bool Discontinued { get; set; }

    public static explicit operator DBProductDto(Product v)
    {
        throw new NotImplementedException();
    }
}

