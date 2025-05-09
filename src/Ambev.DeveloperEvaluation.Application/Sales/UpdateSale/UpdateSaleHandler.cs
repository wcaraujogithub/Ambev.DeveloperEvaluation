﻿using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Mensaging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly IMediator _mediator;
    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="SaleRepository">The Sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for UpdateSaleCommand</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<UpdateSaleHandler> logger, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The Updated sale details</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando atualização de venda: {@command}", command);
        var validator = new UpdateSaleCommandValidator();

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Falha na validação de atualização da venda: {@Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }
        var sale = _mapper.Map<Sale>(command);

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        if (updatedSale is null)
        {
            _logger.LogWarning("Sale with ID {Id} not found", command.Id);
            throw new KeyNotFoundException($"Sale com ID {command.Id} not found.");
        }

        _logger.LogInformation("Venda atualizada com sucesso. ID: {@Id}", command.Id);

        // Publica o evento
        await _mediator.Publish(new SaleUpdatedEvent
        {
            SaleId = updatedSale.Id,
            SaleNumber = updatedSale.SaleNumber,
            Customer = updatedSale.Customer,
            CreatedAt = updatedSale.CreatedAt
        }, cancellationToken);


        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}
