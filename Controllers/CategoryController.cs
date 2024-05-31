using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Dtos;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly DataContextDapper _dapper;
        public CategoryController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            string sql = "SELECT * FROM Category";
            return _dapper.LoadData<Category>(sql);
        }

        [HttpGet("{CategoryID}")]
        public Category GetOnCategory(int categoryID)
        {
            string sql = @"
                SELECT * FROM Category 
                WHERE CategoryID = " + categoryID.ToString();
            
            return _dapper.LoadDataSingle<Category>(sql);
        }

        [HttpPost]
        public IActionResult AddNewCategory(CategoryAdd category)
        {
            string sqlAddCategory = @"
                INSERT INTO Category (CategoryName)
                VALUES ('" + category.CategoryName + "')";

            if(!_dapper.ExecuteSql(sqlAddCategory))
            {
                throw new Exception("Failed to add new category");
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateCategory(Category category)
        {
            string sqlUpdateCategory = @"
                UPDATE Category
                SET 
                    CategoryName = '" + category.CategoryName + @"'
                WHERE CategoryID = " + category.CategoryID.ToString();

            if(!_dapper.ExecuteSql(sqlUpdateCategory))
            {
                throw new Exception("Failed to update category");
            }

            return Ok();
        }

        [HttpDelete("{categoryID}")]
        public IActionResult DeleteCategory(int categoryID)
        {
            string sqlDeleteCategory = @"
                DELETE FROM Category 
                WHERE CategoryID = " + categoryID.ToString();

            if(!_dapper.ExecuteSql(sqlDeleteCategory))
            {
                throw new Exception("Failed to delete category");
            }

            return Ok();
        }
    }
}