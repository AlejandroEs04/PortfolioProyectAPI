using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Dtos;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]
    public class SpecificationController : Controller
    {
        private readonly DataContextDapper _dapper;
        public SpecificationController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet]
        public IEnumerable<Specification> GetSpecifications()
        {
            string sql = "SELECT * FROM Specification";
            return _dapper.LoadData<Specification>(sql);
        }

        [HttpGet("{specificationID}")]
        public Specification GetOneSpecification(int specificationID) 
        {
            string sql = @"
                SELECT * FROM Specification 
                WHERE SpecificationID = " + specificationID.ToString();

            return _dapper.LoadDataSingle<Specification>(sql);
        }

        [HttpPost]
        public IActionResult AddNewSpecification(SpecificationAdd specification)
        {
            string sqlAddSpecification = @"
                INSERT INTO Specification (SpecificationDesc)
                VALUES ('" + specification.SpecificationDesc + "')";

            if(!_dapper.ExecuteSql(sqlAddSpecification))
            {
                throw new Exception("Failed to add new specification");
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateSpecification(Specification specification)
        {
            string sqlUpdateSpecification = @"
                UPDATE Specification
                SET 
                    SpecificationDesc = '" + specification.SpecificationDesc + @"'
                WHERE SpecificationID = " + specification.SpecificationID.ToString();

            if(!_dapper.ExecuteSql(sqlUpdateSpecification))
            {
                throw new Exception("Failed to update specification");
            }
            return Ok();
        }

        [HttpDelete("{specificationID}")]
        public IActionResult DeleteSpecification(int specificationID)
        {
            string sqlDeleteSpecification = @"
                DELETE FROM Specification
                WHERE SpecificationID = " + specificationID.ToString();
            
            if(!_dapper.ExecuteSql(sqlDeleteSpecification))
            {
                throw new Exception("Failed to delete specification");
            }

            return Ok();
        }
    }
}