using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class DeleteSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateSale operation
        /// </summary>
        public DeleteSaleProfile()
        {
          
            CreateMap<Sale, DeleteSaleResponse>()
                       .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
            // Mapeia os itens
            CreateMap<SaleItem, DeleteSaleItemResult>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Quantities, opt => opt.MapFrom(src => src.Quantities))
                .ForMember(dest => dest.UnitPrices, opt => opt.MapFrom(src => src.UnitPrices));         

        }
    }
}
