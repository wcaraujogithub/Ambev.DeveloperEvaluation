using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Common.Constants;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validation
{
    public class CreateSaleCommandValidatorTests
    {
        [Fact]
        public void Should_Return_Valid_Error_When_No_Items()
        {
            var command = new CreateSaleCommand
            {
                Items = new List<SaleItemDTO>(), // vazio
                SaleNumber = "001"
            };

            var validator = new CreateSaleCommandValidator();
            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains(ValidationMessages.Sale.ProductItensRequired));
        }

        [Fact]
        public void Should_Pass_Valid_With_Valid_Data()
        {
            var command = new CreateSaleCommand
            {
                SaleNumber = "001",
                Customer = "Cliente Top",
                Branch = "SP01",
                Items = new List<SaleItemDTO>
            {
                new SaleItemDTO
                {
                    Name = "Produto A",
                    Quantities = 2,
                    UnitPrices = 10
                }
            }
            };

            var validator = new CreateSaleCommandValidator();
            var result = validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Invalid_When_Item_Has_Zero_Quantity()
        {
            var command = new CreateSaleCommand
            {
                Items = new List<SaleItemDTO>
            {
                new SaleItemDTO
                {
                    Name = "Produto A",
                    Quantities = 0,
                    UnitPrices = 10                   
                }
            }
            };

            var validator = new CreateSaleCommandValidator();
            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains(ValidationMessages.General.InclusiveBetween)); 
        }

        [Fact]
        public void Should_Return_Valid_Result()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "123456",
                Customer = "John Doe",
                Branch = "Main",
                Items = new List<SaleItemDTO>
                {
                    new SaleItemDTO { Name="produto1", Quantities = 2,  UnitPrices = 10 }
                }
            };

            // Act
            var result = command.Validate();

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
      
        [Fact]
        public void Should_Return_Invalid_Result_When_Missing_Fields()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                // Campos em branco
                SaleNumber = "",
                Customer = "",
                Branch = "",
                Items = null
            };

            // Act
            var result = command.Validate();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }


    }
}
