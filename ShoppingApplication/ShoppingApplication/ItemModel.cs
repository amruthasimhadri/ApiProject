using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApplication
{
    public class ItemModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Category")]
        [Required]
        public List<SelectListItem>? CategoryList { get; set; }
        public ItemModel()
        {
            CategoryList = new List<SelectListItem>();
        }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        //[Required]
        //public string ImageUrl { get; set; }
        // public string CategoryName { get; set; }
        [Required]
        public string CategoryId { get; set; }

        [DisplayName("Image")]
        [Required(ErrorMessage = "Please select an image.")]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
        public byte[] ImageData { get; set; }
    }
}
