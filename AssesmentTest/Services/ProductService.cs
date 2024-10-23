using Azure;
using DataModels.Models;

namespace AssesmentTest.Services
{
    public class ProductService : IProductService
    {
        private const string RequestUri = "api/product/";
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<IEnumerable<Product>> GetProducts() => await _httpClient.GetFromJsonAsync<Product[]>(requestUri: RequestUri);

        //public async Task<Product> GetProductById(int id) => await _httpClient.GetFromJsonAsync<Product>(requestUri: $"{RequestUri}{id}");
        // public Task<Product> GetProducts(int Id) => await _httpClient.GetFromJsonAsync<Product>(requestUri: RequestUri);
        public async Task<Product?> GetProductById(int id)
        {
            try
            {
                // Construct the request URI
                var response = await _httpClient.GetAsync(requestUri: $"{RequestUri}{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Product>();
                }
                else
                {
                    Console.WriteLine($"Error: Received status code {response.StatusCode} for product ID {id}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }
        }

        public Task<Product?> UpdateProduct(int productId, Product product)
        {
            try
            {
                // Send the product object as JSON to the API endpoint
                var response =  _httpClient.PutAsJsonAsync($"{RequestUri}{productId}", product).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    // Read and return the response as a Product object if the call was successful
                    return  response.Content.ReadFromJsonAsync<Product>();
                }
                else
                {
                    Console.WriteLine($"Error: Received status code {response.StatusCode} when updating product");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }

        }

        public Task<Product?> AddProduct(Product product)
        {
            try
            {
                // Send the product object as JSON to the API endpoint
                var response = _httpClient.PostAsJsonAsync($"{RequestUri}", product).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    // Read and return the response as a Product object if the call was successful
                    return response.Content.ReadFromJsonAsync<Product>();
                }
                else
                {
                    Console.WriteLine($"Error: Received status code {response.StatusCode} when posting product");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                // Send the product object as JSON to the API endpoint
                var response = await _httpClient.DeleteAsync($"{RequestUri}{productId}");

                if (response.IsSuccessStatusCode)
                {
                    // Read and return the response as a Product object if the call was successful
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error: Received status code {response.StatusCode} when deleting product ID {productId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }
    }
    
}
