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
            //if (minCost == null)
            //{
            //    minCost = 1;
            //}
            //if (maxCost == null)
            //{
            //    maxCost = 999999;
            //}
            //if (string.IsNullOrEmpty(searchString))
            //{
            //    searchResults = _context.EmployeeService.Where(e => e.Service.ServiceCost >= minCost && e.Service.ServiceCost <= maxCost)
            //        .GroupBy(e => e.Employee.EmployeeId)
            //        .Select(e => e.First().Employee)
            //        .ToList();
            //}
            //else
            //{
            //    if (rate == true)
            //    {
            //        searchResults = _context.EmployeeService
            //        .Where(e => (e.Employee.EmployeeAddress.Contains(searchString) ||
            //             e.Employee.EmployeeDescription.Contains(searchString) ||
            //             e.Service.ServiceName.Contains(searchString)) &&
            //            e.Service.ServiceCost >= minCost &&
            //            e.Service.ServiceCost <= maxCost && int.Parse(e.Employee.EmployeeRate) >= 4)
            //        .GroupBy(e => e.Employee.EmployeeId)
            //        .Select(e => e.First().Employee)
            //        .ToList();
            //    }
            //    else
            //    {
            //        searchResults = _context.EmployeeService
            //            .Where(e => (e.Employee.EmployeeAddress.Contains(searchString) ||
            //                 e.Employee.EmployeeDescription.Contains(searchString) ||
            //                 e.Service.ServiceName.Contains(searchString)) &&
            //                e.Service.ServiceCost >= minCost &&
            //                e.Service.ServiceCost <= maxCost)
            //            .GroupBy(e => e.Employee.EmployeeId)
            //            .Select(e => e.First().Employee)
            //            .ToList();
            //    }
            //}

            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = searchResults = _context.Employee2.ToList(),
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
        [HttpPost]
        public IActionResult DeleteService(int serviceId)
        {
            var service = _context.Service.Find(serviceId);
            if (service != null)
            {
                _context.Service.Remove(service);
                _context.SaveChanges();
            }
            return RedirectToAction("Services");
        }
        [HttpGet]
        public IActionResult EditService(int serviceId)
        {
            Service service1 = _context.Service.Find(serviceId);
            return View(service1);
        }
        [HttpPost]
        public IActionResult EditService(Service updateService)
        {
            var existingService = _context.Service.Find(updateService.ServiceId);
            if (existingService != null)
            {
                existingService.ServiceName = updateService.ServiceName;
                existingService.ServiceCost = updateService.ServiceCost;

                _context.SaveChanges();
                return RedirectToAction("Services");
            }
            return View(existingService);

        }
        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddService(string ServiceName, int ServiceCost)
        {
            Service newService = new Service();
            newService.ServiceName = ServiceName;
            newService.ServiceCost = ServiceCost;
            _context.Service.Add(newService);
            _context.SaveChanges();
            return RedirectToAction("Services");
        }
        [HttpGet]
        public IActionResult AddMaster()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddMaster(string Image,string EmployeeName, string SecondName, string Patronomic, string EmployeeDescription, string EmployeeAddress, string EmployeePhone)
        {
            Employee newEmployee = new Employee();
            newEmployee.EmployeeName = EmployeeName;
            newEmployee.EmployeeSurname = SecondName;
            newEmployee.EmployeePatronomic = Patronomic;
            newEmployee.EmployeeDescription = EmployeeDescription;
            newEmployee.EmployeeAddress = EmployeeAddress;
            newEmployee.EmployeePhone = EmployeePhone;
            _context.Add(newEmployee);
            _context.SaveChanges();
            return RedirectToAction("Admin");
        }
        [HttpPost]
        public IActionResult DeleteMaster(int employeeId)
        {
            var employee = _context.Employee2.Find(employeeId);
            if (employee != null)
            {
                _context.Employee2.Remove(employee);
                _context.SaveChanges();
            }

            return RedirectToAction("Admin");
        }
        [HttpGet]
        public IActionResult EditMaster(int EmployeeId)
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
        [HttpPost]
        public IActionResult EditMaster()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EditMasterAdmin(int employeeId)
        {
            Employee employee = _context.Employee2.Find(employeeId);
            return View(employee);
        }
        [HttpPost]
        public IActionResult EditMasterAdmin(Employee updateEmployee)
        {
            var existingEmployee = _context.Employee2.Find(updateEmployee.EmployeeId);
            if (existingEmployee != null)
            {
                existingEmployee.EmployeeName = updateEmployee.EmployeeName;
                existingEmployee.EmployeeDescription = updateEmployee.EmployeeDescription;
                existingEmployee.EmployeeAddress = updateEmployee.EmployeeAddress;
                existingEmployee.EmployeePhone = updateEmployee.EmployeePhone;

                _context.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(existingEmployee);
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

        

       


        
    }
}