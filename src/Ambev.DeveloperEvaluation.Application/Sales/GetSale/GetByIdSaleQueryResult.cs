using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Response model for GetByIdSaleQueryResult operation
/// </summary>
public class GetByIdSaleQueryResult
{
    /// <summary>
    /// The unique identifier of the Sale for id
    /// </summary>
    public Guid Id { get; set; }
    public List<GetSaleItemResult>? Items { get; set; }
    public bool? Cancelled { get; set; }
    public string? SaleNumber { get; set; }
    public string? Customer { get; set; }
    public string? Branch { get; set; }
    public string? TotalValue { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
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