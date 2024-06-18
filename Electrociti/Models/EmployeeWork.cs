using System.ComponentModel.DataAnnotations;

namespace Electrociti.Models
{
    public class EmployeeWork
    {
        public int EmployeeWorkId { get; set; }
        public DateTime EmployeeWorkDate { get; set; }

        public string EmployeeWorkTime { get; set; }
        [Required(ErrorMessage = "Поле 'Адрес клиента' заполнено неправильно.")]
        public string EmployeeWorkAdress { get; set; }
        [Required(ErrorMessage = "Поле 'Телефон клиента' заполнено неправильно.")]
        public string EmployeeWorkPhone { get; set; }
        [Required(ErrorMessage = "Поле 'Имя клиента' заполнено неправильно.")]
        public string EmployeeWorkName { get; set; }
        public string EmployeeWorkStatus { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
