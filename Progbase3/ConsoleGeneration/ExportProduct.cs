using System;
using System.Collections.Generic;
    public class ExportProduct
    {
        public void DoExp(ProductRepository repository)
        {
            Console.WriteLine("Ent");
            string s = "12";
            LibProduct expPro = repository.Export(s);
            Export export = new Export(expPro, "exp.xml");
            export.Serialize();
        }
    }
