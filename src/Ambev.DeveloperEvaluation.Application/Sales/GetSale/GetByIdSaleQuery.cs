using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for get a GetByIdSaleQuery
/// </summary>
public record GetByIdSaleQuery : IRequest<GetByIdSaleQueryResult>
{
    /// <summary>
    /// The unique identifier of the Sale 
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of GetByIdSaleQuery
    /// </summary>
    /// <param name="id">The ID of the Sale </param>
    public GetByIdSaleQuery(Guid id)
    {
        Id = id;
    }
}
