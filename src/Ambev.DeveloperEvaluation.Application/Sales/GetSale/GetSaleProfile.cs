using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Profile for mapping between Sale entity and GetSaleResponse
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale operation
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Sale, GetByIdSaleQueryResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        // Mapeia os itens
        CreateMap<SaleItem, GetSaleItemResult>();
    }
}
