
//Задача 7.
//	Создать на языке C# статический метод класса Parallel.WaitAll, который:	
//	- принимает в параметрах массив делегатов;
//-выполняет все указанные делегаты параллельно с помощью пула потоков;
//-дожидается окончания выполнения всех делегатов.	

//	"Реализовать простейший пример использования метода Parallel.WaitAll.

//Задача 8.
//Создать на языке C# пользовательский атрибут с именем ExportClass, применимый только к классам, и реализовать консольную программу, которая:
//- принимает в параметре командной строки путь к сборке .NET (EXE- или DLL-файлу);
//-загружает указанную сборку в память;
//-выводит на экран полные имена всех public -типов данных этой сборки, помеченные атрибутом ExportClass."	


using System;
using System.Threading;

namespace Spp7N8
{
    delegate void Work();
    delegate void NotWork();

    class Parallel
    {
        public static bool? WaitAll<T>(T array)
        {
            var actions = (Work)(object)array; // Override GetInvocationList if u want (T)(object) not (Work)(object) ex. => interface -> waitall<t>(t array) where t:interface
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
    class Program
    {
        static void Main(string[] args)
        {
            Work pseudoArray = new Work(Hello) + DisplayThread + new Work(Done); // Delegate have INBUILT ADRDESS ARRAY not [], but u can use it if u want, to net nikakoy raznica
            bool? fun = Parallel.WaitAll(pseudoArray) ?? false;
            if ((bool)!fun ^ (fun??=false?true:false))
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
