using System.Collections.Generic;
using Customs.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab3_.ViewModels
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public ProductViewModel ProductViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SelectList ListYears { get; set; }
    }
}