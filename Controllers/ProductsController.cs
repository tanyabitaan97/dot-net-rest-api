using Microsoft.AspNetCore.Mvc;
using dotnet.Models;

namespace dotnet.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {

        private static List<Product> products = new List<Product>()
        {
            new Product { Id = 1, Name = "Laptop", Price = 75000, Quantity = 10 },
            new Product { Id = 2, Name = "Mobile", Price = 20000, Quantity = 20 },
            new Product { Id = 3, Name = "Tablet", Price = 30000, Quantity = 5 }
        };


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(products);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }


        [HttpPost]
        public IActionResult Create([FromBody] Product newProduct)
        {
            newProduct.Id = products.Max(p => p.Id) + 1;   // Auto increment ID

            products.Add(newProduct);
            return Ok("Product created successfully");
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Quantity = updatedProduct.Quantity;

            return Ok("Product updated successfully");
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            products.Remove(product);

            return Ok("Product deleted successfully");
        }
    }
}
