using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Customs.DAL.Models
{
    public class Duty
    {
        [DisplayName("#")]
        public int Id { get; set; }

        [DisplayName("Номер скалада")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int StorageId { get; set; }

        public Storage Storage { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [DisplayName("Дата поступления товара")]
        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime DateOfReceipt { get; set; }

        [DisplayName("Количество поступления товара")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int AmountOfReceipt { get; set; }

        [DisplayName("Номер таможненного документа")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int CustomsDocumentNumber { get; set; }

        [DisplayName("Таможненный агент")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [DisplayName("Размер пошлины за весь товар")]
        [Required(ErrorMessage = "Обязательное поле")]
        public double DutyForAllProducts { get; set; }

        [DisplayName("Дата уплаты пошлины")]
        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime DutyPaymentDate { get; set; }

        [DisplayName("Дата вывоза товара")]
        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime ExportProductsDate { get; set; }
    }
}