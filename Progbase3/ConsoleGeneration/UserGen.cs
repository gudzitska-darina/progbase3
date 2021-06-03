using System;
using System.IO;
using static System.Console;
using System.Collections.Generic;
public class UserGen
{
    UserRepository userRepository;
    public UserGen(UserRepository usrepo)
    {
        this.userRepository = usrepo;
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
            int i = 1;
            while(i < int.Parse(sub[0]))
            {
                User user = new User();
                user.login = GenerateLogin();
                user.password = GeneratePassword();
                if(bool.Parse(sub[1]))
                {
                    bool st = GenerateStatus();
                    if(st)
                    {
                        user.status ="moderator";
                    }
                    else{
                        user.status = "client";
                    }
                }
                else
                {
                    user.status = "client";
                }
                userRepository.Insert(user);
                i++;
            }
          
        }
    }

    private string GenerateLogin()
    {
        List<string> names = new List<string>
        {
            "Mary",
            "Bob",
            "Alex",
            "Mett",
            "Max",
            "Ann",
            "Apple",
        };

        Random random = new Random();
        int index = random.Next(names.Count);
        string name = names[index];
        string sufix = random.Next(10, 399).ToString();
        string login = name + sufix;

        return login;
    }
    private string GeneratePassword()
    {
        List<string> passwords = new List<string>
        {
            "c8DzJeTC9t",
            "rvbrYASYyh", 
            "rx9joRV7hQ", 
            "WK98oyCuMF",
            "26NrWqz1K2",
            "5HPhLYeMcX", 
            "td8yMhhKLn",
            "4CFhP6BanW",
            "Fj89bxOAj6"
        };
        Random random = new Random();
        int index = random.Next(passwords.Count);
        string password = passwords[index];

        return password;
    }
    
    private bool GenerateStatus()
    {
        Random rand = new Random();
        int value = rand.Next(0, 15);
        if(value % 3 == 0)
        {
            return true;
        }
        else 
            return false;
    }

}
