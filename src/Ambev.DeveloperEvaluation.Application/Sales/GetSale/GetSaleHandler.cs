using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleCommand requests
/// </summary>
public class GetSaleHandler : IRequestHandler<GetByIdSaleQuery, GetByIdSaleQueryResult>
{
    private readonly ILogger<GetSaleHandler> _logger;
    private readonly ISaleRepository _SaleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleHandler
    /// </summary>
    /// <param name="SaleRepository">The Sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for GetSaleCommand</param>
    public GetSaleHandler(
        ISaleRepository SaleRepository,
        IMapper mapper,
        ILogger<GetSaleHandler> logger)
    {
        _SaleRepository = SaleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetSaleCommand request
    /// </summary>
    /// <param name="query">The GetSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The Sale details if found</returns>
    public async Task<GetByIdSaleQueryResult> Handle(GetByIdSaleQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando consulta de venda por id: {@query}", query);
        var validator = new GetByIdSaleValidator();
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Falha na validação da venda: {@Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }
        var Sale = await _SaleRepository.GetByIdAsync(query.Id, cancellationToken);
        if (Sale == null)
        {
            _logger.LogWarning("Sale with ID {@Id} not found", query.Id);
            throw new KeyNotFoundException($"Sale with ID {query.Id} not found");
        }
        return _mapper.Map<GetByIdSaleQueryResult>(Sale);
    }
}
