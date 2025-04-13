using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Response model for DeleteUser operation
/// </summary>
public class DeleteSaleResponse
{
    /// <summary>
    /// Indicates whether the deletion was successful
    /// </summary>
    public Guid Id { get; set; }
    public List<DeleteSaleItemResult>? Items { get; set; }
    public bool? Cancelled { get; set; }
    public string? SaleNumber { get; set; }
    public string? Customer { get; set; }
    public string? Branch { get; set; }
    public string? TotalValue { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
public class DeleteSaleItemResult
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Name { get; set; }
    public int Quantities { get; set; }
    public decimal UnitPrices { get; set; }
    public decimal Discounts { get; set; }
    public decimal TotalValueItem { get; set; }
}