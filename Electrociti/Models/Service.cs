using System.ComponentModel.DataAnnotations;

namespace Electrociti.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        
        public int ServiceCost { get; set; }

        [Required(ErrorMessage = "Название услуги не может содержать только пробелы.")]
        public string ServiceName { get; set; }
    }
}
