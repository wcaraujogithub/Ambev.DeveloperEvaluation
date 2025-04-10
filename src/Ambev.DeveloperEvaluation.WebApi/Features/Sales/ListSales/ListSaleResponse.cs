namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// API response model for ListSale operation
/// </summary>
public class ListSaleResponse
{

    public Guid Id { get; set; }
    public List<ListSaleItemResponse>? Items { get; set; }
    public int Quantities { get; set; }
    public decimal UnitPrices { get; set; }
    public bool? Cancelled { get; set; }
    public string? SaleNumber { get; set; }
    public DateTime? SaleDate { get; set; }
    public string? Customer { get; set; }
    public string? Branch { get; set; }
}
public class ListSaleItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Name { get; set; }
    public int Quantities { get; set; }
    public decimal UnitPrices { get; set; }
    public decimal Discounts { get; set; }
    public decimal TotalValueItem { get; set; }
}