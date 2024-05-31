using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Dtos;
using PortfolioAPI.Helpers;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]

    public class UserController : Controller
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            string sql = "SELECT * FROM [User]";
            return _dapper.LoadData<User>(sql);
        }

        [HttpPost]
        public IActionResult AddNewUser(UserAdd user)
        {
            string sqlExistEmail = @"
                SELECT * FROM [User]
                WHERE Email like '" + user.Email + "' OR PhoneNumber like '" + user.PhoneNumber + "'";

            Console.WriteLine(sqlExistEmail);

            IEnumerable<User> users = _dapper.LoadData<User>(sqlExistEmail);

            if(users.Count() > 0)
            {
                throw new Exception("There's someone else with this email or phone number");
            }

            string sqlAddUser = @"
                INSERT INTO [User] (FirstName, LastName, Email, PhoneNumber)
                VALUES (
                    '" + user.FirstName + @"', 
                    '" + user.LastName + @"', 
                    '" + user.Email + @"', 
                    '" + user.PhoneNumber + @"'
                )";

            // if(!_dapper.ExecuteSql(sqlAddUser))
            // {
            //     throw new Exception("Failed to create user");
            // }

            byte[] passwordSalt = new byte[129 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = new byte[0];

            string sqlGetUserCreated = "SELECT MAX(UserID) AS UserID FROM [User]";

            // I need to finish this
            
            return Ok();
        }
    }
}