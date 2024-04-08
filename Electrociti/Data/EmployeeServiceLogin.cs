using Electrociti.Models;

namespace Electrociti.Data
{
    public class EmployeeServiceLogin
    {
        private List<Employee> _employee;
        private readonly ApplicationContext _context;
        public EmployeeServiceLogin(ApplicationContext context)
        {
            _context = context; 
            _employee = _context.Employee2.ToList();
        }
        public Employee GetEmployee(string login, string password) 
        {
            return _employee.FirstOrDefault(e => e.EmployeePhone == login && e.EmployeePassword == password);
        }
    }
}
