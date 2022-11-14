using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Customs.DAL.Models
{
    public class Product
    {
        [DisplayName("#")]
        public int Id { get; set; }

        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Name { get; set; }

        [DisplayName("Единица измерения")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int UnitMeasurement { get; set; }

        public int StorageId { get; set; }


        public Storage Storage { get; set; }

        public Duty Duty { get; set; }
    }
}