using Microsoft.JSInterop.Implementation;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingApplicationMVC.Models

{
    public class ApiService
    {

        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<UserDetails> GetAllUserDetails()
        {
            HttpResponseMessage response = _httpClient.GetAsync("https://localhost:7189/api/Shopping/GetAllUserDetails").Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            List<UserDetails> userList = JsonConvert.DeserializeObject<List<UserDetails>>(jsonContent);
            return userList;
        }
        public bool FindUser(string email)
        {
            string apiUrl = $"https://localhost:7189/api/Shopping/FindUser?email={email}";
            var response = _httpClient.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<bool>().Result;
            }

            return false;
        }
        public bool TestPassword(User user)
        {
            string apiUrl = $"https://localhost:7189/api/Shopping/TestPassword?email={user.Email}&password={user.Password}";
            var response = _httpClient.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<bool>().Result;
            }
            return false;
        }
        public string GetUserName(string email)
        {
            string apiUrl = $"https://localhost:7189/api/Shopping/GetUserName?email={email}";
            var response = _httpClient.GetAsync(apiUrl).Result;

            if (response.IsSuccessStatusCode)
            {

                return response.Content.ReadAsStringAsync().Result;
            }
            return null;
        }
        public string FindRole(string email)
        {
            string apiUrl = $"https://localhost:7189/api/Shopping/FindRole?email={email}";
            var response = _httpClient.GetAsync(apiUrl).Result;

            if (response.IsSuccessStatusCode)
            {

                return response.Content.ReadAsStringAsync().Result;
            }
            return null;
        }
        public ItemModel GetItemModel()
        {

            string apiUrl = "https://localhost:7189/api/Shopping/GetItemModel";
            var response = _httpClient.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var itemModel = response.Content.ReadFromJsonAsync<ItemModel>().Result;
                return itemModel;
            }
            return null;
        }
        public void AddItem(ItemModel1 model)
        {
            string apiUrl = "https://localhost:7189/api/Shopping/AddItem";


            var response = _httpClient.PostAsJsonAsync(apiUrl, model).Result;

        }
        public List<AdminModel> GetItemList()
        {
            HttpResponseMessage response = _httpClient.GetAsync("https://localhost:7189/api/Shopping/GetItemList").Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            List<AdminModel> items = JsonConvert.DeserializeObject<List<AdminModel>>(jsonContent);
            return items;
        }
        public List<AdminModel> GetUserList()
        {
            HttpResponseMessage response = _httpClient.GetAsync("https://localhost:7189/api/Shopping/GetUserList").Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            List<AdminModel> items = JsonConvert.DeserializeObject<List<AdminModel>>(jsonContent);
            return items;
        }
        public List<AdminModel> GetItemListByCategory(int selectedId)
        {
            string apiUrl = $"https://localhost:7189/api/Shopping/GetItemListByCategory?Id={selectedId}";
           
            HttpResponseMessage response = _httpClient.GetAsync(apiUrl).Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            List<AdminModel> items = JsonConvert.DeserializeObject<List<AdminModel>>(jsonContent);
            return items;
        }
        public List<AdminModel> GetUserListByCategory(int selectedId)
        {
            string apiUrl = $"https://localhost:7189/api/Shopping/GetUserListByCategory?Id={selectedId}";

            HttpResponseMessage response = _httpClient.GetAsync(apiUrl).Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            List<AdminModel> items = JsonConvert.DeserializeObject<List<AdminModel>>(jsonContent);
            return items;
        }
        public List<SelectListItem> GetDropDown()
        {
            HttpResponseMessage response = _httpClient.GetAsync("https://localhost:7189/api/Shopping/GetDropDown").Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(jsonContent);
            return items;
        }
        public void ApproveItem(int Id)
        {
            string apiUrl = "https://localhost:7189/api/Shopping/ApproveItem";


            var response = _httpClient.PostAsJsonAsync(apiUrl, Id).Result;

        }

    }
}

            
        

    
