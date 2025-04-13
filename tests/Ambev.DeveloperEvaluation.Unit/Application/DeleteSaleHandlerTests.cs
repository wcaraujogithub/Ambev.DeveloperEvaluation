using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;


namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteSaleHandlerTests
    {
        private readonly ILogger<DeleteSaleHandler> _loggerMock;
        private readonly ISaleRepository _repositoryMock;
        private readonly DeleteSaleHandler _handler;
        private readonly IMapper _mapper;
        public DeleteSaleHandlerTests()
        {
            _repositoryMock = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _loggerMock = Substitute.For<ILogger<DeleteSaleHandler>>();
            //_mapper = config.CreateMapper();
            _handler = new DeleteSaleHandler(_repositoryMock,_mapper, _loggerMock);
        }

        [Fact]
        public async Task Handle_Should_Delete_Sale_When_Exists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var listItemSaleRemoved = new List<SaleItem>() { SaleTestData.GenerateValidSaleItem() };
            var saleRemoved = new Sale
            {
                Id = saleId,
                Branch = "Cliente Original",
                Customer = "Filial Original",
                Items = listItemSaleRemoved
            };
            var command = new DeleteSaleCommand(saleId);

            var deleteSaleResponseReturns = new DeleteSaleResponse()
            {
                Id = saleRemoved.Id,
                Branch = saleRemoved.Branch,
                Customer = saleRemoved.Customer,
                Cancelled = saleRemoved.Cancelled,
                CreatedAt = saleRemoved.CreatedAt,
                SaleNumber = saleRemoved.SaleNumber,
                TotalValue = saleRemoved.TotalValue.ToString(),
                UpdatedAt = saleRemoved.UpdatedAt            
            };

            foreach (var item in saleRemoved.Items)
            {
                deleteSaleResponseReturns?.Items?.Add(new DeleteSaleItemResult
                {
                    Name = item.Name,
                    UnitPrices = item.UnitPrices,
                    Quantities = item.Quantities
                });
            }

            _mapper.Map<DeleteSaleResponse>(saleRemoved).Returns(deleteSaleResponseReturns);

            _repositoryMock.DeleteSaleItemsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(listItemSaleRemoved);

            _repositoryMock.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saleRemoved);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
        
            deleteSaleResponseReturns.Should().NotBeNull();
            deleteSaleResponseReturns?.Id.Should().Be(saleRemoved.Id);
            await _repositoryMock.Received(1).DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_When_SaleItem_Does_Not_Exist()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var saleRemoved = new Sale
            {
                Id = saleId,
                Branch = "Cliente Original",
                Customer = "Filial Original"
            };
            var command = new DeleteSaleCommand(saleId);
            var listItemSaleRemoved = new List<SaleItem>() { SaleTestData.GenerateValidSaleItem() };

            _repositoryMock.DeleteSaleItemsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

            _repositoryMock.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saleRemoved);

            // Act
            var result = async () => await _handler.Handle(command, CancellationToken.None);


            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(result);
            exception.Message.Should().Be($"Sale items with ID {command.Id} not found");

        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Sale_Does_Not_Exist()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var saleRemoved = new Sale
            {
                Id = saleId,
                Branch = "Cliente Original",
                Customer = "Filial Original"
            };
            var command = new DeleteSaleCommand(saleId);
            var listItemSaleRemoved = new List<SaleItem>() { SaleTestData.GenerateValidSaleItem() };

            _repositoryMock.DeleteSaleItemsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(listItemSaleRemoved);

            _repositoryMock.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

            // Act
            var result = async() => await _handler.Handle(command, CancellationToken.None);

           
            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(result);
            exception.Message.Should().Be($"Sale with ID {command.Id} not found");

        }        
    }
}
