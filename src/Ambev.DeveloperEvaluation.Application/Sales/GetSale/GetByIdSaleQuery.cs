using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Common.Validation;
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
    public ValidationResultDetail Validate()
    {
        var validator = new GetByIdSaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
