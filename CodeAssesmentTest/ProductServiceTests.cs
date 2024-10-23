using AssesmentTest.Services;
using DataModels.Models;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeAssesmentTest
{
    public class ProductServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly ProductService _productService;
        public ProductServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:7012/") // Mocked base address
            };
            _productService = new ProductService(_httpClient);
        }

        [Fact]
        public async Task GetProducts()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 100 },
            new Product { Id = 2, Name = "Product 2", Price = 200 }
        };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(products), System.Text.Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _productService.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Product 1", result.First().Name);
        }


        [Fact]
        public async Task GetProductById()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100 };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri == new Uri("https://localhost:7012/api/Product/1")), // Ensure this matches your service method exactly
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _productService.GetProductById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Product 1", result.Name);
            Assert.Equal(100, result.Price);
        }

        [Fact]
        public async Task AddProduct()
        {
            // Arrange
            var newProduct = new Product { Id = 3, Name = "New Product", Price = 150 };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonSerializer.Serialize(newProduct), System.Text.Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri == new Uri("https://localhost:7012/api/Product")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _productService.AddProduct(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Product", result.Name);
            Assert.Equal(150, result.Price);
        }

        [Fact]
        public async Task UpdateProduct()
        {
            // Arrange
            var updatedProduct = new Product { Id = 1, Name = "Updated Product", Price = 150 };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(updatedProduct), System.Text.Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Put &&
                        req.RequestUri == new Uri("https://localhost:7012/api/Product/1")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _productService.UpdateProduct(1, updatedProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Product", result.Name);
            Assert.Equal(150, result.Price);
        }

        [Fact]
        public async Task DeleteProduct()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent // 204 No Content indicates success for DELETE
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri == new Uri("https://localhost:7012/api/Product/1")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _productService.DeleteProduct(1);

            // Assert
            Assert.True(result);
        }
    }
}

