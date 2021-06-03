using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;

public class ProductRepository
{
    private SqliteConnection connection;
    public ProductRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }
    public List<string> GetAllName()
    {
        this.connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products";
    
        SqliteDataReader reader = command.ExecuteReader();
        List<string> products = new List<string>();
        while(reader.Read())
        {   
            string name = reader.GetString(1);
           
            products.Add(name);
        }
        
        reader.Close();
        this.connection.Close();
        return products;  
    }
    public Product GetByName(string name)
    {
        this.connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products WHERE name = $name";
        command.Parameters.AddWithValue("$name", name);
    
        SqliteDataReader reader = command.ExecuteReader();
        Product pro = new Product();
        if (reader.Read())
        {   
            pro.id = long.Parse(reader.GetString(0));
            pro.name = reader.GetString(1);
            pro.info = reader.GetString(2);
            pro.price = int.Parse(reader.GetString(3));
            pro.availability = int.Parse(reader.GetString(4)) == 1;
        }

        reader.Close();
        return pro;  
    }
    public int DeleteById(long id) 
    {   
        this.connection.Open();          
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM products WHERE product_id = $id";
        command.Parameters.AddWithValue("$id", id);
        
        int nChanged = command.ExecuteNonQuery();

        this.connection.Close();
        return nChanged;
    }
    public long Insert(Product pro) 
    {   
        this.connection.Open(); 

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO products (name, info, price, availability)
        VALUES ($name, $info, $price, $availability);
        SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$name", pro.name);
        command.Parameters.AddWithValue("$info", pro.info);
        command.Parameters.AddWithValue("$price", pro.price);
        command.Parameters.AddWithValue("$availability", pro.availability);
        
        long newId = (long)command.ExecuteScalar();
        
        this.connection.Close(); 
        return newId;
    }
    public int UpdateById(long id, Product pro)
    {
        this.connection.Open();          
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"UPDATE products 
        SET name = $name, info = $info, price = $price, availability = $availability
        WHERE product_id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", pro.name);
        command.Parameters.AddWithValue("$info", pro.info);
        command.Parameters.AddWithValue("$price", pro.price);
        command.Parameters.AddWithValue("$availability", pro.availability);
        int nChanged = command.ExecuteNonQuery();
        this.connection.Close();
        return nChanged;
    }
    public long GetCountProducts()
    {   
        this.connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM products";       
        return (long)command.ExecuteScalar();
    }
    public int GetTotalPages(int pageSize)
    {
        return (int)Math.Ceiling(this.GetCountProducts() / (double)pageSize);
    }
    public List<Product> GetPage(int pageNumber, int pageSize) 
    { 
        this.connection.Open();  
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
        command.Parameters.AddWithValue("$pageSize", pageSize);
        command.Parameters.AddWithValue("$pageNumber", pageNumber);
        SqliteDataReader reader = command.ExecuteReader();
        List<Product> products = new List<Product>();
        while(reader.Read())
        {
            long id = long.Parse(reader.GetString(0));
            string name = reader.GetString(1);
            string info = reader.GetString(2);
            int price = int.Parse(reader.GetString(3));
            bool availability = int.Parse(reader.GetString(4)) == 1;

            Product pro = new Product();
            pro.id = id;
            pro.name = name;
            pro.info = info;
            pro.price = price;
            pro.availability = availability;
            products.Add(pro);
        }
        reader.Close();
        this.connection.Close();  
        return products;
    }
    
    public int GetSearchPages(int pageSize, string searchVal)
    {
        if(string.IsNullOrEmpty(searchVal))
        {
            return (int)GetTotalPages(pageSize);
        }
        return (int)Math.Ceiling(GetCountSearchProds(searchVal).Count / (double)pageSize);
    }

    public List<Product> GetCountSearchProds(string searchVal)
    {
        this.connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products 
        WHERE name LIKE '%' || $substring || '%'";
        command.Parameters.AddWithValue("$substring", searchVal);

        SqliteDataReader reader = command.ExecuteReader();
        List<Product> searchProducts = new List<Product>();
        while(reader.Read())
        {   
            Product pro = new Product();
            
            pro.id = long.Parse(reader.GetString(0));
            pro.name = reader.GetString(1);
            pro.info = reader.GetString(2);
            pro.price = int.Parse(reader.GetString(3));
            pro.availability = int.Parse(reader.GetString(4)) == 1;
            searchProducts.Add(pro);
        }
        
        reader.Close();
        this.connection.Close();
        return searchProducts;
    }
    public List<Product> GetSearchPage(string substring, int pageNumber, int pageSize) 
    { 
        if(string.IsNullOrEmpty(substring))
        {
            return GetPage(pageNumber, pageSize);
        }
        else
        {
            this.connection.Open();  
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products
            WHERE name LIKE '%' || $substring || '%' 
            LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
            command.Parameters.AddWithValue("$substring", substring);
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumber", pageNumber);
            SqliteDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();
            while(reader.Read())
            {
                long id = long.Parse(reader.GetString(0));
                string name = reader.GetString(1);
                string info = reader.GetString(2);
                int price = int.Parse(reader.GetString(3));
                bool availability = int.Parse(reader.GetString(4)) == 1;

                Product pro = new Product();
                pro.id = id;
                pro.name = name;
                pro.info = info;
                pro.price = price;
                pro.availability = availability;
                products.Add(pro);
            }
            reader.Close();
            this.connection.Close();  
            return products;
        }        
    }
    
    public LibProduct Export(string substring)
    {
        this.connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products WHERE name LIKE '%' || $substring || '%'";
        command.Parameters.AddWithValue("$substring", substring);
    
        SqliteDataReader reader = command.ExecuteReader();
        LibProduct productlib = new LibProduct();
        List<Product> products = new List<Product>();
        while(reader.Read())
        {   
            long id = long.Parse(reader.GetString(0));
            string name = reader.GetString(1);
            string info = reader.GetString(2);
            int price = int.Parse(reader.GetString(3));
            bool availability = int.Parse(reader.GetString(4)) == 1;

            Product pro = new Product();
            pro.id = id;
            pro.name = name;
            pro.info = info;
            pro.price = price;
            pro.availability = availability;
            products.Add(pro);
        }
        
        reader.Close();
        productlib.products = products;
        this.connection.Close();
        return productlib;          
    }

    public List<Product> GetOrderProducts(List<long> prodIds)
    {
        List<Product> prodList = new List<Product>();
        connection.Open();
        foreach (long id in prodIds)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products WHERE product_id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Product pro = new Product();
                pro.id = long.Parse(reader.GetString(0));
                pro.name = reader.GetString(1);
                pro.info = reader.GetString(2);
                pro.price = int.Parse(reader.GetString(3));
                pro.availability = int.Parse(reader.GetString(4)) == 1;
                prodList.Add(pro);
            }
        }
        connection.Close();

        return prodList;
    }

    public List<Product> GetAll()
    {
        List<Product> prodList = new List<Product>();
        connection.Open();
        
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products";
        SqliteDataReader reader = command.ExecuteReader();
        while(reader.Read())
        {
            Product pro = new Product();
            pro.id = long.Parse(reader.GetString(0));
            pro.name = reader.GetString(1);
            pro.info = reader.GetString(2);
            pro.price = int.Parse(reader.GetString(3));
            pro.availability = int.Parse(reader.GetString(4)) == 1;
            prodList.Add(pro);
        }
        connection.Close();

        return prodList;
    }
}
