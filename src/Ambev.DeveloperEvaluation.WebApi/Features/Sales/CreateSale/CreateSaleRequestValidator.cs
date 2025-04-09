using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateUserRequest that defines validation rules for user creation.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateUserRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be valid format (using EmailValidator)
    /// - Username: Required, length between 3 and 50 characters
    /// - Password: Must meet security requirements (using PasswordValidator)
    /// - Phone: Must match international format (+X XXXXXXXXXX)
    /// - Status: Cannot be Unknown
    /// - Role: Cannot be None
    /// </remarks>
    public CreateSaleRequestValidator()
    {
        RuleFor(user => user.Customer)
          .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
          .MaximumLength(50).WithMessage("Username cannot be longer than 50 characters.");

        RuleFor(x => x.SaleNumber).NotEmpty().WithMessage("numero da venda é obrigatório.");
        RuleFor(x => x.Customer).NotEmpty().WithMessage("Cliente é obrigatório.");
        RuleFor(x => x.Branch).NotEmpty().WithMessage("Filial é obrigatória.");
        RuleFor(x => x.Products).NotEmpty().WithMessage("Deve haver ao menos um produto na venda.");


        RuleForEach(x => x.Products).ChildRules(item =>
        {
            item.RuleFor(i => i.Name).NotEmpty().WithMessage("Nome do produto é obrigatório.");
            item.RuleFor(i => i.Quantities)
                .InclusiveBetween(1, 20)
                .WithMessage("A quantidade deve estar entre 1 e 20.");
            item.RuleFor(i => i.UnitPrices)
                .GreaterThan(0).WithMessage("Preço unitário deve ser maior que zero.");
        });

    }
}