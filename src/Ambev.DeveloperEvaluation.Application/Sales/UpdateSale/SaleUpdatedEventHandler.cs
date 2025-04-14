using Ambev.DeveloperEvaluation.Domain.Mensaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class SaleUpdatedEventHandler : INotificationHandler<SaleUpdatedEvent>
    {
        private readonly ILogger<SaleUpdatedEventHandler> _logger;

        public SaleUpdatedEventHandler(ILogger<SaleUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SaleUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Evento SaleModified recebido: Venda {SaleId} - {SaleNumber}, Cliente {Customer}, Data {CreatedAt}",
                notification.SaleId,
                notification.SaleNumber,
                notification.Customer,
                notification.CreatedAt);

            // Aqui futuramente você pode publicar em um broker.
            return Task.CompletedTask;
        }
    }
}
