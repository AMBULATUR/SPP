//Задача 6.
//Создать класс на языке C#, который:	
//Создать класс LogBuffer, который:	
//-представляет собой журнал строковых сообщений;
//-предоставляет метод public void Add(string item);

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SPP5n6
{
    class LogBuffer
    {
        private Queue<string> buffer;
        public delegate void AddHandler(string message);
        public event AddHandler Notify;
        private string logPath;
        private int bufferMaxLength;
        public int BufferMaxLength { get { return bufferMaxLength; } private set { bufferMaxLength = value; } }
        public string LogPath { get { return logPath; } private set { logPath = value; } }

        private System.Timers.Timer timer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logPath">Путь к лог-файлу</param>
        /// <param name="bufferMaxLength">Максимальное кол-во элементов в буффере после чего произойдёт запись </param>
        /// <param name="timerValue">Вызов записи в буффер каждый N ms</param>
        public LogBuffer(string logPath, int bufferMaxLength, double timerValue)
        {
            buffer = new Queue<string>();
            Notify += LogBuffer_Notify;
            this.LogPath = logPath;
            this.BufferMaxLength = bufferMaxLength;
            timer = new System.Timers.Timer(timerValue);
            timer.Elapsed += onTimerEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void onTimerEvent(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine("ByTimer");
            _ = AppendLogAsync();
        }
        private void LogBuffer_Notify(string message)
        {
            buffer.Enqueue(message);
            if (buffer.Count == BufferMaxLength)
            {
                System.Console.WriteLine("ByLogMax");
                _ = AppendLogAsync();
                ResetTimer();
            }
        }
        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }

        public void Add(string item) // Добавление в буфер
        {
            Notify?.Invoke(item);
        }

        private async Task AppendLogAsync()
        {
            int counter = 0;
            System.Console.WriteLine($"Append occured");
            var tasks = new List<Task>();
            using (StreamWriter writer = new StreamWriter(LogPath, true))
            {
                foreach (var item in buffer.ToList())
                {
                    buffer.Dequeue();
                    counter++;
                    tasks.Add(writer.WriteLineAsync(item));
                }
                await Task.WhenAll();
                writer.Flush();
            }
            System.Console.WriteLine($"Buffer done {counter} lines");
        }
    }
}
