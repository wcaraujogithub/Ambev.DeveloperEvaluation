using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// Profile for mapping ListSale feature requests to commands
/// </summary>
public class ListSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListSale feature
    /// </summary>
    public ListSaleProfile()
    {
        CreateMap<ListSaleResult, ListSaleResponse>();
        CreateMap<ListSaleItemResult, ListSaleItemResponse>();
    }
}
