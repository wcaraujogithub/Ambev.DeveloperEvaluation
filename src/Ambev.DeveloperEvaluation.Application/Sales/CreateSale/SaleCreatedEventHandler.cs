using Ambev.DeveloperEvaluation.Domain.Mensaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
    {
        private readonly ILogger<SaleCreatedEventHandler> _logger;

        public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Evento SaleCreated recebido: Venda {SaleNumber}, Cliente {Customer}, Data {CreatedAt}",
                notification.SaleNumber,
                notification.Customer,
                notification.CreatedAt);

            // Aqui futuramente você pode publicar em um broker.
            return Task.CompletedTask;
        }
    }
}
