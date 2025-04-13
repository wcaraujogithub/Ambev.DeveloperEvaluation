using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetByIdSaleHandlerTests
    {
        private readonly ILogger<GetSaleHandler> _loggerMock;
        private readonly ISaleRepository _repositoryMock;
        private readonly GetSaleHandler _handler;
        private readonly IMapper _mapper;
        public GetByIdSaleHandlerTests()
        {
            _repositoryMock = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _loggerMock = Substitute.For<ILogger<GetSaleHandler>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GetSaleProfile());
            });
            //_mapper = config.CreateMapper();
            _handler = new GetSaleHandler(_repositoryMock, _mapper, _loggerMock);
        }


        [Fact]
        public async Task Handle_Should_GetById_Sale_When_Exists()
        {
            // Arrange
            var saleRetuns = SaleTestData.GenerateValidSale();
            var command = new GetByIdSaleQuery(saleRetuns.Id);

            var getByIdSaleQueryResult = new GetByIdSaleQueryResult() { Id = saleRetuns.Id };

            _mapper.Map<GetByIdSaleQueryResult>(saleRetuns).Returns(getByIdSaleQueryResult);

            _repositoryMock.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saleRetuns);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(saleRetuns.Id);
            await _repositoryMock.Received(1).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
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
            var command = new GetByIdSaleQuery(saleId);
            var listItemSaleRemoved = new List<SaleItem>() { SaleTestData.GenerateValidSaleItem() };

            _repositoryMock.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Sale?)null);

            // Act
            var result = async () => await _handler.Handle(command, CancellationToken.None);


            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(result);
            exception.Message.Should().Be($"Sale with ID {command.Id} not found");

        }
      
        [Fact]
        public async Task GetSaleByIdHandler_Should_Return_Sale_When_Found()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var resultMapped = new GetByIdSaleQueryResult { Id = sale.Id, Customer = sale.Customer };

            _repositoryMock.GetByIdAsync(sale.Id).Returns(sale);
            _mapper.Map<GetByIdSaleQueryResult>(sale).Returns(resultMapped);

         
            var query = new GetByIdSaleQuery(sale.Id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
            result.Customer.Should().Be(sale.Customer);
        }
    }
}
