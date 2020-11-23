using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SPP5n6
{
    //    Задача 5.	
    //Реализовать консольную программу на языке C#, которая:	
    //- принимает в параметре командной строки путь к сборке.NET(EXE- или DLL-файлу);	
    //- загружает указанную сборку в память;	
    //- выводит на экран полные имена всех public-типов данных этой сборки, упорядоченные по пространству имен(namespace) и по имени.

    class PseudoReflectionClass
    {
        Assembly asm;
        //#TODO,BTW Works
        public void Load(string Path)
        {
            asm = Assembly.LoadFrom(Path);

            Console.WriteLine(asm.FullName);
            // получаем все типы из сборки
            var types = asm.GetTypes();
           //var linqTypes = asm.GetTypes().Where(x=>x.IsPublic);

            foreach (Type t in types)
            {
                var b = t.GetMethods().Where(x=>x.IsPublic);
                foreach (var item in b)
                {
                    Console.WriteLine(item.Name);
                }
            }
            Console.ReadLine();
        }
    }
}
