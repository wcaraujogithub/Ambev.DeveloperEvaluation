using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    public class ListSalesQuery : IRequest<PagedResult<ListSaleResult>>
    {
        public int _page { get; set; } = 1;
        public int _size { get; set; } = 10;
        public string? _order { get; set; } = null;
    }
}
