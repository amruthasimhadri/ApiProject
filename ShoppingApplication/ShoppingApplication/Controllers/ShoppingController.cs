using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace ShoppingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection con;
        public ShoppingController(IConfiguration configuration) 
        {
            _configuration = configuration;
            con = new SqlConnection(_configuration.GetConnectionString("connectionstring"));
            con.Open();
        }
        
        [HttpGet]
        [Route("GetAllUserDetails")]
        public ActionResult<IEnumerable<UserDetails>> GetAllUserDetails()
        {
            List<UserDetails> userdetails = new List<UserDetails>();
            SqlCommand cmd = new SqlCommand("GetAllUserDetails", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                UserDetails userDetails = new UserDetails
                {
                    Id = (int)dr["Id"],
                    Name = dr["Name"].ToString(), // Adjust property names based on your actual class
                    Email = dr["Email"].ToString(),
                    Password = dr["Password"].ToString(),
                    Role = dr["Role"].ToString()
                    // Add other properties as needed
                };
                userdetails.Add(userDetails);
            }
            return Json(userdetails);
        }
        [HttpGet("{email}")]
        [Route("FindUser")]
        public ActionResult<bool> FindUser([FromQuery] string email)
        {
            SqlCommand cmd = new SqlCommand("GetAllUserDetails", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var emaill = (string)reader["Email"];
                    if (emaill == email)
                    {
                        return Ok(true);
                    }
                }
            }

            return Ok(false);
        }
        [HttpGet]
        [Route("TestPassword")]
        public ActionResult<bool> TestPassword([FromQuery] string email, [FromQuery] string password)
        {
            SqlCommand sqlCommand = new SqlCommand("GetPassword", con);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@email", email);
            string Password=sqlCommand.ExecuteScalar().ToString();
            

                if (Password==password)
                {
                   
                    return true;
                }
            

           
            return false;
        }
        [HttpGet]
        [Route("GetUserName")]
        public ActionResult<string> GetUserName([FromQuery] string email)
        {
            SqlCommand cmd = new SqlCommand("GetUserName", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);

            
            string username = cmd.ExecuteScalar()?.ToString();

            if (!string.IsNullOrEmpty(username))
            {
                return Ok(username);
            }
            else
            {
                return NotFound(); 
            }
        }
        [HttpGet]
        [Route("FindRole")]
        public ActionResult<string> FindRole([FromQuery] string email)
        {
            SqlCommand cmd = new SqlCommand("GetRole", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);

            string role = cmd.ExecuteScalar()?.ToString();

            if (!string.IsNullOrEmpty(role))
            {
                return Ok(role);
            }
            else
            {
                return NotFound(); 
            }
        }

        [HttpGet]
        [Route("GetItemModel")]
        public ActionResult<ItemModel> GetItemModel()
        {
            // Replace this with your logic to retrieve an inventory item
            ItemModel model = new ItemModel();

            SqlCommand cmd = new SqlCommand("GetCategories", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            
            while (dr.Read())
            {

                model.CategoryList.Add(new SelectListItem { Text = dr.GetString(1), Value = dr.GetString(0) });
            }
            // Return the inventory item along with an HTTP 200 status code
            return Ok(model);
        }

        [HttpPost]
        [Route("AddItem")]
        public void AddItem([FromBody] ItemModel1 item)
        {
            SqlCommand cmd = new SqlCommand("AddItem",con);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name",item.Name);
            cmd.Parameters.AddWithValue("@CategoryId", item.CategoryId);
            cmd.Parameters.AddWithValue("@Description", item.Description);
            cmd.Parameters.AddWithValue("@Price", item.Price);
            cmd.Parameters.AddWithValue("@ImageUrl", item.ImageData);
            
            cmd.ExecuteNonQuery();
            
        }
        
        [HttpPost]
        [Route("ApproveItem")]
        public void ApproveItem([FromBody] int Id)
        {
            SqlCommand cmd = new SqlCommand("ApproveItem", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            
            cmd.ExecuteNonQuery();

        }
        [HttpGet]
        [Route("GetItemList")]
        public ActionResult<IEnumerable<AdminModel>> GetItemList()
        {
            List<AdminModel> items = new List<AdminModel>();
            SqlCommand cmd = new SqlCommand("GetItems", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                AdminModel item = new AdminModel
                {
                    Id = (int)dr["Id"],
                    Name = dr["Name"].ToString(), // Adjust property names based on your actual class
                    CategoryName = dr["CategoryName"].ToString(),
                    Description = dr["Description"].ToString(),
                    ImageData = (byte[])dr["ImageUrl"],
                    Price = dr.GetDecimal(dr.GetOrdinal("Price"))
                    // Add other properties as needed
                };
                items.Add(item);
            }
            return Json(items);
        }
        [HttpGet]
        [Route("GetItemListByCategory")]
        public ActionResult<IEnumerable<AdminModel>> GetItemListByCategory([FromQuery] int Id)
        {
            List<AdminModel> items = new List<AdminModel>();
            SqlCommand cmd = new SqlCommand("GetItemsByCategory", con);
            cmd.CommandType=CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                AdminModel item = new AdminModel
                {
                    Id = (int)dr["Id"],
                    Name = dr["Name"].ToString(), // Adjust property names based on your actual class
                    CategoryName = dr["CategoryName"].ToString(),
                    Description = dr["Description"].ToString(),
                    ImageData = (byte[])dr["ImageUrl"],
                    Price = dr.GetDecimal(dr.GetOrdinal("Price"))
                    // Add other properties as needed
                };
                items.Add(item);
            }
            return Json(items);
        }

        [HttpGet]
        [Route("GetUserList")]
        public ActionResult<IEnumerable<AdminModel>> GetUserList()
        {
            List<AdminModel> items = new List<AdminModel>();
            SqlCommand cmd = new SqlCommand("GetUserItems", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                AdminModel item = new AdminModel
                {
                    Id = (int)dr["Id"],
                    Name = dr["Name"].ToString(), // Adjust property names based on your actual class
                    CategoryName = dr["CategoryName"].ToString(),
                    Description = dr["Description"].ToString(),
                    ImageData = (byte[])dr["ImageUrl"],
                    Price = dr.GetDecimal(dr.GetOrdinal("Price"))
                    // Add other properties as needed
                };
                items.Add(item);
            }
            return Json(items);
        }
        [HttpGet]
        [Route("GetUserListByCategory")]
        public ActionResult<IEnumerable<AdminModel>> GetUserListByCategory([FromQuery] int Id)
        {
            List<AdminModel> items = new List<AdminModel>();
            SqlCommand cmd = new SqlCommand("GetUserItemsByCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                AdminModel item = new AdminModel
                {
                    Id = (int)dr["Id"],
                    Name = dr["Name"].ToString(), // Adjust property names based on your actual class
                    CategoryName = dr["CategoryName"].ToString(),
                    Description = dr["Description"].ToString(),
                    ImageData = (byte[])dr["ImageUrl"],
                    Price = dr.GetDecimal(dr.GetOrdinal("Price"))
                    // Add other properties as needed
                };
                items.Add(item);
            }
            return Json(items);
        }


        [HttpGet]
        [Route("GetDropDown")]
        public ActionResult<IEnumerable<SelectListItem>> GetDropDown()
        {
            List<SelectListItem> dropdownValues = new List<SelectListItem>();

            SqlCommand command = new SqlCommand("SELECT Id, Name FROM Categories", con);

            SqlDataReader reader = command.ExecuteReader();
            dropdownValues.Add(new SelectListItem {  Value= null,Text = "All Categories" });
                        while (reader.Read())
                        {
                            string id = reader.GetString(0);
                            string name = reader.GetString(1);

                            dropdownValues.Add(new SelectListItem { Value = id , Text = name });
                        }

                    
                
            


            return Json(dropdownValues);
        }


        public IActionResult Index()
        {
            
            return View();
        }
    }
}
