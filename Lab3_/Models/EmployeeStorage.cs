namespace Customs.DAL.Models
{
    public class EmployeeStorage
    {
        public int EmployeeId { get; set; }
        public int StorageId { get; set; }

        public Employee Employee { get; set; }
        public Storage Storage { get; set; }
    }
}