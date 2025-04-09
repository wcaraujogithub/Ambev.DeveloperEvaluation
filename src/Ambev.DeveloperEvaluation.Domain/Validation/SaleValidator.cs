using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {

            RuleFor(user => user.Customer)
                .NotEmpty()
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
}
