using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public List<SaleItemDTO>? Itens { get; set; }
        public int Quantities { get; set; }
        public decimal UnitPrices { get; set; }
        public bool? Cancelled { get; set; } = false;
        public string? SaleNumber { get; set; }
        public DateTime? SaleDate { get; set; }
        public string? Customer { get; set; }
        public string? Branch { get; set; }

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
        public decimal Discounts { get; set; }
        public decimal TotalItem { get; set; }
    }
}
