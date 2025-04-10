namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Response model for ListSale operation
/// </summary>
public class ListSaleResult
{
    /// <summary>
    /// The unique identifier of the Sale
    /// </summary>
    public Guid Id { get; set; }
    public List<ListSaleItemResult>? Items { get; set; }  
    public bool? Cancelled { get; set; }
    public string? SaleNumber { get; set; }  
    public string? Customer { get; set; }
    public string? Branch { get; set; }
    public string? TotalValue { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ListSaleItemResult
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Name { get; set; }
    public int Quantities { get; set; }
    public decimal UnitPrices { get; set; }
    public decimal Discounts { get; set; }
    public decimal TotalValueItem { get; set; }
}