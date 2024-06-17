﻿using Electrociti.Data;
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
using System.Drawing.Printing;

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

        public IActionResult Index(string searchString, int? minCost, int? maxCost, bool? rate, bool? byMaterial, int experience = 1, int pageNumber = 1, int pageSize = 3)
        {
            var query = _context.EmployeeService.AsQueryable();

            if (minCost.HasValue)
            {
                query = query.Where(es => es.Service.ServiceCost >= minCost.Value);
            }
            if (maxCost.HasValue)
            {
                query = query.Where(es => es.Service.ServiceCost <= maxCost.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(es => es.Employee.EmployeeAddress.Contains(searchString) ||
                                          es.Employee.EmployeeDescription.Contains(searchString) ||
                                          es.Service.ServiceName.Contains(searchString));
            }

            if (rate == true)
            {
                query = query.Where(es => int.Parse(es.Employee.EmployeeRate) >= 4);
            }

            if (byMaterial == true)
            {
                query = query.Where(es => es.Employee.EmployeeRole == 2); 
            }

            if (experience > 1)
            {
                var minYears = experience - 1;
                var cutoffDate = DateTime.Now.AddYears(-minYears);
                query = query.Where(es => es.Employee.EmployeeRegistrationDate <= cutoffDate);
            }

            var groupedResults = query.GroupBy(es => es.Employee.EmployeeId)
                                      .Select(g => g.First().Employee)
                                      .ToList();

            int totalItems = groupedResults.Count;
            var employeesOnPage = groupedResults.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var VM = new EmployeeServiceEmployeeServices
            {
                Employee = employeesOnPage,
                Services = _context.Service.ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
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
            var employeeServices = _context.EmployeeService
            .Include(es => es.Service)
            .Where(es => es.EmployeeId == EmployeeId)
            .ToList();
            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = _context.Employee.Where(e => e.EmployeeId == EmployeeId).ToList(),
                Services = employeeServices.Select(es => es.Service).ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
                Works = _context.EmployeeWork.Where(e => e.EmployeeId == EmployeeId).ToList(),
            };

            return View("EmployeeProfile", VM);
        }

        [HttpGet]
        public IActionResult Master(int EmployeeId)
        {
            var employeeServices = _context.EmployeeService
                .Include(es => es.Service)
                .Where(es => es.EmployeeId == EmployeeId)
                .ToList();

            var employee = _context.Employee.Where(e => e.EmployeeId == EmployeeId).ToList();

            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = employee,
                Services = employeeServices.Select(es => es.Service).ToList(),
                EmployeeServices = employeeServices,
            };

            return View(VM);
        }

        public IActionResult Folowing()
        {
            return View();
        }

        public IActionResult Admin(string searchString, int? minCost, int? maxCost, bool rate, int pageNumber = 1, int pageSize = 3)
        {
            IQueryable<Employee> query = _context.Employee
                                          .Where(e => e.EmployeeRole == 2);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e => e.EmployeeName.Contains(searchString));
            }

            if (rate)
            {
                query = query.OrderByDescending(e => e.EmployeeRate);
            }

            int totalItems = query.Count();

            var employeesOnPage = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = employeesOnPage,
                Services = _context.Service.ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return View(VM);
        }


        public IActionResult Services(int pageNumber = 1, int pageSize = 11)
        {
            var services = _context.Service.ToList();

            int totalItems = services.Count;
            var servicesOnPage = services.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ServicesViewModel viewModel = new ServicesViewModel
            {
                Service = servicesOnPage,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return View(viewModel);
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
        [HttpPost]
        public IActionResult DeleteEmployeeService(int employeeId, int serviceId)
        {

            var employeeService = _context.EmployeeService
        .FirstOrDefault(es => es.EmployeeId == employeeId && es.ServiceId == serviceId);

            if (employeeService != null)
            {
                _context.EmployeeService.Remove(employeeService);
                _context.SaveChanges();
            }
            int? EmployeeRole = HttpContext.Session.GetInt32("EmployeeRole");
            if (EmployeeRole == 2)
            {
                return RedirectToAction("EmployeeProfile");
            }
            else
            {
                return RedirectToAction("Admin");
            }
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
        public IActionResult AddEmployee()
        {
            DateTime today = DateTime.Today;
            ViewBag.Today = today.ToString("yyyy-MM-dd");

            return View();
        }
        [HttpPost]
        public IActionResult AddEmployee(string Image, string EmployeeName, string SecondName, string? Patronomic, string EmployeeDescription, string EmployeeAddress, string EmployeePhone, DateTime SelectedDate, string EmployeeRate, string EmployeePassword)
        {
            var existingEmployee = _context.Employee.FirstOrDefault(e => e.EmployeePhone == EmployeePhone);
            if (existingEmployee != null)
            {
                ModelState.AddModelError("EmployeePhone", "Сотрудник с таким номером телефона уже существует.");
                return View("AddEmployee");
            }

            // Проверка валидности строки Base64
            if (!IsBase64String(Image))
            {
                ModelState.AddModelError("Image", "Некорректное изображение.");
                return View("AddEmployee");
            }

            DateTime today = DateTime.Today;
            ViewBag.Today = today.ToString("yyyy-MM-dd");
            if (!decimal.TryParse(EmployeeRate, out var rate) || rate < 0 || rate > 5)
            {
                ModelState.AddModelError("EmployeeRate", "Рейтинг должен быть числом от 0 до 5.");
                return View("AddEmployee");
            }
            Employee newEmployee = new Employee
            {
                EmployeeName = EmployeeName,
                EmployeeSurname = SecondName,
                EmployeePatronomic = Patronomic ?? "",
                EmployeeDescription = EmployeeDescription ?? "",
                EmployeeAddress = EmployeeAddress,
                EmployeePhone = EmployeePhone,
                EmployeeRate = EmployeeRate,
                EmployeeRole = 2,
                EmployeePassword = EmployeePassword,
                EmployeeImage = Image,
                EmployeeRegistrationDate = DateTime.Now,
                EmployeeBirthday = SelectedDate
            };

            _context.Add(newEmployee);
            _context.SaveChanges();

            return RedirectToAction("Admin");
        }



        // Метод для проверки валидности строки Base64
        private bool IsBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                return false;
            }

            string base64Data = base64.Contains(",") ? base64.Split(',')[1] : base64;
            Span<byte> buffer = new Span<byte>(new byte[base64Data.Length]);
            return Convert.TryFromBase64String(base64Data, buffer, out _);
        }



        [HttpPost]
        public IActionResult DeleteMaster(int employeeId)
        {
            var employee = _context.Employee.Find(employeeId);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
                _context.SaveChanges();
            }
            int? EmployeeRole = HttpContext.Session.GetInt32("EmployeeRole");
            if (EmployeeRole == 2)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Admin");
            }
            
        }
        [HttpGet]
        public IActionResult EditMaster(int EmployeeId)
        {
            var employeeServices = _context.EmployeeService
            .Include(es => es.Service)
            .Where(es => es.EmployeeId == EmployeeId)
            .ToList();

            var employee = _context.Employee.Where(e => e.EmployeeId == EmployeeId).ToList();

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
            Employee employee = _context.Employee.Find(employeeId);
            return View(employee);
        }
        [HttpPost]
        public IActionResult EditMasterAdmin(Employee updateEmployee)
        {
            var existingEmployee = _context.Employee.Find(updateEmployee.EmployeeId);
            if (existingEmployee != null)
            {
                // Логирование строки Base64 для отладки
                System.Diagnostics.Debug.WriteLine("Base64 Image: " + updateEmployee.EmployeeImage);

                // Проверка валидности строки Base64
                if (!IsBase64String(updateEmployee.EmployeeImage))
                {
                    ModelState.AddModelError("Image", "Некорректное изображение.");
                    return View(updateEmployee);
                }

                existingEmployee.EmployeeImage = updateEmployee.EmployeeImage;
                existingEmployee.EmployeeName = updateEmployee.EmployeeName;
                existingEmployee.EmployeeSurname = updateEmployee.EmployeeSurname;
                existingEmployee.EmployeePatronomic = updateEmployee.EmployeePatronomic ?? "";
                existingEmployee.EmployeeDescription = updateEmployee.EmployeeDescription ?? "";
                existingEmployee.EmployeeAddress = updateEmployee.EmployeeAddress;
                existingEmployee.EmployeePhone = updateEmployee.EmployeePhone;

                _context.SaveChanges();
            }
            
            int? EmployeeRole = HttpContext.Session.GetInt32("EmployeeRole");
            if (EmployeeRole == 1)
            {
                return RedirectToAction("Admin");
            }
            else
            {
                return RedirectToAction("EmployeeProfile");
            }
        }

        public IActionResult Employees()
        {
            List<Employee> employees;
            employees = _context.Employee.ToList();
            return View(employees);
        }
        public IActionResult Test()
        {
            List<Employee> employees;
            employees = _context.Employee.ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Login()
        {

            int? EmployeeId = HttpContext.Session.GetInt32("EmployeeId");

            if (EmployeeId != null)
            {
                return RedirectToAction("Index");
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
                Employee = _context.Employee.ToList(),
                Services = _context.Service.ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
            };
            if (employee != null && employee.EmployeeRole != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (employee != null && employee.EmployeeRole == 1)
            {
                return RedirectToAction("Admin", VM);
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
            var employee = _context.Employee.Find(employeeId);

            return View(employee);
        }
        [HttpPost]
        public IActionResult UpdateEmployeeProfile(Employee updatedEmployee, string Password)
        {

            var existingEmployee = _context.Employee.Find(updatedEmployee.EmployeeId);

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
        [HttpPost]
        public IActionResult AddEmployeeService(int selectedServiceId, int employeeId)
        {
            EmployeeService employeeService = new()
            {
                EmployeeId = employeeId,
                ServiceId = selectedServiceId
            };

            _context.EmployeeService.Add(employeeService);
            _context.SaveChanges();
            int? EmployeeRole = HttpContext.Session.GetInt32("EmployeeRole");
            if (EmployeeRole == 2)
            {
                return RedirectToAction("EmployeeProfile");
            }
            else
            {
                return RedirectToAction("Admin");
            }
        }
        [HttpGet]
        public IActionResult AddEmployeeService(int employeeId)
        {
            var services = _context.Service.ToList();

            ViewBag.EmployeeId = employeeId;
            return View(services);
        }
        
        [HttpGet]
        public IActionResult AddEmployeeWork(int employeeId)
        {
            ViewBag.EmployeeId = employeeId;
            DateTime today = DateTime.Today;
            ViewBag.Today = today.ToString("yyyy-MM-dd");
            return View();
        }
        [HttpPost]
        public IActionResult AddEmployeeWork(string EmployeeWorkAdress, string EmployeeWorkPhone, string EmployeeWorkName, string EmployeeWorkTime, DateTime SelectDate, int employeeId)
        {
            EmployeeWork employeeWork = new()
            {
                EmployeeWorkDate = SelectDate,
                EmployeeWorkTime = EmployeeWorkTime,
                EmployeeWorkAdress = EmployeeWorkAdress,
                EmployeeWorkPhone = EmployeeWorkPhone,
                EmployeeWorkName = EmployeeWorkName,
                EmployeeWorkStatus = "В ожидании",
                EmployeeId = employeeId
            };
            _context.Add(employeeWork);
            _context.SaveChanges();
            int? EmployeeRole = HttpContext.Session.GetInt32("EmployeeRole");
            if (EmployeeRole == 1)
            {
                return RedirectToAction("Admin");
            }
            else
            {
                return RedirectToAction("EmployeeProfile");
            }
        }

        [HttpPost]
        public ActionResult CompleteWork(int workId)
        {
            var employeeWork = _context.EmployeeWork.Find(workId);


            employeeWork.EmployeeWorkStatus = "Выполнено";
            _context.SaveChanges();
            
            int? EmployeeRole = HttpContext.Session.GetInt32("EmployeeRole");
            if (EmployeeRole == 1)
            {
                return RedirectToAction("Admin");
            }
            else
            {
                return RedirectToAction("EmployeeProfile");
            }
        }

        [HttpPost]
        public ActionResult DeleteEmployeeWork(int workId)
        {

            var employeeWork = _context.EmployeeWork.Find(workId);


            employeeWork.EmployeeWorkStatus = "Отклонено";
            _context.SaveChanges();

            return RedirectToAction("EmployeeProfile");
        }

        public IActionResult EmployeeWorks()
        {
            int? EmployeeId = HttpContext.Session.GetInt32("EmployeeId");
            if (EmployeeId == null)
            {
                return View("Login");
            }
            var employeeServices = _context.EmployeeService
            .Include(es => es.Service)
            .Where(es => es.EmployeeId == EmployeeId)
            .ToList();
            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = _context.Employee.Where(e => e.EmployeeId == EmployeeId).ToList(),
                Services = employeeServices.Select(es => es.Service).ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
                Works = _context.EmployeeWork.Where(e => e.EmployeeId == EmployeeId).ToList(),
            };
            return View(VM);
        }
        [HttpPost]
        public IActionResult EmployeeWorks(int employeeId)
        {
            var employeeServices = _context.EmployeeService
            .Include(es => es.Service)
            .Where(es => es.EmployeeId == employeeId)
            .ToList();
            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = _context.Employee.Where(e => e.EmployeeId == employeeId).ToList(),
                Services = employeeServices.Select(es => es.Service).ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
                Works = _context.EmployeeWork.Where(e => e.EmployeeId == employeeId).ToList(),
            };
            return View(VM);
        }
        

        public ActionResult About_us ()
        {
            return View();
        }
        public ActionResult Rights_reserved()
        {
            return View();
        }
        public ActionResult Contact_us()
        {
            return View();
        }
        public ActionResult Terms_of_service()
        {
            return View();
        }
        public ActionResult Privacy_policy()
        {
            return View();
        }
    }
}