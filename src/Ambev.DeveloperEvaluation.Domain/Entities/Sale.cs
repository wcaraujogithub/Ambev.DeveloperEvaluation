using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string? SaleNumber { get; set; } = string.Empty;
        public string? Customer { get; set; } = string.Empty;
        public decimal? TotalValue { get; set; }
        public string? Branch { get; set; } = string.Empty;
        public List<SaleItem> Itens { get; set; }
        public bool? Cancelled { get; set; }
        /// <summary>
        /// Gets the date and time when the sale was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets the date and time of the last update to the sale's information.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Performs validation of the Sale entity using the SaleValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        /// <remarks>
        /// <listheader>The validation includes checking:</listheader>
        /// <list type="bullet">Salename format and length</list>
        /// <list type="bullet">Email format</list>
        /// <list type="bullet">Phone number format</list>
        /// <list type="bullet">Password complexity requirements</list>
        /// <list type="bullet">Role validity</list>
        /// 
        /// </remarks>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
        /// <summary>
        /// Not cancelled the sale.
        /// Changes the sale's status to NotCancelled.
        /// </summary>
        public void NotCancelled()
        {
            Cancelled = false;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Cancelled the sale.
        /// Changes the sale's status to IsCancelled.
        /// </summary>
        public void IsCancelled()
        {
            Cancelled = true;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
