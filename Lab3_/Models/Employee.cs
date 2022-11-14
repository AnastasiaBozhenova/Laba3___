using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Customs.DAL.Models
{
    public class Employee
    {
        [DisplayName("#")]
        public int Id { get; set; }

        [DisplayName("Имя")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string LastName { get; set; }

        [DisplayName("Отчество")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string MiddleName { get; set; }

        [DisplayName("Номер удостоверения")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string IdNumber { get; set; }

        [DisplayName("Должность")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Role { get; set; }



        public List<EmployeeStorage> EmployeeStorages { get; set; }

        public List<Duty> Duties { get; set; }
    }
}