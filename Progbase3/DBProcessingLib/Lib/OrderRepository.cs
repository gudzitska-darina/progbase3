using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;

public class OrderRepository
{
    private SqliteConnection connection;
    private UserRepository userRepository;
    public OrderRepository(SqliteConnection connection, UserRepository users)
    {
        this.connection = connection;
        this.userRepository = users;
    }
    public Order GetById(long id)
    {
        this.connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM orders WHERE order_id = $id";
        command.Parameters.AddWithValue("$id", id);
    
        SqliteDataReader reader = command.ExecuteReader();
        Order ord = new Order();
        if (reader.Read())
        {   
            ord.id = long.Parse(reader.GetString(0));
            ord.createdAt = DateTime.Parse(reader.GetString(1));
            ord.isPayed = int.Parse(reader.GetString(2)) == 1;
            ord.userId = long.Parse(reader.GetString(3));
            ord.autor = userRepository.GetById(ord.userId);
        }

        reader.Close();
        return ord;  
    }
    public int DeleteById(long id) 
    {   
        this.connection.Open();          
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM orders WHERE order_id = $id";
        command.Parameters.AddWithValue("$id", id);
        
        int nChanged = command.ExecuteNonQuery();

        this.connection.Close();
        return nChanged;
    }
    public long Insert(Order ord, DateTime date) 
    {   
        this.connection.Open(); 

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO orders (created_at, user_id, is_payed)
        VALUES ($created_at, $user_id, $is_payed);
        SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$created_at", date);
        command.Parameters.AddWithValue("$user_id", ord.userId);
        command.Parameters.AddWithValue("$is_payed", ord.isPayed);
        
        long newId = (long)command.ExecuteScalar();
        
        this.connection.Close(); 
        return newId;
    }

    public List<Order> GetByUserId(long userId)
    {
        this.connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM orders WHERE user_id = $user_id";
        command.Parameters.AddWithValue("$user_id", userId);
    
        SqliteDataReader reader = command.ExecuteReader();
        List<Order> listOrd = new List<Order>(); 
        while(reader.Read())
        {   Order ord = new Order();
            ord.id = long.Parse(reader.GetString(0));;
            ord.createdAt = DateTime.Parse(reader.GetString(1));
            ord.isPayed = int.Parse(reader.GetString(2)) == 1;
            ord.userId = long.Parse(reader.GetString(3));
            ord.autor = userRepository.GetById(userId);
            
            listOrd.Add(ord);
        }

        reader.Close();
        return listOrd;  
    }

    public int UpdateById(long id, Order ord)
    {
        this.connection.Open();          
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"UPDATE orders 
        SET is_payed = $is_payed 
        WHERE order_id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$is_payed", ord.isPayed);
        int nChanged = command.ExecuteNonQuery();
        this.connection.Close();
        return nChanged;
    }
    public long GetCountOrders()
    {   
        this.connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM orders";       
        return (long)command.ExecuteScalar();
    }
    public int GetTotalPages(int pageSize)
    {
        return (int)Math.Ceiling(this.GetCountOrders() / (double)pageSize);
    }
        
    public List<Order> GetPage(int pageNumber, int pageSize) 
    { 
        this.connection.Open();  
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM orders LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
        command.Parameters.AddWithValue("$pageSize", pageSize);
        command.Parameters.AddWithValue("$pageNumber", pageNumber);
        SqliteDataReader reader = command.ExecuteReader();
        List<Order> orders = new List<Order>();
        while(reader.Read())
        {
            Order ord = new Order();
            ord.id = long.Parse(reader.GetString(0));
            ord.createdAt = DateTime.Parse(reader.GetString(1));
            ord.isPayed = int.Parse(reader.GetString(2)) == 1;
            ord.userId = this.userRepository.GetById(long.Parse(reader.GetString(3))).id;
            ord.autor = this.userRepository.GetById(ord.userId);

            orders.Add(ord);
        }
        reader.Close();
        this.connection.Close();  
        return orders;
    }
    
    public List<long> GetAllOrderPrdId(long id)
    {
        this.connection.Open();  
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM bills WHERE order_id = $id";
        command.Parameters.AddWithValue("$id", id);

        SqliteDataReader reader = command.ExecuteReader();
        List<long> productsID = new List<long>();
        while(reader.Read())
        {   
            long prodId = long.Parse(reader.GetString(1));
            productsID.Add(prodId);
        }
        reader.Close();
        this.connection.Close();  
        return productsID;
    }
}   
