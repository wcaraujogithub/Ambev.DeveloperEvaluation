using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class SaleRepositoryTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly CreateSaleHandler _handler;
        private readonly ILogger<CreateSaleHandler> _loggerMock;
        private readonly IMediator _mediatorMock;

        public SaleRepositoryTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _mediatorMock = Substitute.For<IMediator>();
            _loggerMock = Substitute.For<ILogger<CreateSaleHandler>>();
            _handler = new CreateSaleHandler(_saleRepository, _mapper, _loggerMock, _mediatorMock);
        }

        [Fact]
        public async Task CreateAsync_Should_Call_Repository()
        {
            // Arrange
            var sale = new Sale { Id = Guid.NewGuid(), SaleNumber = "123", CreatedAt = DateTime.UtcNow };

            // Act
            await _saleRepository.CreateAsync(sale);

            // Assert
            await _saleRepository.Received(1).CreateAsync(sale);
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
        };

            _saleRepository
                .ListAsync(1, 10, "customer asc, totalValue desc")
                .Returns(Task.FromResult(new PagedResult<Sale>
                {
                    Items = data
                        .OrderBy(s => s.Customer)
                        .ThenByDescending(s => s.TotalValue)
                        .ToList()
                }));

            // Act
            var result = await _saleRepository.ListAsync(1, 10, "customer asc, totalValue desc");

            // Assert
            result.Items[0].Customer.ShouldBe("Ana");
            result.Items[0].TotalValue.ShouldBe(100);
            result.Items[1].Customer.ShouldBe("Ana");
            result.Items[2].Customer.ShouldBe("Zeca");
        }

        [Fact]
        public async Task Should_Ignore_Invalid_Order_Fields()
        {
            // Arrange
            var data = new List<Sale>
        {
            new Sale { Customer = "Carlos", CreatedAt = new DateTime(2024, 1, 1) },
            new Sale { Customer = "Bruna", CreatedAt = new DateTime(2024, 2, 1) },
        };

            _saleRepository
                .ListAsync(1, 10, "senha desc")
                .Returns(Task.FromResult(new PagedResult<Sale>
                {
                    Items = data.OrderByDescending(s => s.CreatedAt).ToList()
                }));

            // Act
            var result = await _saleRepository.ListAsync(1, 10, "senha desc");

            // Assert
            result.Items[0].Customer.ShouldBe("Bruna");
        }

        [Fact]
        public async Task Should_Assume_Default_Direction_As_Asc()
        {
            // Arrange
            var data = new List<Sale>
        {
            new Sale { Branch = "C" },
            new Sale { Branch = "A" },
            new Sale { Branch = "B" }
        };

            _saleRepository
                .ListAsync(1, 10, "branch")
                .Returns(Task.FromResult(new PagedResult<Sale>
                {
                    Items = data.OrderBy(s => s.Branch).ToList()
                }));

            // Act
            var result = await _saleRepository.ListAsync(1, 10, "branch");

            // Assert
            result.Items[0].Branch.ShouldBe("A");
            result.Items[1].Branch.ShouldBe("B");
            result.Items[2].Branch.ShouldBe("C");
        }

        
        private static DbSet<T> GetFakeDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var dbSet = Substitute.For<DbSet<T>, IQueryable<T>>();

            ((IQueryable<T>)dbSet).Provider.Returns(queryable.Provider);
            ((IQueryable<T>)dbSet).Expression.Returns(queryable.Expression);
            ((IQueryable<T>)dbSet).ElementType.Returns(queryable.ElementType);
            ((IQueryable<T>)dbSet).GetEnumerator().Returns(queryable.GetEnumerator());

            return dbSet;
        }
    }
}
