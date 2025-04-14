using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Mensaging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="SaleRepository">The Sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for CreateSaleCommand</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<CreateSaleHandler> logger, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando inclusão de venda: {@command}", command);
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Falha na validação da inclusão da venda: {@Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }

        var sale = _mapper.Map<Sale>(command);

        decimal total = 0;
        foreach (var prod in sale.Items)
        {
            _logger.LogInformation("Realizando os calculos do item. ID: {@ProdctId}", prod.ProductId);
            prod.Discounts = CalcularDesconto(prod.Quantities, prod.UnitPrices);
            prod.TotalValueItem = prod.UnitPrices * prod.Quantities - prod.Discounts;
            total += prod.TotalValueItem;
        }
        sale.TotalValue = total;
        sale.CreatedAt = DateTime.UtcNow;
        sale.UpdatedAt = DateTime.UtcNow;

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
    
        _logger.LogInformation("Venda criada com sucesso. ID: {@SaleId}", sale.Id);

        // Publica o evento
        await _mediator.Publish(new SaleCreatedEvent
        {
            SaleId = createdSale.Id,
            SaleNumber = createdSale.SaleNumber,
            Customer = createdSale.Customer,
            CreatedAt = createdSale.CreatedAt
        }, cancellationToken);
              
        return _mapper.Map<CreateSaleResult>(createdSale);
    }

    private decimal CalcularDesconto(int quantidade, decimal precoUnitario)
    {
        if (quantidade < 4)
            return 0;

        if (quantidade > 20)
            throw new ValidationException("Não é possível vender mais de 20 itens do mesmo produto.");


        if (quantidade >= 10)
            return precoUnitario * quantidade * 0.20m;

        if (quantidade >= 4)
            return precoUnitario * quantidade * 0.10m;

        return 0;
    }
}
