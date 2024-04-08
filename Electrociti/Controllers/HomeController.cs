using Electrociti.Data;
using Electrociti.Models;
using Electrociti.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            EmployeeServiceEmployeeServices VM = new EmployeeServiceEmployeeServices
            {
                Employee = _context.Employee2.ToList(),
                Services = _context.Service.ToList(),
                EmployeeServices = _context.EmployeeService.ToList(),
            };
            return View(VM);
        }
        public IActionResult Master()
        {
            return View();
        }
        public IActionResult Folowing()
        {
            return View();
        }
        public IActionResult EmployeeProfile()
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
            if (HttpContext.Session.GetInt32("EmployeeId") != null)
            {
                return View("EmployeeProfile");
            }
            else { return View("Login"); }
        }
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var employeeService = new EmployeeServiceLogin(_context);
            var employee = employeeService.GetEmployee(login, password);
            if (employee != null)
            {
                HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неверная попытка входа.");
                return View();
            }

            if (HttpContext.Session.GetInt32("EmployeeId") != 1 && employee != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (HttpContext.Session.GetInt32("EmployeeId") == 1)
            {
                return View("Admin");
            }

            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
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