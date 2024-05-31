using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Dtos;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]

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

        [HttpGet("{ProductId}")]
        public Product GetOneProduct(int ProductId)
        {
            string sql = "SELECT * FROM Product WHERE ProductId = " + ProductId.ToString();
            return _dapper.LoadDataSingle<Product>(sql);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductAdd product)
        {
            string sqlAddProduct = @"
                INSERT INTO Product
                (ProductName, ProductDesc, UnitPrice, UnitsInStock, UnitCost, CategoryID, Model, Active)
                VALUES 
                (
                    '" + product.ProductName + @"',
                    '" + product.ProductDesc + @"',
                    '" + product.UnitPrice + @"',
                    '" + product.UnitsInStock + @"',
                    '" + product.UnitCost + @"',
                    '" + product.CategoryID + @"',
                    '" + product.Model + @"',
                    '" + product.Active + @"'
                )
            ";

            if(!_dapper.ExecuteSql(sqlAddProduct))
            {
                throw new Exception("Failed to add new product");
            }

            string sqlGetProduct = "SELECT MAX(ProductID) AS ProductID FROM Product";

            Product newProduct = _dapper.LoadDataSingle<Product>(sqlGetProduct);

            if(product.Specifications.Count() != 0)
            {
                string sqlAddSpecifications = @"
                    INSERT INTO ProductSpecification
                    (ProductID, SpecificationID, SpecificationDesc)
                    VALUES ";
                
                foreach(var specification in product.Specifications)
                {
                    sqlAddSpecifications += "('" + newProduct.ProductID + "', '" + specification.SpecificationID + "', '" + specification.SpecificationDesc + "'),";
                }

                Console.WriteLine(sqlAddSpecifications.Substring(0, sqlAddSpecifications.Length - 1));

                if(!_dapper.ExecuteSql(sqlAddSpecifications.Substring(0, sqlAddSpecifications.Length - 1))) 
                {
                    throw new Exception("Failed to add new characteristic to product");
                }
            }

            if(product.Images.Count() != 0) 
            {
                string sqlAddImage = @"
                    INSERT INTO ProductImage
                    (ProductID, ImageURL)
                    VALUES ";
                
                foreach(var image in product.Images)
                {
                    sqlAddImage += "('" + newProduct.ProductID + "', '" + image.ImageURL + "'),";
                }

                if(!_dapper.ExecuteSql(sqlAddImage.Substring(0, sqlAddImage.Length - 1))) 
                {
                    throw new Exception("Failed to add new images to product");
                }
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateProduct (Product product)
        {
            string sqlUpdate = @"
                UPDATE Product 
                SET 
                    ProductName = '" + product.ProductName +  @"', 
                    ProductDesc = '" + product.ProductDesc +  @"', 
                    UnitPrice = '" + product.UnitPrice +  @"', 
                    UnitsInStock = '" + product.UnitsInStock +  @"', 
                    UnitCost = '" + product.UnitCost +  @"', 
                    Model = '" + product.Model +  @"', 
                    Active = '" + product.Active +  @"', 
                    CategoryID = '" + product.CategoryID +  @"'
                WHERE ProductId = " + product.ProductID;

            if(!_dapper.ExecuteSql(sqlUpdate)) 
            {
                throw new Exception("Failed to update product");
            }
            return Ok();
        }

        [HttpDelete("{ProductID}")]
        public IActionResult DeleteProduct (int ProductID)
        {
            string sqlDelete = @"
                DELETE FROM Product
                WHERE ProductID = " + ProductID.ToString();
            
            if(!_dapper.ExecuteSql(sqlDelete))
            {
                throw new Exception("Failed to delete product");
            }

            return Ok();
        }
    }
}