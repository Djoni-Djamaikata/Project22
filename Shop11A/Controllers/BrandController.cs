using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Shop11A.Models;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Shop11A.Controllers
{
	public class BrandController : Controller
	{ // Метод за получаване на продукти от базата данни
		public async Task<List<Brand>> GetBrands()
		{
			var brands = new List<Brand>(); // Инициализиране на списък за съхраняване на продуктите.

			// Коригирана връзка с MySQL база данни
			using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=djoni;Uid=root;Pwd=;");
			await connection.OpenAsync();

			// SQL заявка за извличане на всички продукти
			using var command = new MySqlCommand("SELECT * FROM brand;", connection);
			using var reader = await command.ExecuteReaderAsync();

			// Четене на данни от резултата на заявката
			while (await reader.ReadAsync())
			{
				var brand = new Brand()
					{
						Id = reader.GetInt32(0),
						Name = reader.GetString(1),
						Logo = reader.GetString(2),
					};

			brands.Add(brand); // Добавяне на продукта в списъка
			}

			return brands; // Връщане на списъка с продукти
		}

		// Действие за извеждане на началната страница
		public async Task<IActionResult> Index()
		{
			var products = await GetBrands(); // Извличане на продукти от базата данни
			return View(products); // Предаване на продуктите към изгледа
		}


        public async Task<List<Product>> GetProducts()
        {
            var produts = new List<Product>(); // Инициализиране на списък за съхраняване на продуктите.
            int id = 0;
            id = int.Parse(Request.Query["brand"]);
            // Коригирана връзка с MySQL база данни
            using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=djoni;Uid=root;Pwd=;");
            await connection.OpenAsync();

            // SQL заявка за извличане на всички продукти
            using var command = new MySqlCommand("SELECT * FROM produkti  WHERE brand = " + id +";", connection);
            using var reader = await command.ExecuteReaderAsync();

            // Четене на данни от резултата на заявката
            while (await reader.ReadAsync())
            {
                var product = new Product
                {
                    Id = reader.GetInt32(0),           // Съответства на първата колона (Id)
                    Name = reader.GetString(1),        // Съответства на втората колона (Name)
                    Description = reader.GetString(2), // Съответства на третата колона (Description)
                    Price = reader.GetDecimal(3),      // Съответства на четвъртата колона (Price)
                    ImageUrl = reader.GetString(4),    // Съответства на петата колона (ImageUrl)
                    Brand = new Brand()
                    {
                        Id = reader.GetInt32(5),

                    },
                };

                produts.Add(product); // Добавяне на продукта в списъка
            }

            return produts; // Връщане на списъка с продукти
        }

        public async Task<IActionResult> Detals()
        {
            var products = await GetProducts(); // Извличане на продукти от базата данни
            return View(products); // Предаване на продуктите към изгледа

        }
    }
}
