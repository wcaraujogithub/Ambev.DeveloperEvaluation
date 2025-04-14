using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Mensaging
{

    public class SaleCreatedEvent : INotification
    {
        public Guid SaleId { get; set; }
        public string? SaleNumber { get; set; }
        public string? Customer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
