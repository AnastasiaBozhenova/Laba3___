using System.Collections.Generic;
using Customs.DAL.Models;

namespace Lab3_.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Storage> Storages { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}