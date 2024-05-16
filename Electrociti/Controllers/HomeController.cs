using Electrociti.Data;
using Electrociti.Models;
using Electrociti.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Electrociti.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ViewResult Index(string searchString, int? minCost, int? maxCost, bool rate, int MasterId)
        {

            List<Employee> searchResults;
            if (minCost == null)
            {
                minCost = 1;
            }
            if (maxCost == null)
            {
                maxCost = 999999;
            }
            if (string.IsNullOrEmpty(searchString))
            {
                searchResults = _context.EmployeeService.Where(e => e.Service.ServiceCost >= minCost && e.Service.ServiceCost <= maxCost)
                    .GroupBy(e => e.Employee.EmployeeId)
                    .Select(e => e.First().Employee)
                    .ToList();
            }
            else
            {
                if (rate == true)
                {
                    searchResults = _context.EmployeeService
                    .Where(e => (e.Employee.EmployeeAddress.Contains(searchString) ||
                         e.Employee.EmployeeDescription.Contains(searchString) ||
                         e.Service.ServiceName.Contains(searchString)) &&
                        e.Service.ServiceCost >= minCost &&
                        e.Service.ServiceCost <= maxCost && int.Parse(e.Employee.EmployeeRate) >= 4)
                    .GroupBy(e => e.Employee.EmployeeId)
                    .Select(e => e.First().Employee)
                    .ToList();
                }
                else
                {
                    searchResults = _context.EmployeeService
                        .Where(e => (e.Employee.EmployeeAddress.Contains(searchString) ||
                             e.Employee.EmployeeDescription.Contains(searchString) ||
                             e.Service.ServiceName.Contains(searchString)) &&
                            e.Service.ServiceCost >= minCost &&
                            e.Service.ServiceCost <= maxCost)
                        .GroupBy(e => e.Employee.EmployeeId)
                        .Select(e => e.First().Employee)
                        .ToList();
                }
            }

            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = searchResults,
                Services = _context.Service.ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
            };

            return View(VM);

        }

        public IActionResult EP()
        {
            return View();
        }
        public IActionResult EmployeeProfile()
        {
            int? EmployeeId = HttpContext.Session.GetInt32("EmployeeId");
            if (EmployeeId == null)
            {
                return View("Login");
            }
            Employee? employee = _context.Employee2.Where(e => e.EmployeeId == EmployeeId).FirstOrDefault();
            return View("EmployeeProfile", employee);
        }
        [HttpGet]
        public IActionResult Master(int EmployeeId)
        {
            var employeeServices = _context.EmployeeService
                .Include(es => es.Service)
                .Where(es => es.EmployeeId == EmployeeId)
                .ToList();

            var employee = _context.Employee2.Where(e => e.EmployeeId == EmployeeId).ToList();

            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = employee,
                Services = employeeServices.Select(es => es.Service).ToList(),
                EmployeeServices = employeeServices
            };

            return View(VM);
        }



        public IActionResult Folowing()
        {
            return View();
        }

        public IActionResult Admin(string searchString, int? minCost, int? maxCost, bool rate, int MasterId)
        {
            List<Employee> searchResults;
            if (minCost == null)
            {
                minCost = 1;
            }
            if (maxCost == null)
            {
                maxCost = 999999;
            }
            if (string.IsNullOrEmpty(searchString))
            {
                searchResults = _context.EmployeeService.Where(e => e.Service.ServiceCost >= minCost && e.Service.ServiceCost <= maxCost)
                    .GroupBy(e => e.Employee.EmployeeId)
                    .Select(e => e.First().Employee)
                    .ToList();
            }
            else
            {
                if (rate == true)
                {
                    searchResults = _context.EmployeeService
                    .Where(e => (e.Employee.EmployeeAddress.Contains(searchString) ||
                         e.Employee.EmployeeDescription.Contains(searchString) ||
                         e.Service.ServiceName.Contains(searchString)) &&
                        e.Service.ServiceCost >= minCost &&
                        e.Service.ServiceCost <= maxCost && int.Parse(e.Employee.EmployeeRate) >= 4)
                    .GroupBy(e => e.Employee.EmployeeId)
                    .Select(e => e.First().Employee)
                    .ToList();
                }
                else
                {
                    searchResults = _context.EmployeeService
                        .Where(e => (e.Employee.EmployeeAddress.Contains(searchString) ||
                             e.Employee.EmployeeDescription.Contains(searchString) ||
                             e.Service.ServiceName.Contains(searchString)) &&
                            e.Service.ServiceCost >= minCost &&
                            e.Service.ServiceCost <= maxCost)
                        .GroupBy(e => e.Employee.EmployeeId)
                        .Select(e => e.First().Employee)
                        .ToList();
                }
            }

            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = searchResults,
                Services = _context.Service.ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
            };
            return View(VM);
        }
        public IActionResult Services()
        {
            List<Service> services;
            services = _context.Service.ToList();
            return View(services);
        }
        public IActionResult Employees()
        {
            List<Employee> employees;
            employees = _context.Employee2.ToList();
            return View(employees);
        }
        public IActionResult Test()
        {
            List<Employee> employees;
            employees = _context.Employee2.ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Login()
        {

            int? EmployeeId = HttpContext.Session.GetInt32("EmployeeId");
            
            if (EmployeeId != null)
            {
                return View("Index");
            }
            else 
            { 
                return View(); 
            }
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
            {
            var employeeService = new EmployeeServiceLogin(_context);
            var employee = employeeService.GetEmployee(login, password);
            if (employee != null)
            {
                HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);
                HttpContext.Session.SetInt32("EmployeeRole", employee.EmployeeRole);
            }

            else
            {
                return View();
            }
            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = _context.Employee2.ToList(),
                Services = _context.Service.ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
            };
            if (employee != null && employee.EmployeeRole != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (employee != null && employee.EmployeeRole == 1)
            {
                return View("Admin", VM);
            }
            else
            {
                
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult UpdateEmployeeProfile()
        {
            int employeeId = HttpContext.Session.GetInt32("EmployeeId").Value;
            var employee = _context.Employee2.Find(employeeId);

            return View(employee);
        }
        [HttpPost]
        public IActionResult UpdateEmployeeProfile(Employee updatedEmployee, string Password)
        {

            var existingEmployee = _context.Employee2.Find(updatedEmployee.EmployeeId);

            if (existingEmployee != null)
            {
                existingEmployee.EmployeeName = updatedEmployee.EmployeeName;
                existingEmployee.EmployeeAddress = updatedEmployee.EmployeeAddress;
                existingEmployee.EmployeePhone = updatedEmployee.EmployeePhone;
                if (existingEmployee.EmployeePassword == updatedEmployee.EmployeePassword)
                {
                    if (!string.IsNullOrEmpty(updatedEmployee.EmployeePassword))
                    {
                        existingEmployee.EmployeePassword = Password;
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(updatedEmployee);

        }

        public IActionResult Register()
        {
            return View();
        }

        /*[HttpPost]
        public ActionResult Register(Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var context = new ApplicationContext())
                {
                    if (context.Employee.Any(u => u.EmployeeName == employee.EmployeeName))
                    {
                        ModelState.AddModelError("Username", "Пользователь с таким именем уже существует.");
                        return View(employee);
                    }
                    employee.EmployeePassword = HashPassword(employee.EmployeePassword);
                    context.Employee.Add(employee);
                    context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(employee);
        }*/

        //private string HashPassword(string password)
        //{
        //    string salt = "RandomSalt123";
        //    return Convert.ToBase64String(Encoding.UTF8.GetBytes(password + salt));
        //}



        
    }
}