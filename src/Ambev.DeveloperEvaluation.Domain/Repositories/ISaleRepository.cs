using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<bool> DeleteSaleItemsAsync(Guid saleId, CancellationToken cancellationToken = default);
        Task<PagedResult<Sale>> ListAsync(int page, int pageSize, string? order, CancellationToken cancellationToken = default);
    }

}
