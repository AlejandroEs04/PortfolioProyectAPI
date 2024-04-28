using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/api/products")]

    public class ProductController : Controller
    {
        private readonly DataContextDapper _dapper;

        public ProductController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            string sql = @"SELECT * FROM Product";

            return _dapper.LoadData<Product>(sql);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            
            return Ok();
        }
    }
}