using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Profile for mapping GetSale feature requests to commands
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale feature
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Guid, GetByIdSaleQuery>()
            .ConstructUsing(id => new GetByIdSaleQuery(id));

        CreateMap<GetByIdSaleQueryResult, GetSaleResponse>()
          .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Items));

        // Mapeia os itens
        CreateMap<GetSaleItemResult, GetSaleItemResponse>();   
    }
}
