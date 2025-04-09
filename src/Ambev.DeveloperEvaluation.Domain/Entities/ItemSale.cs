using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {       
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public Guid SaleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantities { get; set; }
        public decimal UnitPrices { get; set; }
        public decimal Discounts { get; set; }
        public decimal TotalValueItem { get; set; }
    }
}
