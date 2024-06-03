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
    public class AuthController : Controller
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [HttpPost("Signup")]
        public IActionResult Register(UserAddDto user)
        {
            int userId = 0;

            string sqlExistEmail = @"
                SELECT * FROM [User]
                WHERE Email like '" + user.Email + "' OR PhoneNumber like '" + user.PhoneNumber + "'";

            IEnumerable<User> users = _dapper.LoadData<User>(sqlExistEmail);

            if(users.Count() > 0)
            {
                string sqlGetUserId = @"
                    SELECT UserID FROM [User] 
                    WHERE Email = '" + user.Email + "' OR PhoneNumber = '" + user.PhoneNumber + "'";

                userId = _dapper.LoadDataSingle<int>(sqlGetUserId);

                string getAuthExist = @"
                    SELECT * FROm Auth
                    WHERE UserID  = " + userId.ToString();

                IEnumerable<Auth> authExist = _dapper.LoadData<Auth>(getAuthExist);

                if(authExist.Count() > 0)
                {
                    throw new Exception("There's someone else with this email or phone number");
                }    

                string sqlUpdateUser = @"
                    UPDATE [User]
                    SET Active = 'true'
                    WHERE UserID = " + userId.ToString();    

                if(!_dapper.ExecuteSql(sqlUpdateUser))
                {
                    throw new Exception("Failed to create user");
                }        
            } else {
                string sqlAddUser = @"
                    INSERT INTO [User] (FirstName, LastName, Email, PhoneNumber)
                    VALUES (
                        '" + user.FirstName + @"', 
                        '" + user.LastName + @"', 
                        '" + user.Email + @"', 
                        '" + user.PhoneNumber + @"'
                    )";

                if(!_dapper.ExecuteSql(sqlAddUser))
                {
                    throw new Exception("Failed to create user");
                }

                string sqlGetUserCreated = "SELECT MAX(UserID) AS UserID FROM [User]";
                userId = _dapper.LoadDataSingle<int>(sqlGetUserCreated);
            }


            byte[] passwordSalt = new byte[129 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, passwordSalt);

            string sqlAddAuth = @"
                INSERT INTO Auth (UserID, PasswordHash, PasswordSalt)
                VALUES (" + userId + ", @PasswordHash, @PasswordSalt)";

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
            passwordSaltParameter.Value = passwordSalt;

            SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
            passwordHashParameter.Value = passwordHash;

            sqlParameters.Add(passwordSaltParameter);
            sqlParameters.Add(passwordHashParameter);

            if(!_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
            {
                throw new Exception("Failed to add user");
            } 
            
            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginDto userLogin)
        {
            string sqlExistUserEmail = @"
                SELECT * FROM [User]
                WHERE Email like '" + userLogin.Email + "'";


            if(_dapper.LoadData<User>(sqlExistUserEmail).Count() == 0)
            {
                return StatusCode(401, "The email does not exist.");
            }

            User user = _dapper.LoadDataSingle<User>(sqlExistUserEmail);

            string sqlGetAuth = @"SELECT * FROM Auth WHERE UserID = " + user.UserID.ToString();

            Auth userAuth = _dapper.LoadDataSingle<Auth>(sqlGetAuth);
            byte[] passwordHash = _authHelper.GetPasswordHash(userLogin.Password, userAuth.PasswordSalt);
            
            for (int index = 0; index < passwordHash.Length; index++)
            {
                if(passwordHash[index] != userAuth.PasswordHash[index])
                {
                    return StatusCode(401, "Incorrect Password");
                }
            }
            
            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(user.UserID)}
            });
        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value + "";

            string userIdSql = @"
                SELECT UserID FROM [User]
                WHERE UserID = " + userId.ToString();
            
            int userIdFromDB = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userIdFromDB)},
            });
        }

    }
}