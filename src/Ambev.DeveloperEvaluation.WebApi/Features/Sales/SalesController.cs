using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing Sale operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private ILogger<SalesController> _logger;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SalesController(IMediator mediator, IMapper mapper, ILogger<SalesController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new Sale
    /// </summary>
    /// <param name="request">The Sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created Sale details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando requisição name: {@name} - request: {@request}", nameof(CreateSale), request);
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Falha na validação da venda: {@Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<CreateSaleResponse>(response);

        _logger.LogInformation("Requisição name: {@name} finalizada com sucesso. ID: {@id}", nameof(CreateSale), response.Id);
        return Created("GetByIdSale", new { id = result.Id }, result);
    }


    /// <summary>
    /// Deletes a Sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the Sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the Sale was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando requisição name: {@name} - ID: {@id}", nameof(DeleteSale), id);
        var request = new DeleteSaleRequest { Id = id };
        var validator = new DeleteSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<DeleteSaleCommand>(request.Id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result is null)
        {
            _logger.LogInformation("Requisição name: {@name} finalizada com falha. ID: {@id}", nameof(DeleteSale), id);
            return BadRequest(result);
        }

        _logger.LogInformation("Requisição name: {@name} finalizada com sucesso. ID: {@id}", nameof(DeleteSale), id);
        return Ok(result);


    }


    /// <summary>
    /// Retrieves a Sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the Sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The Sale details if found</returns>
    [HttpGet("{id}", Name = "GetByIdSale")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando requisição name: {@name} - ID: {@id}", nameof(GetByIdSale), id);
        var request = new GetSaleRequest { Id = id };
        var validator = new GetSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetByIdSaleQuery>(request.Id);
        var response = await _mediator.Send(command, cancellationToken);

        var result = _mapper.Map<GetSaleResponse>(response);

        if (result is null)
        {
            _logger.LogInformation("Requisição name: {@name} não localizada. ID: {@id}", nameof(GetByIdSale), id);
            return NotFound(result);
        }
        else
        {
            _logger.LogInformation("Requisição name: {@name} finalizada com sucesso. ID: {@id}", nameof(GetByIdSale), id);
            return Ok(result);

        }


    }

    /// <summary>
    /// List the Sales
    /// </summary>  
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List sales if found</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ListSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListSale([FromQuery] ListSalesQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando requisição name: {@name} - request: {@query}", nameof(ListSale), query);
        var pagedResult = await _mediator.Send(query, cancellationToken: cancellationToken);

        var mapped = _mapper.Map<List<ListSaleResponse>>(pagedResult.Items);

        var paginated = new PaginatedList<ListSaleResponse>(
            mapped,
            pagedResult.TotalCount,
            pagedResult.CurrentPage,
            pagedResult.PageSize
            );
        _logger.LogInformation("Requisição name: {@name} finalizada com sucesso. request: {@query}", nameof(ListSale), query);
        return OkPaginated(paginated);

    }

    /// <summary>
    /// Update a Sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the Sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The Sale details if found</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleRequest>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale([FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando requisição name: {@name} - request: {@request}", nameof(UpdateSale), request);
        var validator = new UpdateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        var result = _mapper.Map<UpdateSaleResponse>(response);

        if (result is null)
        {
            _logger.LogInformation("Requisição name: {@name} finalizada com sucesso. request: {@request}", nameof(ListSale), request);
            return BadRequest(result);
        }

        _logger.LogInformation("Requisição name: {@name} finalizada com falha. request: {@request}", nameof(ListSale), request);
        return Ok(result);

    }
}
