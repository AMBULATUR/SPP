using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//Задача 9.

//Создать на языке C# обобщенный (generic-) класс DynamicList<T>, который:
//- реализует динамический массив с помощью обычного массива T[];
//-имеет свойство Count, показывающее количество элементов;
//-имеет свойство Items для доступа к элементам по индексу;
//-имеет методы Add, Remove, RemoveAt, Clear для соответственно добавления, удаления, удаления по индексу и удаления всех элементов;
//-реализует интерфейс IEnumerable<T>.
//Реализовать простейший пример использования класса DynamicList<T> на языке C#.

namespace SPP9
{
    class DynamicList<T> : IEnumerable<T>
    {
        private readonly int defaultCapacity = 4;
        private readonly int _count;
        private T[] _items;
        private int _capacity = 0;
        private int _current = 0;

        public DynamicList()
        {
            _items = new T[defaultCapacity];
        }

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                _capacity = value;
                T[] newItems = new T[_capacity];
                Array.Copy(_items, newItems, _items.Length);
                _items = newItems;
            }
        }
        public int Count
        {
            get { return _count; }
        }
        public T[] Items
        {
            get { return _items; }
        }
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? defaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > System.Int32.MaxValue) newCapacity = System.Int32.MaxValue;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }

        public void Add(T value)
        {
            EnsureCapacity(_current);
            Items[_current++] = value;
        }
        public void Remove(T value)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].Equals(value))
                {
                    Array.Copy(_items, i + 1, _items, i, _items.Length - (i + 1));
                    _items[_items.Length - 1] = default(T);
                }
            }
        }
        public void RemoveAt(int index)
        {
            Array.Copy(_items, index + 1, _items, index, _items.Length - (index + 1));
            _items[_items.Length - 1] = default(T);
        }
        public void Clear()
        {
            _items = new T[defaultCapacity];
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (GetEnumerator().MoveNext())
                yield return GetEnumerator().Current;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            DynamicList<int> integer = new DynamicList<int>();

            for (int i = 0; i < 15; i++)
            {
                integer.Add(i);
            }
            foreach (var item in integer)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
