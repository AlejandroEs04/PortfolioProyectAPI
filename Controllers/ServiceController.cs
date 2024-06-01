using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Dtos;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]
    public class ServiceController : Controller
    {
        private readonly DataContextDapper _dapper;
        public ServiceController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet]
        public IEnumerable<Service> GetServices()
        {
            string sqlGetAllCategories = @"SELECT * FROM Service";
            return _dapper.LoadData<Service>(sqlGetAllCategories);
        }

        [HttpGet("{serviceID}")]
        public Service GetOneService(int serviceID)
        {
            string sqlGetOneService = @"
                SELECT * FROM Service
                WHERE ServiceID = " + serviceID.ToString();

            return _dapper.LoadDataSingle<Service>(sqlGetOneService);
        }

        [HttpPost]
        public IActionResult AddNewService(ServiceAddDto service)
        {
            string sqlAddService = @"
                INSERT INTO Service (ServiceName, ServiceDesc, ImageURL)
                VALUES ('" + service.ServiceName + "', '" + service.ServiceDesc + "', '" + service.ImageURL + "')";
            
            if(!_dapper.ExecuteSql(sqlAddService))
            {
                throw new Exception("Filed to create service");
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateService(Service service)
        {
            string sqlUpdateService = @"
                UPDATE Service
                SET 
                    ServiceName = '" + service.ServiceName + @"', 
                    ServiceDesc = '" + service.ServiceDesc + @"', 
                    ImageURL = '" + service.ImageURL + @"', 
                    Active = '" + service.Active + @"'
                WHERE ServiceID = " + service.ServiceID.ToString();

            if(!_dapper.ExecuteSql(sqlUpdateService))
            {
                throw new Exception("Failed to update service");
            }

            return Ok();
        }

        [HttpDelete("{serviceID}")]
        public IActionResult DeleteService(int serviceID)
        {
            string sqlDeleteService = @"
                DELETE FROM Service
                WHERE ServiceID = " + serviceID.ToString();
            
            if(!_dapper.ExecuteSql(sqlDeleteService))
            {
                throw new Exception("Failed to delete service");
            }

            return Ok();
        }
    }
}