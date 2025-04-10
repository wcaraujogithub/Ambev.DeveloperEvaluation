using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of SaleRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new sale in the database
        /// </summary>
        /// <param name="sale">The sale to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale</returns>
        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        /// <summary>
        /// List a sales 
        /// </summary>  
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sales, null otherwise</returns>
        public async Task<Domain.Common.PagedResult<Sale>> ListAsync(int page, int pageSize, string? order, CancellationToken cancellationToken = default)
        {
            var query = _context.Sales
                .Include(x => x.Items)
                .AsNoTracking();

            // Campos permitidos para ordenação
            var allowedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)  
            {
                "Id",                
                "SaleNumber",
                "Customer",
                "TotalValue",
                "Branch",
                "CreatedAt",
                "UpdatedAt",
                "Cancelled"
            };         

            // Validação e aplicação da ordenação
            if (!string.IsNullOrWhiteSpace(order))
            {
                // Divide as colunas por vírgula: "id desc, customer asc"
                var orderParts = order.Split(',', StringSplitOptions.RemoveEmptyEntries);

                var validatedOrderParts = new List<string>();

                foreach (var part in orderParts)
                {
                    var match = Regex.Match(part.Trim(), @"^(?<column>\w+)(\s+(?<direction>asc|desc))?$", RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        var column = match.Groups["column"].Value;
                        var direction = match.Groups["direction"].Success ? match.Groups["direction"].Value : "asc";

                        if (allowedColumns.Contains(column))
                        {
                            validatedOrderParts.Add($"{column} {direction}");
                        }
                    }
                }

                if (validatedOrderParts.Any())
                {
                    var validatedOrder = string.Join(", ", validatedOrderParts);
                    query = query.OrderBy(validatedOrder);
                }
                else
                {
                    query = query.OrderByDescending(s => s.CreatedAt); // fallback
                }
            }
            else
            {
                query = query.OrderByDescending(s => s.CreatedAt); // padrão
            }

            var totalCount = await query.CountAsync(cancellationToken);
         
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new Domain.Common.PagedResult<Sale>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
   
        /// <summary>
        /// Retrieves a sale by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns> sale, null otherwise</returns>
        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context
                .Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Delete a sale from the database
        /// </summary>
        /// <param name="id">The unique identifier of the sale to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the sale was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale is null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Deletes a sale items from the database
        /// </summary>
        /// <param name="saleId">The unique identifier of the sale to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the sale was deleted, false if not found</returns>
        public async Task<bool> DeleteSaleItemsAsync(Guid saleId, CancellationToken cancellationToken = default)
        {
            var itemss = _context.SaleItems.Where(i => i.SaleId == saleId).ToList();
            var items = _context.SaleItems.Where(i => i.SaleId == saleId);
            if (items is null)
                return false;

            _context.SaleItems.RemoveRange(items);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Update a sale from the database
        /// </summary>
        /// <param name="sale">The unique identifier of the sale to Update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the sale was Update, false if not found</returns>
        public async Task<bool> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            var saleUpdate = await GetByIdAsync(sale.Id, cancellationToken);
            if (saleUpdate == null)
                return false;

            saleUpdate.Branch = sale.Branch;
            saleUpdate.Customer = sale.Customer;
            saleUpdate.UpdatedAt = DateTime.UtcNow;

            _context.Sales.Update(saleUpdate);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
