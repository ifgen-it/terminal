using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TerminalServer.Domain.Abstract;
using TerminalServer.Domain.Concrete;
using TerminalServer.Domain.Entities;

namespace TerminalServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ITerminalRepository repo;
        private readonly ILogger<ProductController> logger;

        public ProductController(ITerminalRepository repo, ILogger<ProductController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        // GET: api/Product
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = repo.Products
                .OrderBy(p => p.Name)
                .ToList();
            logger.LogInformation($"{DateTime.Now} GET: api/product > all products - not printed");
            Thread.Sleep(2000);

            return products;
        }

        // GET: api/Product/5
        [HttpGet("{productName}")]
        public ActionResult<Product> GetProduct(string productName)
        {
            productName = productName.Trim();
            var product = repo.Products
                .FirstOrDefault(p => p.Name == productName);

            if(product == null)
            {
                logger.LogInformation($"{DateTime.Now} GET: api/product/{productName} > null");
                return NotFound();
            }
            logger.LogInformation($"{DateTime.Now} GET: api/product/{productName} > {product}");
            return product;
        }

        // PUT: api/Product/5
        [HttpPut("{productName}")] // It will accumulate product amount = old product.Amount + new product.Amount
        public ActionResult<Product> PutProduct(string productName, Product product)
        {
            product.Name = product.Name.Trim();
            productName = productName.Trim();

            if (productName != product.Name)
            {
                logger.LogInformation($"{DateTime.Now} PUT: api/product/{productName} > BadRequest - {product}");
                return BadRequest();
            }

            if (!ProductExist(product.Name))
            {
                logger.LogInformation($"{DateTime.Now} PUT: api/product > BadRequest - {product}");
                return BadRequest("This product name was not created");
            }
            else
            {
                Thread.Sleep(5000);
                var updatedProduct = repo.Update(product);
                logger.LogInformation($"{DateTime.Now} PUT: api/product > {product}");
                return updatedProduct;
            }
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult<Product> PostProduct(Product product)
        {
            product.Name = product.Name.Trim();
            if (ProductExist(product.Name))
            {
                logger.LogInformation($"{DateTime.Now} POST: api/product > BadRequest - {product}");
                return BadRequest("This product name already in use");
            }
            else
            {
                Thread.Sleep(5000);
                repo.AddProduct(product);
                logger.LogInformation($"{DateTime.Now} POST: api/product > {product}");
                return CreatedAtAction("GetProduct", new { productName = product.Name }, product);
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{productName}")]
        public ActionResult<Product> DeleteProduct(string productName)
        {
            productName = productName.Trim();
            var product = repo.FindProduct(productName);

            if (product == null)
            {
                logger.LogInformation($"{DateTime.Now} DELETE: api/product/{productName} > NotFound");
                return NotFound();
            }
            repo.RemoveProduct(product);
            logger.LogInformation($"{DateTime.Now} DELETE: api/product/{productName} > {product}");
            Thread.Sleep(2000);

            return product;
        }
        
        private bool ProductExist(string productName)
        {
            var product = repo.Products
                            .FirstOrDefault(p => p.Name == productName);
            if (product != null)
                return true;
            else
                return false;
        }
    }
}
