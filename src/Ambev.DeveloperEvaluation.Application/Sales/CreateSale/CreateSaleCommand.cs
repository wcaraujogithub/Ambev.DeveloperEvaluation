using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? SaleNumber { get; set; }
        public string? Customer { get; set; }
        public string? Branch { get; set; }
        public List<SaleItemDTO>? Items { get; set; }
        public ValidationResultDetail Validate()
        {
            var validator = new CreateSaleCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }

    public class SaleItemDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Quantities { get; set; }
        public decimal UnitPrices { get; set; }
    }
}
