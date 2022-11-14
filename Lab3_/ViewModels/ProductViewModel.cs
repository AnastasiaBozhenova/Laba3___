using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab3_.ViewModels
{
    public class ProductViewModel
    {
        [DisplayName("#")]
        public int Id { get; set; }

        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Name { get; set; }

        [DisplayName("Единица измерения")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int UnitMeasurement { get; set; }

        [DisplayName("Склад")]
        public string Storage { get; set; }

        public SortViewModel SortViewModel { get; set; }
    }
}