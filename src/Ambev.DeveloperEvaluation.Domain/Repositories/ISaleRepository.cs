using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Sale?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Sale?> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<List<SaleItem>?> DeleteSaleItemsAsync(Guid saleId, CancellationToken cancellationToken = default);
        Task<PagedResult<Sale>> ListAsync(int page, int pageSize, string? order, CancellationToken cancellationToken = default);
    }

}
