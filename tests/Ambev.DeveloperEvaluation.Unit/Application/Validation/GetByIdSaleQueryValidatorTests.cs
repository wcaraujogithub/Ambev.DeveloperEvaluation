using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Common.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validation
{
    public class GetByIdSaleQueryValidatorTests
    {
        [Fact]
        public void Should_Return_Valid_Error_When_No_ID()
        {
            var command = new GetByIdSaleQuery(Guid.Empty);

            var validator = new GetByIdSaleValidator();
            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains(ValidationMessages.General.FieldIsRequired));
        }

        [Fact]
        public void Should_Return_Valid_Result_OK()
        {
            // Arrange
            var command = new GetByIdSaleQuery(Guid.NewGuid());

            // Act

            var result = command.Validate();

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
