using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Response model for GetSale operation
/// </summary>
public class GetSaleResult
{
    /// <summary>
    /// The unique identifier of the Sale
    /// </summary>
    public Guid Id { get; set; }
    public List<GetSaleItemResult>? Itens { get; set; }
    public int Quantities { get; set; }
    public decimal UnitPrices { get; set; }
    public bool? Cancelled { get; set; } = false;
    public string? SaleNumber { get; set; }
    public DateTime? SaleDate { get; set; }
    public string? Customer { get; set; }
    public string? Branch { get; set; }
}
public class GetSaleItemResult
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Name { get; set; } 
    public int Quantities { get; set; }
    public decimal UnitPrices { get; set; }
    public decimal Discounts { get; set; }
    public decimal TotalValueItem { get; set; }
}