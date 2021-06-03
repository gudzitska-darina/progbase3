using System;
using System.Collections.Generic;

public class Order
{
    public long id;
    public DateTime createdAt;
    public long userId;
    public bool isPayed;
    public User autor;
    public List<Product> products;

    public override string ToString()
    {
        return $"[{autor.login}] —  Pay: {isPayed.ToString()} ";
    }
}

