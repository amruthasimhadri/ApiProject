using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingApplicationMVC.Models;
using System.Data;
using System.Reflection;

namespace ShoppingApplicationMVC.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly ApiService _apiService;
        public ShoppingController(ApiService apiService)
        {
            _apiService = apiService;
        }
        public IActionResult Index()
        {
            List<UserDetails> userDetails = _apiService.GetAllUserDetails();
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Admin()
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                //string successMessage = TempData["SuccessMessage"] as string;
               // ViewBag.SuccessMessage = successMessage;
                List<AdminModel> items = _apiService.GetItemList();

                List<SelectListItem> dropdownValues =  _apiService.GetDropDown();

                ViewBag.DropDownValues = dropdownValues;
                return View("Admin", items);
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Admin(int selectedId)
        {
            if (selectedId == 0)
            {
                return RedirectToAction("Admin");
            }
            List<AdminModel> items = _apiService.GetItemListByCategory(selectedId);
            List<SelectListItem> dropdownValues = _apiService.GetDropDown();

            ViewBag.DropDownValues = dropdownValues;
            return View(items);
        }
        public IActionResult Inventory()
        {
            ItemModel model = _apiService.GetItemModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Inventory(ItemModel item)
        {
            if (HttpContext.Session.GetString("UserRole") == "inventory")
            {
                using (var stream = new MemoryStream())
                {
                    item.ImageFile.CopyTo(stream);
                    item.ImageData = stream.ToArray();
                }
                ItemModel1 item1 = new ItemModel1();
                item1.Name = item.Name;
                item1.Description = item.Description;
                item1.ImageData = item.ImageData;
                item1.Price= item.Price;
                item1.CategoryId = item.CategoryId;
                _apiService.AddItem(item1);
                ViewBag.ResponseMessage = "Product added successfully";
                ItemModel model = _apiService.GetItemModel();
                return View(model);
            }
            return RedirectToAction("Login");
        }
        public IActionResult User()
        {
            if (HttpContext.Session.GetString("UserRole") == "user")
            {
                List<SelectListItem> dropdownValues = _apiService.GetDropDown();

                ViewBag.DropDownValues = dropdownValues;
                List<AdminModel> items = _apiService.GetUserList();
                return View("User",items);
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult User(int selectedId)
        {
            if (selectedId == 0)
            {
                return RedirectToAction("User");
            }
            List<AdminModel> items = _apiService.GetUserListByCategory(selectedId);
            List<SelectListItem> dropdownValues = _apiService.GetDropDown();

            ViewBag.DropDownValues = dropdownValues;
            return View(items);
        }

        [HttpPost]
        public IActionResult Approve(int Id)
        {
            _apiService.ApproveItem(Id);
            TempData["SuccessMessage"] = "Item Approved";
            return RedirectToAction("Admin");

        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                if (_apiService.FindUser(user.Email))
                {
                    if (_apiService.TestPassword(user))
                    {
                        string username = _apiService.GetUserName(user.Email);
                        HttpContext.Session.SetString("UserName", username);
                        string role = _apiService.FindRole(user.Email);
                        HttpContext.Session.SetString("UserRole", role);
                        if (role == "admin")
                        {
                            return RedirectToAction("Admin");
                        }
                        else if (role == "inventory")
                        {
                            return RedirectToAction("Inventory");
                        }
                        else if(role == "user")
                        {
                            return RedirectToAction("User");
                        }
                    }

                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect Password");
                        return View(user);
                    }
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Your Email Is Not Registered");
                        return View(user);
                    }

                }

                return View(user);
            }
           
        }
    }
