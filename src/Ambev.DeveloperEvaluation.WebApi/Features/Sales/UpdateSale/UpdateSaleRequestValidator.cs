using Ambev.DeveloperEvaluation.Common.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleCommand that defines validation rules for sale updation command.
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include: 
    /// - Customer: Required, length between 3 and 50 characters 
    /// - Branch: Required
    /// </remarks>
    public UpdateSaleRequestValidator()
    {
        RuleFor(sale => sale.Customer)
              .NotEmpty().WithMessage(ValidationMessages.General.FieldIsRequired)
            .MinimumLength(3)
            .WithMessage(ValidationMessages.General.MaximumLength)
            .MaximumLength(50)
            .WithMessage(ValidationMessages.General.MaximumLength);
        RuleFor(x => x.Branch)
            .NotEmpty()
            .WithMessage(ValidationMessages.General.FieldIsRequired);
    }
}