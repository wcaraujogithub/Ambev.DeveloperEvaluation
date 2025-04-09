using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateUserCommand that defines validation rules for user creation command.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateUserCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be in valid format (using EmailValidator)
    /// - Username: Required, must be between 3 and 50 characters
    /// - Password: Must meet security requirements (using PasswordValidator)
    /// - Phone: Must match international format (+X XXXXXXXXXX)
    /// - Status: Cannot be set to Unknown
    /// - Role: Cannot be set to None
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(user => user.Customer)
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Username cannot be longer than 50 characters.");

        RuleFor(x => x.Customer).NotEmpty().WithMessage("Cliente é obrigatório.");
        RuleFor(x => x.Branch).NotEmpty().WithMessage("Filial é obrigatória.");
        RuleFor(x => x.Itens).NotEmpty().WithMessage("Deve haver ao menos um produto na venda.");

        RuleForEach(x => x.Itens).ChildRules(item =>
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