using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;

public class CheckRepository
{
    private SqliteConnection connection;
    public CheckRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }
    public int DeleteByOrderId(long id) 
    {   
        this.connection.Open();          
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM bills WHERE order_id = $id";
        command.Parameters.AddWithValue("$id", id);
        
        int nChanged = command.ExecuteNonQuery();

        this.connection.Close();
        return nChanged;
    }
    public int DeleteByProductId(long id) 
    {   
        this.connection.Open();          
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM bills WHERE product_id = $id";
        command.Parameters.AddWithValue("$id", id);
        
        int nChanged = command.ExecuteNonQuery();

        this.connection.Close();
        return nChanged;
    }
    public int UpdateByOrderId(long id, Order ord)
    {
        this.connection.Open();          
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"UPDATE bills 
        SET order_id = $orderId
        WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$order_id", ord.id);
        int nChanged = command.ExecuteNonQuery();
        this.connection.Close();
        return nChanged;
    }
    public long Insert(Check bill) 
    {   
        this.connection.Open(); 

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO bills (order_id, product_id)
        VALUES ($order_id, $product_id);
        SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$order_id", bill.orderId);
        command.Parameters.AddWithValue("$product_id", bill.productId);
        
        long newId = (long)command.ExecuteScalar();
        
        this.connection.Close(); 
        return newId;
    }

}
