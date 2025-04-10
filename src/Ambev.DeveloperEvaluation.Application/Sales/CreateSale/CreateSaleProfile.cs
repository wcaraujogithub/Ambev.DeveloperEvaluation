using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateSale operation
        /// </summary>
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.SaleDate))
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => s.SaleDate))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Itens));
            CreateMap<Sale, CreateSaleResult>();

            // Mapeia os itens
            CreateMap<SaleItemDTO, SaleItem>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Quantities, opt => opt.MapFrom(src => src.Quantities))
                .ForMember(dest => dest.UnitPrices, opt => opt.MapFrom(src => src.UnitPrices))
                .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => src.Discounts))
                .ForMember(dest => dest.TotalValueItem, opt => opt.MapFrom(src => src.TotalItem));

        }
    }
}
