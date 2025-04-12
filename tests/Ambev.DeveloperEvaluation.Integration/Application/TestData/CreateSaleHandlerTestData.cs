using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Application.TestData
{
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand>
                createSaleHandlerFaker = new Faker<CreateSaleCommand>()
        .RuleFor(u => u.SaleNumber, f => f.Random.Number(1, 999).ToString())
        .RuleFor(u => u.Customer, f => f.Internet.UserName())
        .RuleFor(u => u.Branch, f => f.Company.CompanyName())
            .RuleFor(u => u.Items, f => new List<SaleItemDTO>
            {
                createSaleItemHandlerFaker.Generate()
            });

        private static readonly Faker<SaleItemDTO>
                createSaleItemHandlerFaker = new Faker<SaleItemDTO>()
        .RuleFor(u => u.Name, f => f.Commerce.ProductName())
        .RuleFor(u => u.Quantities, f => f.Random.Number(1, 20))
        .RuleFor(u => u.UnitPrices, f => f.Random.Decimal());


        public static CreateSaleCommand GenerateValidSaleCommand()
        {
            return createSaleHandlerFaker.Generate();
        }
        public static SaleItemDTO GenerateValidSaleItemCommand()
        {
            return createSaleItemHandlerFaker.Generate();
        }
    }
}
