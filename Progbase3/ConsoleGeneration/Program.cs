using System;
using static System.Console;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = "C:/Users/darin/Documents/0_0/university/ОП/project/progbase3/data/progdb.db";
            SqliteConnection connection = new SqliteConnection($"Data Source={file}");
            ProductRepository repoProd = new ProductRepository(connection);
            UserRepository repoUs = new UserRepository(connection);
            OrderRepository repoOrd = new OrderRepository(connection, repoUs);
            CheckRepository billRepo = new CheckRepository(connection);
            string csvProd = "C:/Users/darin/Documents/0_0/university/ОП/project/progbase3/data/productGen.csv";
            ProductGen prGen = new ProductGen(csvProd, repoProd);
            OrderGen orGen = new OrderGen(csvProd, repoOrd, repoUs, repoProd, billRepo);
            UserGen usGen = new UserGen(repoUs);
            
            ImageGeneration img = new ImageGeneration("graf.png");
            WriteLine("Таблица связей генерируется автоматически!\nРекомендуется генерировать заказы после генерации пользователей");
            while(true)
            {
                WriteLine("Введите тип сутности которую хотите сгенерировать");
                string entity = "img";
                if(entity == "products")
                {
                    WriteLine("Введите в таком порядке значения: количество(max 55), диапазон цен(записать через '-')");
                    string values = ReadLine();
                    prGen.StartGeneration(values);
                }
                else if(entity == "order")
                {
                    WriteLine("Введите в таком порядке значения: количество, диапазон даты создания(записать через '-')");
                    string values = ReadLine();
                    orGen.StartGeneration(values);
                    break;
                }
                else if(entity == "users")
                {
                    WriteLine("Сгенерированые пароли не хешируются!");
                    WriteLine("Введите в таком порядке значения: количество, генерировать модераторов(true/false)");
                    string values = ReadLine();
                    usGen.StartGeneration(values);
                }
                else if(entity == "img")
                {
                    List<Product> prod = repoProd.GetAll();
                    img.Generate(prod);
                    break;
                }

            }
        }
    }
}
