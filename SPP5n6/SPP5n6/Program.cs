using System;



namespace SPP5n6
{
    public class x { }
    public class y { }
    class Program
    {
        static void Main(string[] args)
        {
            PseudoReflectionClass pseudo = new PseudoReflectionClass();
            pseudo.Load("SPP5n6.dll");
            Console.WriteLine("done, buffer now");

            //LogBuffer buffer = new LogBuffer("log.txt", 10, 10_000);
            //Console.WriteLine("Write some lines");
            //while (true)
            //    buffer.Add(Console.ReadLine());



        }
    }
}
