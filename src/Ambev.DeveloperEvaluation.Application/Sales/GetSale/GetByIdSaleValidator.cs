using Ambev.DeveloperEvaluation.Common.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommand
/// </summary>
public class GetByIdSaleValidator : AbstractValidator<GetByIdSaleQuery>
{
    /// <summary>
    /// Initializes validation rules for GetSaleCommand
    /// </summary>
    public GetByIdSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ValidationMessages.General.FieldIsRequired);
    }
}
