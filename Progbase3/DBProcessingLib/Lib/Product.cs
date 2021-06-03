  
using System.Xml.Serialization;
public class Product
{
    public long id;
    public string name;
    public string info;
    public int price;
    public bool availability;
    public override string ToString()
    {
        return $"{name} — UA {price}";
    }
}
