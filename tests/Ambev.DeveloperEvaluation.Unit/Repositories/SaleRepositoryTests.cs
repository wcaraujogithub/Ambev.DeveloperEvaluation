using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Repositories
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
        public async Task CreateAsync_Should_Add_Sale()
        {
            var sale = new Sale { Id = Guid.NewGuid(), SaleNumber = "002", CreatedAt = DateTime.UtcNow };
            var result = await _repository.CreateAsync(sale);

            Assert.NotNull(result);
            Assert.Equal(sale.SaleNumber, result.SaleNumber);
        }
       
        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Sale()
        {
            var sale = await CreateSaleWithItemsAsync();
            var result = await _repository.GetByIdAsync(sale.Id);

            Assert.NotNull(result);
            Assert.Equal(sale.Id, result.Id);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_NotFound()
        {
            var result = await _repository.GetByIdAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Sale()
        {
            var sale = await CreateSaleWithItemsAsync();
            var deleted = await _repository.DeleteAsync(sale.Id);
            var result = await _repository.GetByIdAsync(sale.Id);

            Assert.NotNull(deleted);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteSaleItemsAsync_Should_Remove_Items()
        {
            var sale = await CreateSaleWithItemsAsync();
            var deleted = await _repository.DeleteSaleItemsAsync(sale.Id);
            var updatedSale = await _repository.GetByIdAsync(sale.Id);

            Assert.NotNull(deleted);
            Assert.Empty(updatedSale.Items);
        }
       
        [Fact]
        public async Task DeleteAsync_Should_Return_False_If_NotFound()
        {
            var deleted = await _repository.DeleteAsync(Guid.NewGuid());
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteSaleItemsAsync_Should_Return_False_If_NoItems()
        {
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = "99",
                CreatedAt = DateTime.UtcNow,
                Items = new List<SaleItem>()
            };
            await _repository.CreateAsync(sale);
            var deleted = await _repository.DeleteSaleItemsAsync(sale.Id);

            Assert.Null(deleted);
        }
      
        [Fact]
        public async Task UpdateAsync_Should_Modify_Fields()
        {
            var sale = await CreateSaleWithItemsAsync();
            sale.Customer = "Updated";
            var updated = await _repository.UpdateAsync(sale);
            var result = await _repository.GetByIdAsync(sale.Id);

            Assert.NotNull(updated);
            Assert.Equal("Updated", result.Customer);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_False_If_NotFound()
        {
            var sale = new Sale { Id = Guid.NewGuid(), SaleNumber = "001" };
            var updated = await _repository.UpdateAsync(sale);

            Assert.Null(updated);
        }

        private async Task<Sale> CreateSaleWithItemsAsync()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Items = [SaleTestData.GenerateValidSaleItem()];
            await _repository.CreateAsync(sale);
            return sale;
        }

    }
}
