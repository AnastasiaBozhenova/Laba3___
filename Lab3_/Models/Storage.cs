using System.Collections.Generic;
using System.ComponentModel;

namespace Customs.DAL.Models
{
    public class Storage
    {
        [DisplayName("#")]
        public int Id { get; set; }

        [DisplayName("Название склада")]
        public string Name { get; set; }


        public List<Product> Products { get; set; }

        public List<EmployeeStorage> EmployeeStorages { get; set; }

        public List<Duty> Duties { get; set; }
    }
}