using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class SaleRepositoryTests
    {
        private readonly Mock<DefaultContext> _mockContext;
        private readonly SaleRepository _repository;

        public SaleRepositoryTests()
        {
            _mockContext = new Mock<DefaultContext>();
            _repository = new SaleRepository(_mockContext.Object);
        }

        [Fact]
        public async Task Should_Order_By_Customer_Asc_And_TotalValue_Desc()
        {
            // Arrange
            var data = new List<Sale>
        {
            new Sale { Customer = "Zeca", TotalValue = 50 },
            new Sale { Customer = "Ana", TotalValue = 100 },
            new Sale { Customer = "Ana", TotalValue = 80 }
        }.AsQueryable();

            var mockSet = GetMockDbSet(data);
            _mockContext.Setup(c => c.Sales).Returns(mockSet.Object);

            // Act
            var result = await _repository.ListAsync(1, 10, "customer asc, totalValue desc");

            // Assert
            result?.Items[0].Customer.ShouldBe("Ana");
            result?.Items[0].TotalValue.ShouldBe(100);
            result?.Items[1].Customer.ShouldBe("Ana");
            result?.Items[2].Customer.ShouldBe("Zeca");
        }

        [Fact]
        public async Task Should_Ignore_Invalid_Order_Fields()
        {
            var data = new List<Sale>
        {
            new Sale { Customer = "Carlos", CreatedAt = new DateTime(2024, 1, 1) },
            new Sale { Customer = "Bruna", CreatedAt = new DateTime(2024, 2, 1) },
        }.AsQueryable();

            var mockSet = GetMockDbSet(data);
            _mockContext.Setup(c => c.Sales).Returns(mockSet.Object);

            var result = await _repository.ListAsync(1, 10, "senha desc");

            result.Items[0].Customer.ShouldBe("Bruna"); // fallback: CreatedAt desc
        }

        [Fact]
        public async Task Should_Assume_Default_Direction_As_Asc()
        {
            var data = new List<Sale>
        {
            new Sale { Branch = "C" },
            new Sale { Branch = "A" },
            new Sale { Branch = "B" }
        }.AsQueryable();

            var mockSet = GetMockDbSet(data);
            _mockContext.Setup(c => c.Sales).Returns(mockSet.Object);

            var result = await _repository.ListAsync(1, 10, "branch");

            result.Items[0].Branch.ShouldBe("A");
            result.Items[1].Branch.ShouldBe("B");
            result.Items[2].Branch.ShouldBe("C");
        }

        private Mock<DbSet<Sale>> GetMockDbSet(IQueryable<Sale> data)
        {
            var mockSet = new Mock<DbSet<Sale>>();
            mockSet.As<IQueryable<Sale>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Sale>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Sale>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Sale>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }

}
