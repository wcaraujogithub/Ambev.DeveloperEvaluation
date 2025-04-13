using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler for processing ListSaleCommand requests
/// </summary>
public class ListSaleHandler : IRequestHandler<ListSalesQuery, PagedResult<ListSaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    /// <summary>
    /// Initializes a new instance of ListSaleHandler
    /// </summary>
    /// <param name="saleRepository">The Sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public ListSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the ListSalesQuery request
    /// </summary>
    /// <param name="request">The ListSales Query </param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>list sales if found</returns>
    public async Task<PagedResult<ListSaleResult>> Handle(ListSalesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando a consulta das vendas: {@Request}", request);
        var pagedSales = await _saleRepository.ListAsync(request._page, request._size, request._order, cancellationToken);

        if (!pagedSales.Items.Any())
        {
            _logger.LogWarning("a consulta não retornou nenhuma venda: {@Errors}", $"Sales not found");
            throw new KeyNotFoundException("Sales not found");
        }
        var mappedItems = _mapper.Map<List<ListSaleResult>>(pagedSales.Items);
        _logger.LogInformation("Consulta finalizada com sucesso.");
        return new PagedResult<ListSaleResult>
        {
            Items = mappedItems,
            TotalCount = pagedSales.TotalCount,
            CurrentPage = pagedSales.CurrentPage,
            PageSize = pagedSales.PageSize
        };
    }
}
