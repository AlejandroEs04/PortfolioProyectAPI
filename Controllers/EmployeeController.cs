using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Dtos;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]

    public class EmployeeController : Controller
    {
        private readonly DataContextDapper _dapper;
        public EmployeeController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            string sqlGetAllEmployee = "SELECT * FROM Employee";
            return _dapper.LoadData<Employee>(sqlGetAllEmployee);
        }

        [HttpGet("{employeeID}")]
        public Employee GetEmployee(int employeeID)
        {
            string sqlGetOneEmployee = @"
                SELECT * FROM Employee 
                WHERE EmployeeID = " + employeeID.ToString();

            return _dapper.LoadDataSingle<Employee>(sqlGetOneEmployee);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeAddDto employee)
        {
            string sqlAddEmployee = @"
                INSERT INTO Employee (FirstName, LastName, Email, PhoneNumber, Address, RolID, Salary)
                VALUES (
                    '" + employee.FirstName + @"', 
                    '" + employee.LastName + @"', 
                    '" + employee.Email + @"', 
                    '" + employee.PhoneNumber + @"', 
                    '" + employee.Address + @"', 
                    " + employee.RolID + @", 
                    " + employee.Salary + @"
                )";

            if(!_dapper.ExecuteSql(sqlAddEmployee))
            {
                throw new Exception("Failed to add employee");
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateEmployee(Employee employee)
        {
            string sqlUpdateEmployee = @"
                UPDATE Employee
                SET 
                    FirstName = '" + employee.FirstName + @"', 
                    LastName = '" + employee.LastName + @"', 
                    Email = '" + employee.Email + @"', 
                    PhoneNumber = '" + employee.PhoneNumber + @"', 
                    Address = '" + employee.Address + @"', 
                    RolID = " + employee.RolID + @", 
                    Salary = " + employee.Salary + @", 
                    Active = '" + employee.Active + @"'
                WHERE EmployeeID = " + employee.EmployeeID.ToString();

            if(!_dapper.ExecuteSql(sqlUpdateEmployee))
            {
                throw new Exception("Failed to update employee");
            }

            return Ok();
        }

        [HttpDelete("{employeeID}")]
        public IActionResult DeleteEmployee(int employeeID)
        {
            string sqlDeleteEmployee = @"
                UPDATE Employee
                SET Active = 'false'
                WHERE EmployeeID = " + employeeID.ToString();

            if(!_dapper.ExecuteSql(sqlDeleteEmployee))
            {
                throw new Exception("Failed to delete employee");
            }

            return Ok();
        }
    }
}