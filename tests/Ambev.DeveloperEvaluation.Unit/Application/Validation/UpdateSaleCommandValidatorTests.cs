using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Common.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validation
{
    public class UpdateSaleCommandValidatorTests
    {
        [Fact]
        public void Should_Return_Valid_Error_When_No_Customer_MinimumLength()
        {
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                Customer = "aa",
                Branch = "new branch ",
            };

            var validator = new UpdateSaleCommandValidator();
            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains(ValidationMessages.General.MinimumLength));
        }

        [Fact]
        public void Should_Pass_Valid_With_Valid_Data()
        {
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                Customer = "new custumer",
                Branch = "new branch ",
            };

            var validator = new UpdateSaleCommandValidator();
            var result = validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_Valid_Result()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                Customer = "aaa",
                Branch = "new branch ",
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
            var command = new UpdateSaleCommand
            {
                Id = Guid.Empty,
                Customer = string.Empty,
                Branch = string.Empty,
            };

            // Act
            var result = command.Validate();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }
    }
}
