using Electrociti.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace Electrociti.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var Users = _context.Employees.ToList();
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
            // Возвращайте представление для формы входа
            return View();
        }

        [HttpPost]
        public ActionResult Login(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Предполагаем, что у вас есть экземпляр контекста базы данных
                using (var context = new ApplicationContext())
                {
                    // Поиск пользователя по логину и паролю
                    var loginEmployee = context.Employees.SingleOrDefault(u => u.EmployeeName == employee.EmployeeName && u.EmployeeName == employee.EmployeeName);

                    if (loginEmployee != null)
                    {
                        // Пользователь найден, выполняем вход
                        // В реальном приложении это может включать установку куки аутентификации, создание сессии и т.д.

                        // Перенаправление на главную страницу после успешной авторизации
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Пользователь не найден, добавляем ошибку в ModelState
                        ModelState.AddModelError("", "Неверные логин или пароль.");
                    }
                }
            }

            // Если ModelState не прошла валидацию, возвращаем представление с ошибками
            return View(employee);
        }

        public ActionResult Register()
        {
            // Возвращайте представление для формы регистрации
            return View();
        }

        [HttpPost]
        public ActionResult Register(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Предполагается, что у вас уже есть экземпляр контекста базы данных
                using (var context = new ApplicationContext())
                {
                    // Проверка, что пользователь с таким именем пользователя не существует
                    if (context.Employees.Any(u => u.EmployeeName == employee.EmployeeName))
                    {
                        ModelState.AddModelError("Username", "Пользователь с таким именем уже существует.");
                        return View(employee);
                    }

                    // Хеширование пароля (лучше использовать библиотеку хеширования)
                    // В данном примере, просто добавим "salt" к паролю
                    employee.EmployeePassword = HashPassword(employee.EmployeePassword);

                    // Добавление пользователя в контекст базы данных
                    context.Employees.Add(employee);
                    context.SaveChanges();

                    // Перенаправление на главную страницу после успешной регистрации
                    return RedirectToAction("Index", "Home");
                }
            }

            // Если ModelState не прошла валидацию, возвращаем представление с ошибками
            return View(employee);
        }

        private string HashPassword(string password)
        {
            // В реальном приложении лучше использовать библиотеку хеширования
            // Этот пример просто добавляет "salt" к паролю
            string salt = "RandomSalt123";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password + salt));
        }
    }
}