using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
public class Authentication
{
    private UserRepository userRepository;
    private SHA256 sha256Hash = SHA256.Create();
    public Authentication(UserRepository repo)
    {
        this.userRepository = repo;
    }
    public long Registration(string login, string password)
    {
        List<string> userLogins = userRepository.GetAllLogin();
        if(userLogins.Contains(login))
        {
            return 0;
        }
        else
        {
            User newUser = new User();
            newUser.login = login;
            newUser.password = GetHash(sha256Hash, password);
            newUser.status = "client";
            long usId = userRepository.Insert(newUser);
            return usId;
        }
    }

    public static string GetHash(HashAlgorithm hashAlgorithm, string input)
    {
    
        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
    
        var sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
    
        return sBuilder.ToString();
    }
    
    private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
    {
        var hashOfInput = GetHash(hashAlgorithm, input);
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        return comparer.Compare(hashOfInput, hash) == 0;
    }
    public long Login(string login, string password)
    {
        SHA256 sha256Hash = SHA256.Create();

        List<string> userLogins = userRepository.GetAllLogin();
        User searchUs = new User();
        if(userLogins.Contains(login))
        {
            searchUs = userRepository.GetByLogin(login);
            if(searchUs.password == GetHash(sha256Hash, password))
            {
                return searchUs.id;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return 0;
        }
    }
}
