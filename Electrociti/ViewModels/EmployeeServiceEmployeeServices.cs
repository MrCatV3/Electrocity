using Electrociti.Models;

namespace Electrociti.ViewModels
{
    public class EmployeeServiceEmployeeServices
    {
        public List<Employee> Employee { get; set; }
        public List<Service> Services { get; set; }
        public List<EmployeeService> EmployeeServices { get; set; }
        public List<EmployeeWork> Works { get; set; }
        public string Search { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
