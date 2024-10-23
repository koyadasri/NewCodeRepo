using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataModels.Models;
using DataModels.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApp1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("call-apis")]
        public async Task<IActionResult> CallApisAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var task1 = client.GetStringAsync("https://localhost:7234/api/Api2"); // MyApi1
            var task2 = client.GetStringAsync("https://localhost:7275/api/Api3"); // MyApi2

            await Task.WhenAny(task1, task2);

            return Ok(new { Api1Response = await task1, Api2Response = await task2 });
        }

        [HttpGet]
        //[Route("getProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.tblProducts.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product?>> GetProduct(int id)
        {
            var product = await _context.tblProducts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.tblProducts.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.tblProducts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.tblProducts.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
