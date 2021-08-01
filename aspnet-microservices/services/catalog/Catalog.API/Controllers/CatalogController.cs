using Catalog.API.Entities;
using Catalog.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _productRepository;

        public CatalogController(ILogger<CatalogController> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts()
        {
           return Ok(await _productRepository.GetProducts());
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productRepository.GetProduct(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("{name}", Name = "GetProductByName")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var product = await _productRepository.GetProductByName(name);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        public async Task<IActionResult> GetProductByCategory(string category)
        {
            var product = await _productRepository.GetProductByCategory(category);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            await _productRepository.UpdateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct([FromBody] string id)
        {
            return Ok(await _productRepository.DeleteProduct(id));
        }

    }
}
