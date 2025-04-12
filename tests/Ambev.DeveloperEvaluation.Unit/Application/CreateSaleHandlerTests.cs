using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _repositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _repositoryMock = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CreateSaleProfile());
            });

            //_mapper = config.CreateMapper();
            _handler = new CreateSaleHandler(_repositoryMock, _mapper);
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

    }
}

