using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Profile for mapping between Sale entity and ListSaleResponse
/// </summary>
public class ListSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListSale operation
    /// </summary>
    public ListSaleProfile()
    {
        CreateMap<Sale, ListSaleResult>();
        CreateMap<SaleItem, ListSaleItemResult>();
    }
}
