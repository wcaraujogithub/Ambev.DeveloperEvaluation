﻿using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sales in the system.
/// </summary>
public class CreateSaleRequest
{
    public string? SaleNumber { get; set; } = string.Empty; 
    public string? Customer { get; set; } = string.Empty;
    public string? Branch { get; set; } = string.Empty;
    public List<CreateSaleItemRequest>? Items { get; set; }
}

public class CreateSaleItemRequest
{
    public string Name { get; set; } = string.Empty;
    public int Quantities { get; set; }
    public decimal UnitPrices { get; set; }
}


