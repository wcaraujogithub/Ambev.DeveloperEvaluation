using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Application and API CreateSale responses
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale feature
    /// </summary>
    public CreateSaleProfile()
    {

        CreateMap<CreateSaleResult, CreateSaleResponse>();
        CreateMap<CreateSaleRequest, CreateSaleCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        // Mapeia os itens
        CreateMap<CreateSaleItemRequest, SaleItemDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Quantities, opt => opt.MapFrom(src => src.Quantities))
            .ForMember(dest => dest.UnitPrices, opt => opt.MapFrom(src => src.UnitPrices));

    }
}
