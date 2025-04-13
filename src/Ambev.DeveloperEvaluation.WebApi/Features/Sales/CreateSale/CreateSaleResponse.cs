using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// API response model for CreateUser operation
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// The unique identifier of the created user
    /// </summary>
    public Guid? Id { get; set; }
}
