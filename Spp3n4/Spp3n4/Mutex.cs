using System.Threading;

namespace Spp3n4
{
    class Mutex
    {
        #nullable enable
        private object? mutedThread;
        public void Lock()
        {
            object thread = Thread.CurrentThread;
            while (Interlocked.CompareExchange(ref mutedThread, thread, null) != null)
            {
                continue;
            }
        }
        public void Unlock()
        {
            object thread = Thread.CurrentThread;
            Interlocked.CompareExchange(ref mutedThread, null, thread);
            // Interlocked.Exchange() as Binary Semaphon
        }

    }
}
