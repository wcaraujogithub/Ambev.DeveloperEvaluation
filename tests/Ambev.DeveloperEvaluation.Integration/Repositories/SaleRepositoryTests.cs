using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Integration.Domain.Entities.TestData;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Integration.Repositories
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
        public async Task Full_Crud_Flow_Should_Work()
        {
            // CREATE
            var sale = SaleTestData.GenerateValidSale();
            sale.Items = [SaleTestData.GenerateValidSaleItem()];

            var created = await _repository.CreateAsync(sale);
            Assert.NotNull(created);

            // READ
            var retrieved = await _repository.GetByIdAsync(created.Id);
            Assert.NotNull(retrieved);
            Assert.Single(retrieved.Items);

            // UPDATE
            retrieved.Customer = "Updated Customer";
            var updated = await _repository.UpdateAsync(retrieved);
            Assert.NotNull(updated);
            var checkUpdate = await _repository.GetByIdAsync(retrieved.Id);
            Assert.Equal("Updated Customer", checkUpdate.Customer);

            // DELETE ITEMS
            var deletedItems = await _repository.DeleteSaleItemsAsync(retrieved.Id);
            Assert.NotNull(deletedItems);
            var afterDeleteItems = await _repository.GetByIdAsync(retrieved.Id);
            Assert.Empty(afterDeleteItems.Items);

            // DELETE SALE
            var deletedSale = await _repository.DeleteAsync(retrieved.Id);
            Assert.NotNull(deletedSale);
            var afterDeleteSale = await _repository.GetByIdAsync(retrieved.Id);
            Assert.Null(afterDeleteSale);
        }

        [Fact]
        public async Task Pagination_And_Order_Should_Work_Together()
        {
            for (int i = 1; i <= 10; i++)
            {
                await _repository.CreateAsync(SaleTestData.GenerateValidSale());
            }

            var result = await _repository.ListAsync(2, 3, "SaleNumber desc");

            Assert.NotNull(result);
            Assert.Equal(3, result.Items.Count);
            Assert.Equal(10, result.TotalCount);

            var expectedOrder = result.Items.OrderByDescending(s => s.SaleNumber).ToList();
            Assert.Equal(expectedOrder, result.Items);
        }

        [Fact]
        public async Task Handle_Should_Persist_Sale_And_Return_Result_When_Command_Is_Valid()
        {
            // Arrange  

            // Usa repositório real
            var repository = new SaleRepository(_context);

            // Mapper real
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CreateSaleProfile>();
            }).CreateMapper();

            var handler = new CreateSaleHandler(repository, mapper);

            var command = new CreateSaleCommand
            {
                SaleNumber = "001",
                Customer = "Client X",
                Branch = "XPTO",
                Items = new List<SaleItemDTO>
        {
            new SaleItemDTO
            {
                Name = "Produto X",
                UnitPrices = 100,
                Quantities = 2
            }
        }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();

            var savedSale = await _context.Sales.Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == result.Id);

            savedSale.Should().NotBeNull();
            savedSale.Customer.Should().Be("Client X");
            savedSale.Items.Should().HaveCount(1);
            savedSale.Items.First().Name.Should().Be("Produto X");
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
