using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
    .RuleFor(u => u.Id, f => f.Random.Guid())
    .RuleFor(u => u.SaleNumber, f => f.Random.Number(1, 999).ToString())
    .RuleFor(u => u.Customer, f => f.Internet.UserName())
    .RuleFor(u => u.TotalValue, f => f.Random.Number())
    .RuleFor(u => u.Branch, f => f.Company.CompanyName())
    .RuleFor(u => u.Cancelled, f => f.PickRandomParam(new bool[] { true, true, false }))
    .RuleFor(u => u.CreatedAt, f => f.Date.Recent(100))
    .RuleFor(u => u.UpdatedAt, f => f.Date.Recent(100));




        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
    .RuleFor(u => u.Id, f => f.Random.Guid())
    .RuleFor(u => u.ProductId, f => f.Random.Guid())
    .RuleFor(u => u.SaleId, f => f.Random.Guid())
    .RuleFor(u => u.Quantities, f => f.Random.Number())
    .RuleFor(u => u.UnitPrices, f => f.Random.Decimal())
    .RuleFor(u => u.Discounts, f => f.Random.Decimal())
    .RuleFor(u => u.TotalValueItem, f => f.Random.Decimal());


        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }
        public static SaleItem GenerateValidSaleItem()
        {
            return SaleItemFaker.Generate();
        }


    }
}
