using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShoppingController : Controller
    {
        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (IsValidUser(model))
            {
                string role = GetRoleFromDatabase(model.Email);
                return Json(new LoginResponse { Success = true, Role = role });
            }

            return Json(new LoginResponse { Success = false });
        }

        private bool IsValidUser(Login model)
        {
            var connectionString = "Data Source=ASIMHADR-L-5515\\SQLEXPRESS;Initial Catalog=Shopping;User ID=sa;Password=Welcome2evoke@1234";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Password", model.Password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        private string GetRoleFromDatabase(string email)
        {
            var connectionString = "Data Source=ASIMHADR-L-5515\\SQLEXPRESS;Initial Catalog=Shopping;User ID=sa;Password=Welcome2evoke@1234";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Role FROM Users WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    var result = command.ExecuteScalar();

                    // Check if the result is null
                    if (result != null)
                    {
                        return result.ToString();
                    }

                    // Handle the case where the email does not exist in the database
                    return null;
                }
            }
        }
    }
}
