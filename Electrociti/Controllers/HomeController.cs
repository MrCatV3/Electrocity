using Electrociti.Models;
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

        public IActionResult Index()
        {
            var Users = _context.Employee.ToList();
            return View(Users);
        }

        public IActionResult Master()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var context = new ApplicationContext())
                {
                    var loginEmployee = context.Employee.SingleOrDefault(u => u.EmployeeName == employee.EmployeeName && u.EmployeeName == employee.EmployeeName);
                    if (loginEmployee != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверные логин или пароль.");
                    }
                }
            }

            return View(employee);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
        }

        private string HashPassword(string password)
        {
            string salt = "RandomSalt123";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password + salt));
        }
    }
}