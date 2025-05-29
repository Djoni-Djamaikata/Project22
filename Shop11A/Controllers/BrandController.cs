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
			int idCat = 0;
			idCat = int.Parse(Request.Query["cat"]);
			
						
			// Коригирана връзка с MySQL база данни
			string sql = "";

			if (idCat <= 0)
			{
				sql = "SELECT * FROM produkti  WHERE brand = " + id + ";";
			}
			else
			{
				sql = "SELECT * FROM produkti WHERE brand = " + id + " AND category = " + idCat + ";";
			}

			using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=djoni;Uid=root;Pwd=;");
            await connection.OpenAsync();

            // SQL заявка за извличане на всички продукти
            using var command = new MySqlCommand(sql, connection);
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
						Name = await GetBrandNameById(reader.GetInt32(5)),
					},
                };

                produts.Add(product); // Добавяне на продукта в списъка
            }

            return produts; // Връщане на списъка с продукти
        }

		public async Task<string> GetBrandNameById(int Id)
		{
			using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=djoni;Uid=root;Pwd=;");
			await connection.OpenAsync();

			using var command = new MySqlCommand("SELECT brand_name FROM brand WHERE id =" + Id + ";", connection);
			using var reader = await command.ExecuteReaderAsync();

			await reader.ReadAsync();
			
			var brandName = reader.GetString(0);
			
			return brandName; // Предаване на продуктите към изгледа

		}

		public async Task<IActionResult> Detals()
        {
			int id = 0;
			id = int.Parse(Request.Query["brand"]);


			var products = await GetProducts(); // Извличане на продукти от базата данни

			if (products == null)
			{
				var produts = new List<Product>(); // Инициализиране на списък за съхраняване на продуктите.

				var product = new Product
				{
					Id = 0,           // Съответства на първата колона (Id)
					Name = "",        // Съответства на втората колона (Name)
					Description = "", // Съответства на третата колона (Description)
					Price = 0,      // Съответства на четвъртата колона (Price)
					ImageUrl = "",    // Съответства на петата колона (ImageUrl)
					Brand = new Brand()
					{
						Id = id,
						Name = await GetBrandNameById(id),
					},
				};
				produts.Add(product); // Добавяне на продукта в списъка
			}

            return View(products); // Предаване на продуктите към изгледа

        }
    }
}
