using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Mensaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _repositoryMock;
        private readonly IMapper _mapper;
        private readonly IMediator _mediatorMock;
        private readonly CreateSaleHandler _handler;
        private readonly ILogger<CreateSaleHandler> _loggerMock;
        public CreateSaleHandlerTests()
        {
            _repositoryMock = Substitute.For<ISaleRepository>();
            _mediatorMock = Substitute.For<IMediator>();
            _mapper = Substitute.For<IMapper>();
            _loggerMock = Substitute.For<ILogger<CreateSaleHandler>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CreateSaleProfile());
            });

            //_mapper = config.CreateMapper();
            _handler = new CreateSaleHandler(_repositoryMock, _mapper, _loggerMock, _mediatorMock);
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {

            // Given
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            //command.Items = [CreateSaleHandlerTestData.GenerateValidSaleItemCommand()];

            Sale? saleReturns = new Sale
            {
                Id = command.Id,
                Branch = command.Branch,
                SaleNumber = command.SaleNumber,
                Customer = command.Customer
            };

            saleReturns.Items = new List<SaleItem>();

            foreach (var item in command.Items)
            {
                saleReturns.Items.Add(new SaleItem
                {
                    Name = item.Name,
                    UnitPrices = item.UnitPrices,
                    Quantities = item.Quantities
                });
            }
            var createSaleResultReturns = new CreateSaleResult() { Id = saleReturns.Id };

            _mapper.Map<Sale>(command).Returns(saleReturns);
            _mapper.Map<CreateSaleResult>(saleReturns).Returns(createSaleResultReturns);


            _repositoryMock.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(saleReturns);

            // When
            var createSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Then
            createSaleResult.Should().NotBeNull();
            createSaleResult.Id.Should().Be(saleReturns.Id);
            await _repositoryMock.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an invalid user creation request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Given
            var command = new CreateSaleCommand(); // Empty command will fail validation

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        /// <summary>
        /// Tests that the mapper is called with the correct command.
        /// </summary>
        [Fact(DisplayName = "Given valid command When handling Then maps command to sale entity")]
        public async Task Handle_ValidRequest_MapsCommandToSale()
        {
            // Given
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            //   command.Items = [CreateSaleHandlerTestData.GenerateValidSaleItemCommand()];


            Sale? saleReturns = new Sale
            {
                Id = command.Id,
                Branch = command.Branch,
                SaleNumber = command.SaleNumber,
                Customer = command.Customer
            };

            saleReturns.Items = new List<SaleItem>();

            foreach (var item in command.Items)
            {
                saleReturns.Items.Add(new SaleItem
                {
                    Name = item.Name,
                    UnitPrices = item.UnitPrices,
                    Quantities = item.Quantities
                });
            }

            _mapper.Map<Sale>(command).Returns(saleReturns);


            _repositoryMock.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(saleReturns);


            // When
            await _handler.Handle(command, CancellationToken.None);

            // Then
            _mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
                c.Branch == command.Branch &&
                c.Customer == command.Customer &&
                c.Items == command.Items));
        }

        [Fact]
        public async Task CreateSaleHandler_Should_Throw_ValidationException_When_Command_Is_Invalid()
        {
            // Arrange
            var command = new CreateSaleCommand(); // campos obrigatórios faltando


            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);


            // Assert
            var exception = await act.Should().ThrowAsync<ValidationException>();

            // Verificando se a exceção contém erros esperados
            exception.Which.Errors.Should().Contain(e =>
                e.PropertyName == "SaleNumber" &&
                e.ErrorMessage.Contains("required."));
        }

        [Fact]
        public async Task Handle_Should_Publish_SaleCreatedEvent_When_Sale_Is_Created()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            var sale = SaleTestData.GenerateValidSale();
            var saleResult = new CreateSaleResult { Id = sale.Id, Success =true };

            _mapper.Map<Sale>(command).Returns(sale);
            _mapper.Map<CreateSaleResult>(sale).Returns(saleResult);

            _repositoryMock.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                           .Returns(sale);

            var handler = new CreateSaleHandler(_repositoryMock, _mapper, _loggerMock, _mediatorMock);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            await _mediatorMock.Received(1).Publish(Arg.Is<SaleCreatedEvent>(e =>
                e.SaleId == sale.Id &&
                e.SaleNumber == sale.SaleNumber &&
                e.Customer == sale.Customer
            ), Arg.Any<CancellationToken>());

            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
        }
    }
}

