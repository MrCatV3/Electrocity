using Electrociti.Data;
using Electrociti.Models;
using Electrociti.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using System.Text;

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

        public IActionResult Index(string searchString, int? minCost, int? maxCost, bool rate)
        {
            // Установка значений по умолчанию для minCost и maxCost, если они не указаны
            minCost ??= 1;
            maxCost ??= 999999;

            // Запрос на получение данных
            IQueryable<Employee> query = _context.Employee
                .Include(e => e.EmployeeServices) // Включаем связанные данные об услугах
                    .ThenInclude(es => es.Service) // Включаем связанные данные о самих услугах
                .AsQueryable();

            // Применение фильтров
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e =>
                    e.EmployeeAddress.Contains(searchString) ||
                    e.EmployeeDescription.Contains(searchString) ||
                    e.EmployeeServices.Any(es => es.Service.ServiceName.Contains(searchString)));
            }

            if (rate)
            {
                query = query.Where(e => int.Parse(e.EmployeeRate) >= 4);
            }

            // Получение списка отфильтрованных сотрудников
            List<Employee> searchResults = query.ToList();

            // Формирование модели представления
            EmployeeServiceEmployeeServices viewModel = new EmployeeServiceEmployeeServices
            {
                Employee = searchResults,
                Services = _context.Service.ToList(),
                EmployeeServices = _context.EmployeeService.ToList()
            };

            return View(viewModel);
        }

        //public IActionResult GetFilteredUsers(string searchString, string minCost, string maxCost, bool rate)
        //{
        //    var allUsers = "";

        //    var filteredUsers = allUsers.Where(user =>
        //        (string.IsNullOrEmpty(searchString) || user.Name.Contains(searchString)) &&
        //        (string.IsNullOrEmpty(minCost) || user.Cost >= decimal.Parse(minCost)) &&
        //        (string.IsNullOrEmpty(maxCost) || user.Cost <= decimal.Parse(maxCost)) &&
        //        (!rate || user.Rating >= 4)
        //    ).ToList();

        //    // Возвращаем частичное представление с отфильтрованными пользователями
        //    return PartialView("_EmployeeCards", filteredUsers);
        //}

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

        public IActionResult Admin()
        {
            return View();
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
                int? E = HttpContext.Session.GetInt32("EmployeeId");
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
                        return View("UpdateEmployeeProfile");
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("EmployeeProfile");
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

        private string HashPassword(string password)
        {
            string salt = "RandomSalt123";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password + salt));
        }
    }
}