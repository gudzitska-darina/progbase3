using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;
public class UserRepository
{
    private SqliteConnection connection;
    public UserRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }
    public long Insert(User user) 
    {   
        this.connection.Open(); 

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO users (login, password, status)
        VALUES ($login, $password, $status);
        SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$login", user.login);
        command.Parameters.AddWithValue("$password", user.password);
        command.Parameters.AddWithValue("$status", user.status);
        
        long newId = (long)command.ExecuteScalar();
        
        this.connection.Close(); 
        return newId;
    }
    public long Count()
    {   
        this.connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM users";       
        return (long)command.ExecuteScalar();
    }

    public User GetById(long id)
    {
        this.connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM users WHERE user_id = $id";
        command.Parameters.AddWithValue("$id", id);
    
        SqliteDataReader reader = command.ExecuteReader();
        User us = new User();
        if (reader.Read())
        {   
            us.id = long.Parse(reader.GetString(0));
            us.login = reader.GetString(1);
            us.password = reader.GetString(2);
            us.status = reader.GetString(3);
        }

        reader.Close();
        return us;  
    }
    public User GetByLogin(string log)
    {
        this.connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM users WHERE login = $name";
        command.Parameters.AddWithValue("$name", log);
    
        SqliteDataReader reader = command.ExecuteReader();
        User us = new User();
        if (reader.Read())
        {   
            us.id = long.Parse(reader.GetString(0));
            us.login = reader.GetString(1);
            us.password = reader.GetString(2);
            us.status = reader.GetString(3);
        }

        reader.Close();
        return us;  
    }

    public List<string> GetAllLogin()
    {
        this.connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM users";
    
        SqliteDataReader reader = command.ExecuteReader();
        List<string> users = new List<string>();
        while(reader.Read())
        {   
            string name = reader.GetString(1);
           
            users.Add(name);
        }
        
        reader.Close();
        this.connection.Close();
        return users;  
    }
}
