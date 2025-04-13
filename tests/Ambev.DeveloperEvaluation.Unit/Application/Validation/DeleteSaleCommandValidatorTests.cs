using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Common.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validation
{
    public class DeleteSaleCommandValidatorTests
    {
        [Fact]
        public void Should_Return_Valid_Error_When_No_ID()
        {
            var command = new DeleteSaleCommand(Guid.Empty);

            var validator = new DeleteSaleValidator();
            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains(ValidationMessages.General.FieldIsRequired));
        }

        [Fact]
        public void Should_Return_Valid_Result_OK()
        {
            // Arrange
            var command = new DeleteSaleCommand(Guid.NewGuid());

            // Act

            var result = command.Validate();

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

    }
}
