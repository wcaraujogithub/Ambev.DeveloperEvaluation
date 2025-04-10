using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleRequest 
    {
        public Guid Id { get; set; }
        public string? Customer { get; set; }
        public string? Branch { get; set; }

        public ValidationResultDetail Validate()
        {
            var validator = new UpdateSaleRequestValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
