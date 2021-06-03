
using System;
using System.IO;
using static System.Console;
public class ProductGen
{
    private string csvFile;
    ProductRepository repository;
    public ProductGen(string csvFile, ProductRepository repo)
    {
        this.csvFile = csvFile;
        this.repository = repo;
    }

    public void StartGeneration(string values)
    {
        
        int ex;
        string[] sub = values.Split(' ');
        if(sub.Length != 2)
        {
            WriteLine("Ошибка! Некорректные значения");
        }
        else if(!int.TryParse(sub[0], out ex))
        {
            WriteLine("Ошибка! Введите только целочисельные значения");
        }
        else
        {
            int i = 0;
            StreamReader reader = new StreamReader(this.csvFile);
            string str = "";
            while (i < int.Parse(sub[0]))
            {
                str = reader.ReadLine();
                str = str.Replace('"', ' ');
                if (str == null)
                {
                    break;
                }
                
                string[] valEntity = str.Split(','); 

                Product product = new Product();
                product.name = valEntity[0];
                product.info = valEntity[1];
                product.price = CreatePrice(sub[1]);
                product.availability = CreateAvailability();
                repository.Insert(product);

                i++;
            }
            reader.Close();

        }
    }

    private int CreatePrice(string limits)
    {
        string[] val = limits.Split('-');
        Random rand = new Random();

        int valPrice = rand.Next(int.Parse(val[0]), int.Parse(val[1]));
        return valPrice;
    }

    private bool CreateAvailability()
    {
        Random rand = new Random();
        int value = rand.Next(0, 13);
        if(value % 2 == 0)
        {
            return true;
        }
        else 
            return false;
    }

}
