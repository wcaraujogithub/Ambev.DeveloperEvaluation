using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Functional.Domain.Entities.TestData;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Repositories
{
    public class SaleRepositoryTests
    {
        private readonly DefaultContext _context;
        private readonly SaleRepository _repository;

        public SaleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DefaultContext(options);
            _repository = new SaleRepository(_context);

            _context.Database.EnsureCreated();
        }
     
        
        [Fact]
        public async Task ListAsync_Should_Return_Paged_Result_With_Default_Order()
        {
            await SeedSalesAsync();
            var result = await _repository.ListAsync(1, 3, "SaleNumber asc");

            Assert.NotNull(result);
            Assert.Equal(3, result.Items.Count);
            Assert.Equal(5, result.TotalCount);

            Assert.True(Convert.ToInt32(result.Items.First().SaleNumber) < Convert.ToInt32(result.Items.Last().SaleNumber));
        }

        [Fact]
        public async Task ListAsync_Should_Apply_Valid_Order()
        {
            await SeedSalesAsync();
            var result = await _repository.ListAsync(1, 5, "Customer desc");

            Assert.NotNull(result);
            Assert.Equal(5, result.Items.Count);
          
            // Verifica se está ordenado por Customer desc
            var ordered = result.Items.OrderByDescending(s => s.Customer).ToList();
            Assert.Equal(ordered, result.Items);
        }

        [Fact]
        public async Task ListAsync_Should_Ignore_Invalid_Order_And_Use_Default()
        {
            await SeedSalesAsync();
            var result = await _repository.ListAsync(1, 5, "InvalidColumn desc");

            Assert.NotNull(result);
            Assert.Equal(5, result.Items.Count);
        }

        [Fact]
        public async Task ListAsync_Should_Return_Paged_Results()
        {
            await CreateSaleWithItemsAsync();
            var result = await _repository.ListAsync(1, 10, "SaleNumber asc");

            Assert.NotNull(result);
            Assert.True(result.TotalCount > 0);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task ListAsync_Should_Ignore_Invalid_Ordering()
        {
            await CreateSaleWithItemsAsync();
            var result = await _repository.ListAsync(1, 10, "InvalidColumn desc");

            Assert.NotNull(result);
            Assert.True(result.Items.Count > 0);
        }

        private async Task<Sale> CreateSaleWithItemsAsync()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Items = [SaleTestData.GenerateValidSaleItem()];
            await _repository.CreateAsync(sale);
            return sale;
        }

        private async Task SeedSalesAsync()
        {
            for (int i = 1; i <= 5; i++)
            {
                await _repository.CreateAsync(SaleTestData.GenerateValidSale());
            }
        }

    }
}
