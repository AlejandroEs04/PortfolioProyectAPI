using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]

    public class ServicePlanController : Controller
    {
        private readonly DataContextDapper _dapper;
        public ServicePlanController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet]
        public IEnumerable<ServicePlan> GetServicePlans()
        {
            string sqlGetAllServicePlans = "SELECT * FROM ServicePlan";
            return _dapper.LoadData<ServicePlan>(sqlGetAllServicePlans);
        }
    }
}