using System.Xml.Serialization;
using System;
using System.IO;

public class Import
{
    private string filepath;
    private LibProduct cors = null;
    public LibProduct Deserialize()
    {
       
            XmlSerializer serializer = new XmlSerializer(typeof(LibProduct));

            StreamReader reader = new StreamReader(filepath);
            cors = (LibProduct)serializer.Deserialize(reader);
            reader.Close();
            return cors;       
    }
    public void SetFile(string filepath)
    {
        this.filepath = filepath;
    }
}
