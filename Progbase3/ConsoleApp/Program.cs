using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Terminal.Gui;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filedb = "C:/Users/darin/Documents/0_0/university/ОП/project/progbase3/data/progdb.db";
            if(File.Exists(filedb))
            {   
                SqliteConnection connection = new SqliteConnection($"Data Source={filedb}");
                UserRepository user = new UserRepository(connection);
                ProductRepository product = new ProductRepository(connection);
                OrderRepository order = new OrderRepository(connection, user);
                CheckRepository checks = new CheckRepository(connection);
                UserRepository users = new UserRepository(connection);
                Authentication authentication = new Authentication(users);

                Application.Init();
                Toplevel top = Application.Top;

                AunthenticationWindow winAu = new AunthenticationWindow();
                winAu.SetUserRepositoryAndAunth(authentication, users);
                winAu.SetOrderRepository(order);
                winAu.SetProductRepository(product);
                winAu.SetCheckRepository(checks);
                top.Add(winAu);

                Application.Run();
            }
            else
            {
                Application.Init();
                Toplevel top = Application.Top;

                ErrorDBWindow errorDB = new ErrorDBWindow();
                Application.Run();
            }
            
        }
    }
}
