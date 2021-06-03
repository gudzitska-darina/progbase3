using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("root")]
public class LibProduct
{
    [XmlElement("product")]
    public List<Product> products;
}
