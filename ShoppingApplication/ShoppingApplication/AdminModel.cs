﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ShoppingApplication
{
    public class AdminModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; }
        public string CategoryName { get; set; }
        
    }
}
