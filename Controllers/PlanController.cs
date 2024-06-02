using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Dtos;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]
    public class PlanController : Controller
    {
        private readonly DataContextDapper _dapper;
        public PlanController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet]
        public IEnumerable<Plan> GetAllPlans()
        {
            string sqlGetAllPlans = "SELECT * FROM Plan";
            return _dapper.LoadData<Plan>(sqlGetAllPlans);
        }

        [HttpGet("{planID}")]
        public Plan GetOnPlan(int planID)
        {
            string sqlOnePlan = @"
                SELECT * FROM Plan 
                WHERE PlanID = " + planID.ToString();
            return _dapper.LoadDataSingle<Plan>(sqlOnePlan);
        }

        [HttpPost]
        public IActionResult AddNewPlan(PlanAddDto plan)
        {
            string sqlAddPlan = @"
                INSERT INTO Plan (PlanName, Price, LogoURL)
                VALUES ('" + plan.PlanName + "', " + plan.Price + ", '" + plan.LogoURL + "')";

            if(!_dapper.ExecuteSql(sqlAddPlan))
            {
                throw new Exception("Failed to add plan");
            }

            return Ok();
        }
    }
}