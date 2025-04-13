using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Common;
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
    public class ListSaleHandlerTests
    {
        private readonly ISaleRepository _repositoryMock;
        private readonly ListSaleHandler _handler;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSaleHandler> _loggerMock;
        public ListSaleHandlerTests()
        {
            _repositoryMock = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _loggerMock = Substitute.For<ILogger<CreateSaleHandler>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ListSaleProfile());
            });
            //_mapper = config.CreateMapper();
            _handler = new ListSaleHandler(_repositoryMock, _mapper, _loggerMock);
        }

        [Fact]
        public async Task Handle_Should_GetById_Sales_When_Exists()
        {
            // Arrange
            var pagedSales = new PagedResult<Sale> { CurrentPage = 1, Items = new List<Sale>() { SaleTestData.GenerateValidSale() } };
            var command = new ListSalesQuery();

            var listSaleResult = new List<ListSaleResult>() { };

            _mapper.Map<List<ListSaleResult>>(pagedSales.Items).Returns(listSaleResult);

            _repositoryMock.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(pagedSales);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            await _repositoryMock.Received(1).ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_GetById_Sales_When_order_customer_asc()
        {
            var sales = SaleTestData.GenerateValidListSale(10);
            // Arrange
            var pagedSales = new PagedResult<Sale>
            {
                CurrentPage = 1,
                PageSize = 10,
                Items = sales

            };
            var command = new ListSalesQuery() { _order = "customer asc" };

            var listSaleResult = sales.Select(s => new ListSaleResult
            {
                Id = s.Id,
                Customer = s.Customer,
                Branch = s.Branch,
                TotalValue = s.TotalValue.ToString()
            }).ToList();

            _mapper.Map<List<ListSaleResult>>(sales).Returns(listSaleResult);

            _repositoryMock.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(pagedSales);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert 
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(10);
            result.Items.Should().BeEquivalentTo(listSaleResult);

            await _repositoryMock.Received(1).ListAsync(
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Is<string>(s => s == "customer asc"),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Sales_Does_Not_Exist()
        {
            // Arrange
            var pagedSales = new PagedResult<Sale>
            {
                CurrentPage = 1,
                PageSize = 10,
                Items = new List<Sale>()
            };
            var command = new ListSalesQuery { _order = "customer asc" };

            _mapper.Map<List<ListSaleResult>>(pagedSales.Items).Returns(new List<ListSaleResult>());

            _repositoryMock.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(pagedSales);

            // Act
            var result = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(result);
            exception.Message.Should().Be($"Sales not found");
        }

        [Fact]
        public async Task ListSalesQueryHandler_Should_Handle_Null_Or_Empty_Order()
        {
            // Arrange
            var sales = SaleTestData.GenerateValidListSale(5);
            var pagedSales = new PagedResult<Sale>
            {
                CurrentPage = 1,
                PageSize = 10,
                Items = sales
            };

            var command = new ListSalesQuery(); // _order nulo

            var listSaleResult = sales.Select(s => new ListSaleResult
            {
                Id = s.Id,
                Customer = s.Customer,
                Branch = s.Branch,
                TotalValue = s.TotalValue.ToString()
            }).ToList();

            _mapper.Map<List<ListSaleResult>>(sales).Returns(listSaleResult);
            _repositoryMock.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(pagedSales);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(5);
            await _repositoryMock.Received(1).ListAsync(
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ListSalesQueryHandler_Should_Return_Correct_Page_Of_Sales()
        {
            // Arrange
            var sales = SaleTestData.GenerateValidListSale(5);
            var pagedSales = new PagedResult<Sale>
            {
                CurrentPage = 2,
                PageSize = 5,
                Items = sales
            };

            var command = new ListSalesQuery { _page = 2, _size = 5 };

            var listSaleResult = sales.Select(s => new ListSaleResult
            {
                Id = s.Id,
                Customer = s.Customer,
                Branch = s.Branch,
                TotalValue = s.TotalValue.ToString()
            }).ToList();

            _mapper.Map<List<ListSaleResult>>(sales).Returns(listSaleResult);
            _repositoryMock.ListAsync(2, 5, Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(pagedSales);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(5);
            await _repositoryMock.Received(1).ListAsync(2, 5, Arg.Any<string>(), Arg.Any<CancellationToken>());
        }
    }
}
