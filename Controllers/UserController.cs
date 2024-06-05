using System.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        [HttpGet("{userID}")]
        public User GetOneUser(int userID)
        {
            string sql = @"SELECT * FROM [User] WHERE UserID = " + userID.ToString();
            return _dapper.LoadDataSingle<User>(sql);
        }

        [HttpPut]
        public IActionResult UpdateUser(UserUpdateDto user)
        {
            if(user.Password != "")
            {
                byte[] passwordSalt = new byte[129 / 8];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetNonZeroBytes(passwordSalt);
                }

                byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, passwordSalt);
                
                string sqlUpdateAuth = @"
                    UPDATE Auth
                    SET
                        PasswordHash = @PasswordHash, 
                        PasswordSalt = @PasswordSalt
                    WHERE UserID = " + user.UserID.ToString();

                List<SqlParameter> sqlParameters = new List<SqlParameter>();

                SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                passwordSaltParameter.Value = passwordSalt;

                SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                passwordHashParameter.Value = passwordHash;

                sqlParameters.Add(passwordSaltParameter);
                sqlParameters.Add(passwordHashParameter);

                if(!_dapper.ExecuteSqlWithParameters(sqlUpdateAuth, sqlParameters))
                {
                    throw new Exception("Failed to update password");
                } 
            }

            string updateUser = @"
                UPDATE [User]
                SET 
                    FirstName = '" + user.FirstName + @"', 
                    LastName = '" + user.LastName + @"', 
                    Email = '" + user.Email + @"', 
                    PhoneNumber = '" + user.PhoneNumber + @"', 
                    Address = '" + user.Address + @"'
                WHERE UserID = " + user.UserID.ToString();

            if(!_dapper.ExecuteSql(updateUser))
            {
                throw new Exception("Failed to update user");
            }

            return Ok();
        }

        [HttpDelete("{userID}")]
        public IActionResult DeleteUser(int userID)
        {
            string sqlDeleteAuth = @"
                DELETE FROM Auth
                WHERE UserID = " + userID.ToString();

            if(!_dapper.ExecuteSql(sqlDeleteAuth)) 
            {
                throw new Exception("Failed to delete user");
            }

            string sqlDeleteUser = @"
                Update [User]
                    SET Active = 'false'
                WHERE UserID = " + userID.ToString();

            if(!_dapper.ExecuteSql(sqlDeleteUser)) 
            {
                throw new Exception("Failed to delete user");
            }

            return Ok();
        }
    }
}