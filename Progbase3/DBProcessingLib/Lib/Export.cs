using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
public class Export
{
    private LibProduct prods;
    private string filepath;
    public Export(LibProduct prods, string filepath)
    {
        this.prods = prods;
        this.filepath = filepath;
    }
    public void Serialize()
    {
        XmlSerializer ser = new XmlSerializer(typeof(LibProduct));
 
        System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
        settings.Indent = true;
        settings.NewLineHandling = System.Xml.NewLineHandling.Entitize;
        System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(filepath, settings);
        
        ser.Serialize(writer, this.prods);
        writer.Close();
    }
}
