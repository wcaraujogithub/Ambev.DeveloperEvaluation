using Ambev.DeveloperEvaluation.Common.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation command.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include: 
    /// - Customer: Required, length between 3 and 50 characters
    /// - SaleNumber: Required
    /// - Branch: Required
    /// - Products.Name: Required
    /// - Products.Quantities: Required
    /// - Products.UnitPrices: Required
    /// - Products.UnitPrices: Required
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage(ValidationMessages.General.FieldIsRequired);

        RuleFor(sale => sale.Customer)
              .NotEmpty()
              .WithMessage(ValidationMessages.General.FieldIsRequired)
            .MinimumLength(3)
            .WithMessage(ValidationMessages.General.MaximumLength)
            .MaximumLength(50)
            .WithMessage(ValidationMessages.General.MaximumLength);

        RuleFor(x => x.Branch)
            .NotEmpty().WithName(c => c.Branch)
            .WithMessage(ValidationMessages.General.FieldIsRequired);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(ValidationMessages.Sale.ProductItensRequired);

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Name)
            .NotEmpty()
            .WithMessage(ValidationMessages.General.FieldIsRequired);

            item.RuleFor(i => i.Quantities)
                  .NotEmpty()
                  .WithMessage(ValidationMessages.General.FieldIsRequired)
                  .InclusiveBetween(1, 20)
                  .WithMessage(ValidationMessages.General.InclusiveBetween);

            item.RuleFor(i => i.UnitPrices)
            .NotEmpty()
            .WithMessage(ValidationMessages.General.FieldIsRequired)
            .GreaterThan(0).WithMessage(ValidationMessages.Sale.UnitPricesRequired);
        });
    }
}