namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// API response model for GetSale operation
/// </summary>
public class GetSaleResponse
{
    /// <summary>
    /// The unique identifier of the Sale
    /// </summary>
    public Guid? Id { get; set; }
    public List<GetSaleItemResponse>? Itens { get; set; }   
    public bool? Cancelled { get; set; } = false;
    public string? SaleNumber { get; set; }  
    public string? Customer { get; set; }
    public string? Branch { get; set; }
    public string? CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }
}

public class GetSaleItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Name { get; set; } 
    public int Quantities { get; set; }
    public decimal UnitPrices { get; set; }
    public decimal Discounts { get; set; }
    public decimal TotalValueItem { get; set; }
}