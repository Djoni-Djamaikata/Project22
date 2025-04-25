using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shop11A.Models;
using System.Linq;
using Shop11A.Data;
using MySql.Data.MySqlClient;

namespace Shop11A.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        

        // Метод за получаване на продукти от базата данни
        public async Task<List<Product>> GetProducts()
        {
            var products = new List<Product>(); // Инициализиране на списък за съхраняване на продуктите.

            // Коригирана връзка с MySQL база данни
            using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=djoni;Uid=root;Pwd=;");
            await connection.OpenAsync();

            // SQL заявка за извличане на всички продукти
            using var command = new MySqlCommand("SELECT * FROM brand AS b JOIN produkti AS p ON b.id = p.brand;", connection);
            using var reader = await command.ExecuteReaderAsync();

            // Четене на данни от резултата на заявката
            while (await reader.ReadAsync())
            {
                var product = new Product
                {
                    Id = reader.GetInt32(3),           // Съответства на първата колона (Id)
                    Name = reader.GetString(4),        // Съответства на втората колона (Name)
                    Description = reader.GetString(5), // Съответства на третата колона (Description)
                    Price = reader.GetDecimal(6),      // Съответства на четвъртата колона (Price)
                    ImageUrl = reader.GetString(7),    // Съответства на петата колона (ImageUrl)
                    Brand = new Brand()
                    {
                        Id=reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Logo = reader.GetString(2),
                    },
                };

                products.Add(product); // Добавяне на продукта в списъка
            }

            return products; // Връщане на списъка с продукти
        }

        // Действие за извеждане на началната страница
        public async Task<IActionResult> Index()
        {
            var products = await GetProducts(); // Извличане на продукти от базата данни
            return View(products); // Предаване на продуктите към изгледа
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
