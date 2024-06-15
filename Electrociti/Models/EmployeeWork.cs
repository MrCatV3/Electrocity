namespace Electrociti.Models
{
    public class EmployeeWork
    {
        public int EmployeeWorkId { get; set; }
        public DateTime EmployeeWorkDate { get; set; }
        public string EmployeeWorkTime { get; set; }
        public string EmployeeWorkAdress { get; set; }
        public string EmployeeWorkPhone { get; set; }
        public string EmployeeWorkName { get; set; }
        public string EmployeeWorkStatus { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
