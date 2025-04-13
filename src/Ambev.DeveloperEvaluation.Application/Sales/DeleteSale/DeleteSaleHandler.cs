using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand requests
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>
{
    private readonly ILogger<DeleteSaleHandler> _logger;
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    /// <summary>
    /// Initializes a new instance of DeleteSaleHandler
    /// </summary>
    /// <param name="SaleRepository">The sale repository</param>
    /// <param name="validator">The validator for DeleteSaleCommand</param>
    public DeleteSaleHandler(ISaleRepository SaleRepository, IMapper mapper, ILogger<DeleteSaleHandler> logger)
    {
        _saleRepository = SaleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the DeleteSaleCommand request
    /// </summary>
    /// <param name="command">The DeleteSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteSaleResponse> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando exclusão de venda: {@command}", command);
        var validator = new DeleteSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Falha na validação da venda: {@Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }
        // Remove os itens primeiro 
        var deleteItemsSale = await _saleRepository.DeleteSaleItemsAsync(command.Id);
        if (deleteItemsSale is null)
        {
            _logger.LogWarning("Sale items with ID {Id} not found", command.Id);
            throw new KeyNotFoundException($"Sale items with ID {command.Id} not found");
        }
        var deleteISale = await _saleRepository.DeleteAsync(command.Id, cancellationToken);
        if (deleteISale is null)
        {
            _logger.LogWarning("Sale with ID {Id} not found", command.Id);
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");
        }
        deleteISale.Items = deleteItemsSale;
        _logger.LogInformation("Venda excluida com sucesso. ID: {@Id}", command.Id);
        return _mapper.Map<DeleteSaleResponse>(deleteISale);
            
          
    }
}
