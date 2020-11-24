


using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Spp7N8
{
    //Задача 7.
    //	Создать на языке C# статический метод класса Parallel.WaitAll, который:	
    //	- принимает в параметрах массив делегатов;
    //-выполняет все указанные делегаты параллельно с помощью пула потоков;
    //-дожидается окончания выполнения всех делегатов.	

    //	"Реализовать простейший пример использования метода Parallel.WaitAll.

    delegate void Work();
    class Parallel
    {
        public static bool? WaitAll<T>(T array)
        {
            var actions = (Work)(object)array;
            int count = actions.GetInvocationList().Length;
            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {
                foreach (Work item in actions.GetInvocationList())
                {
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        item?.Invoke();
                        if (Interlocked.Decrement(ref count) == 0)
                            resetEvent.Set();
                    });
                };
                resetEvent.WaitOne();
            }
            Console.WriteLine("Done all actions");
            return true;
        }
    }
    //Задача 8.
    //Создать на языке C# пользовательский атрибут с именем ExportClass, применимый только к классам, и реализовать консольную программу, которая:
    //- принимает в параметре командной строки путь к сборке .NET (EXE- или DLL-файлу);
    //-загружает указанную сборку в память;
    //-выводит на экран полные имена всех public -типов данных этой сборки, помеченные атрибутом ExportClass."	

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class ExportClassAttribute : Attribute
    {
        public ExportClassAttribute([CallerFilePath] string file = null, [CallerLineNumber] int line = 0)
        {
            Console.WriteLine($"Exported at {line} in {file}");
        }
    }

    [ExportClass]
    class PublicClass1
    {
        public string prop;
    }
    [ExportClass]
    class PublicClass2
    {

    }
    class PrivateClass3
    {
    }

    class ExportHelper
    {

        public static void DisplayEachClass(string path)
        {
            var assembly = Assembly.LoadFrom(path);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                foreach (var item in type.GetCustomAttributes(typeof(ExportClassAttribute), false))
                {
                    Console.WriteLine(type.FullName);
                    //DisplayMethods(type);
                     DisplayProperties(type);
                    DisplayMembers(type);
                    DisplayFields(type);
                }
            }
        }
        private static void DisplayMethods(Type type)
        {
            foreach (var method in type.GetMethods().Where(method => method.IsPublic))
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(method.Name);
                Console.ResetColor();
            }
        }
        private static void DisplayProperties(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine(property.Name);
                Console.ResetColor();
            }
        }
        private static void DisplayMembers(Type type)
        {
            foreach (var member in type.GetMembers())
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(member.Name);
                Console.ResetColor();
            }
        }
        private static void DisplayFields(Type type)
        {
            foreach (var field in type.GetFields())
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine(field.Name);
                Console.ResetColor();
            }
        }
       



    }

    class Program
    {

        static void Main(string[] args)
        {
            ExportHelper.DisplayEachClass(args[0]);

            Work pseudoArray = new Work(Hello) + DisplayThread + new Work(Done); // Delegate have INBUILT ADRDESS ARRAY not [], but u can use it if u want, to net nikakoy raznica
            bool? fun = Parallel.WaitAll(pseudoArray) ?? false;
            Console.WriteLine("Main method done after WaitAll");
        }
        private static void Hello()
        {
            Console.WriteLine($"Hello Tid {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
        }
        private static void DisplayThread()
        {
            Thread.Sleep(2000);
            Console.WriteLine($"from Tid {Thread.CurrentThread.ManagedThreadId}");
        }
        private static void Done()
        {
            Console.WriteLine($"done at Tid {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
