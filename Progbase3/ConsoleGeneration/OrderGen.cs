using System;
using System.IO;
using static System.Console;
using System.Collections.Generic;
public class OrderGen
{
    private Random rand;
    private string csvFile;
    OrderRepository repository;
    UserRepository userRepository;
    ProductRepository productRepository;
    CheckRepository checkRepository;
    public OrderGen(string csvFile, OrderRepository repo, UserRepository usrepo, ProductRepository pr, CheckRepository ch)
    {
        this.csvFile = csvFile;
        this.rand = new Random();
        this.repository = repo;
        this.userRepository = usrepo;
        this.productRepository = pr;
        this.checkRepository = ch;
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
            string[] str = File.ReadAllLines(this.csvFile);
            string s = "";
            while (i < int.Parse(sub[0]))
            {
                s = str[rand.Next(str.Length)];
                
                string[] valEntity = s.Split(','); 

                Order order = new Order();
                order.createdAt = CreateDate(sub[1]);
                order.userId = rand.Next(1, (int)userRepository.Count());

                long insid = repository.Insert(order, order.createdAt);

                Check bill = new Check();
                bill.orderId = insid;
                //bill.productId = pro.id;
                checkRepository.Insert(bill);

                i++;
            }
        }
    }
    private string GenName()
    {
        List<string> names = productRepository.GetAllName();
        Random random = new Random();
        int index = random.Next(names.Count);
        string name = names[index];
        return name;
    }
    private DateTime CreateDate(string limits)
    {
        string[] val = limits.Split('-');
        DateTime start = DateTime.Parse(val[0]);
        DateTime end = DateTime.Parse(val[1]);

        int randomYear = this.rand.Next(start.Year, end.Year) ;
        int randomMonth = this.rand.Next(1, 12) ;
        int randomDay = this.rand.Next(1, DateTime.DaysInMonth(randomYear, randomMonth)) ;
        if (randomYear == start.Year)
        {
            randomMonth = rand.Next(start.Month, 12) ;

            if (randomMonth == start.Month)
                randomDay = this.rand.Next(start.Day, DateTime.DaysInMonth(randomYear, randomMonth)) ;
        }
        if (randomYear == end.Year)
        {
            randomMonth = this.rand.Next(1, end.Month) ;

            if (randomMonth == end.Month)
                randomDay = this.rand.Next(1, end.Day) ;
        }

        DateTime randomDate = new DateTime(randomYear, randomMonth, randomDay);
        return randomDate;
    }

    public bool CreateAvailability()
    {
        int value = this.rand.Next(0, 13);
        if(value % 2 == 0)
        {
            return true;
        }
        else 
            return false;
    }
}
