using System;

public class ListOrder
    {
        private Order[] _items;
        private int _size;
        public ListOrder()
        {
            _items = new Order[16];  
            _size = 0;
        }
        public void Add(Order ord) 
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