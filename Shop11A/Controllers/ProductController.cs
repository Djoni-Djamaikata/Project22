using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Shop11A.Models;

namespace Shop11A.Controllers
{
    public class ProductController : Controller
    {


        // GET: /Product/Search?query=...
        public async Task<IActionResult> Search(string query)
        {

            var products = new List<Product>(); // Инициализиране на списък за съхраняване на продуктите.

            string search = "";
            search = Request.Query["search"];

            // Коригирана връзка с MySQL база данни
            using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=djoni;Uid=root;Pwd=;");
            await connection.OpenAsync();

            // SQL заявка за извличане на всички продукти
            using var command = new MySqlCommand("SELECT * FROM `produkti` WHERE `name` LIKE '%yamaha%';", connection);
            using var reader = await command.ExecuteReaderAsync();

            // Четене на данни от резултата на заявката
            while (await reader.ReadAsync())
            {
                var product = new Product
                {
                    Id = reader.GetInt32(2),           // Съответства на първата колона (Id)
                    Name = reader.GetString(3),        // Съответства на втората колона (Name)
                    Description = reader.GetString(4), // Съответства на третата колона (Description)
                    Price = reader.GetDecimal(5),      // Съответства на четвъртата колона (Price)
                    ImageUrl = reader.GetString(6),    // Съответства на петата колона (ImageUrl)
                    Category = new Category()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    },
                };

                products.Add(product); // Добавяне на продукта в списъка
            }



            return View(products);
        }
    }
}
