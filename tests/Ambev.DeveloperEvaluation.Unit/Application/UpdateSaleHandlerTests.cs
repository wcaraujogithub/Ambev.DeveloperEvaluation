using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Mensaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;



namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class UpdateSaleHandlerTests
    {
        private readonly IMediator _mediatorMock;
        private readonly ISaleRepository _repositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateSaleHandler _handler;
        private readonly ILogger<UpdateSaleHandler> _loggerMock;
        public UpdateSaleHandlerTests()
        {
            _repositoryMock = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _mediatorMock = Substitute.For<IMediator>();
            _loggerMock = Substitute.For<ILogger<UpdateSaleHandler>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UpdateSaleProfile());
            });

            //_mapper = config.CreateMapper();
            _handler = new UpdateSaleHandler(_repositoryMock, _mapper, _loggerMock, _mediatorMock);
        }

        [Fact]
        public async Task Handle_Should_Update_Sale_When_Exists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var existingSale = new Sale
            {
                Id = saleId,
                Branch = "Cliente Original",
                Customer = "Filial Original"
            };
            var command = new UpdateSaleCommand
            {
                Id = saleId,
                Customer = "Cliente Atualizado",
                Branch = "Filial Atualizada"
            };

            var expectedDto = new Sale
            {
                Id = saleId,
                Customer = "Cliente Atualizado",
                Branch = "Filial Atualizada"
            };

            _repositoryMock.GetByIdAsync(saleId).Returns(existingSale);
            _repositoryMock.UpdateAsync(Arg.Any<Sale?>(), Arg.Any<CancellationToken>())
                .Returns(expectedDto);

            _mapper.Map<Sale>(Arg.Any<Sale>()).Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
  
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Sale_Does_Not_Exist()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(), // não existe
                Customer = "Cliente Teste",
                Branch = "Filial Teste"

            };
            var saleReturns = new Sale
            {
                Id = command.Id,
                Branch = command.Branch,
                Customer = command.Customer
            };

            var updateSaleResultReturns = new UpdateSaleResult()
            {
                Id = saleReturns.Id,
                Customer = "Cliente Atualizado",
                Branch = "Filial Atualizada"
            };

            _mapper.Map<Sale>(command).Returns(saleReturns);

            _mapper.Map<UpdateSaleResult>(saleReturns).Returns(updateSaleResultReturns);

            _repositoryMock.UpdateAsync(Arg.Any<Sale?>(), Arg.Any<CancellationToken>())
            .Returns(saleReturns);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(saleReturns.Id);
            result.Customer.Should().Be("Cliente Atualizado");
            result.Branch.Should().Be("Filial Atualizada");
        }

        [Fact]
        public async Task Handle_Should_Publish_SaleCreatedEvent_When_Sale_Is_Created()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(), 
                Customer = "Cliente Teste",
                Branch = "Filial Teste"

            };
            var sale = SaleTestData.GenerateValidSale();
            var saleResult = new UpdateSaleResult
            {
                Id = sale.Id,
                Branch = sale.Branch,
                Cancelled = sale.Cancelled,
                Customer = sale.Customer,
                SaleNumber = sale.SaleNumber,
                TotalValue = sale.TotalValue.ToString(),
                UpdatedAt = sale.UpdatedAt
            };

            _mapper.Map<Sale>(command).Returns(sale);
            _mapper.Map<UpdateSaleResult>(sale).Returns(saleResult);

            _repositoryMock.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                           .Returns(sale);

     

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _mediatorMock.Received(1).Publish(Arg.Is<SaleUpdatedEvent>(e =>
                e.SaleId == sale.Id &&
                e.SaleNumber == sale.SaleNumber &&
                e.Customer == sale.Customer
            ), Arg.Any<CancellationToken>());

            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
        }
    }
}

