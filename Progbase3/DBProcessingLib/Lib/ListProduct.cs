using System;
using System.Xml.Serialization;

[XmlRoot("root")]
public class ListProduct
    {
        [XmlElement("product")]
        private Product[] _items;
        private int _size;
        public ListProduct()
        {
            _items = new Product[16];  
            _size = 0;
        }
        public void Add(Product ord) 
        {
            int i = 0;
            if (_size == 0)
            {
                _items[0] = ord;
                _size++;
                return;
            }
            while (true)
            {
                if (i == _items.Length - 1)
                {
                    Array.Resize(ref _items, _items.Length * 2);
                }
                if (i == _size)
                {
                    _items[i] = ord;
                    _size++;
                    break;
                }
                i++;
            }
        }
    }