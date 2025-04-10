using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="SaleRepository">The Sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for CreateSaleCommand</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = _mapper.Map<Sale>(command);
        sale.Id = Guid.NewGuid();

        decimal total = 0;
        foreach (var prod in sale.Items)
        {
            prod.Discounts = CalcularDesconto(prod.Quantities, prod.UnitPrices);
            prod.TotalValueItem = prod.UnitPrices * prod.Quantities - prod.Discounts;
            total += prod.TotalValueItem;
        }
        sale.TotalValue = total;
        sale.CreatedAt = DateTime.UtcNow;
        sale.UpdatedAt = DateTime.UtcNow;

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(createdSale);       
        return result;
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
