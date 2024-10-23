using DataModels.Models;

namespace AssesmentTest.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<Product?> GetProductById(int id);

        Task<Product?> UpdateProduct(int id, Product product);

        Task<Product?> AddProduct(Product product);
        Task<bool> DeleteProduct(int id);
    }
}
