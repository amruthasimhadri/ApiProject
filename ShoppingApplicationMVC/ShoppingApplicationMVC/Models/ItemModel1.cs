using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShoppingApplicationMVC.Models
{
    public class ItemModel1
    {
        
       
        public string Name { get; set; }
        
       
        public string Description { get; set; }
        
        public decimal? Price { get; set; }
        

        public string CategoryId { get; set; }

       
        
        public byte[] ImageData { get; set; }
    }
}
