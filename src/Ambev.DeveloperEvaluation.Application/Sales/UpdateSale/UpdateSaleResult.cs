namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Represents the response returned after successfully updating a new sale.
    /// </summary>
    /// <remarks>
    /// This response contains the unique identifier of the newly updated sale,
    /// which can be used for subsequent operations or reference.
    /// </remarks>
    public class UpdateSaleResult
    {
        /// <summary>
        /// Indicates whether the updated was successful
        /// </summary>
        /// 
        public Guid Id { get; set; }
        public string? Customer { get; set; }
        public string? Branch { get; set; }  
    }
}
