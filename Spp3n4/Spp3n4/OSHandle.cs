using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Spp3n4
{

    class OSHandle : IDisposable
    {
        [DllImport("Kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);


        private bool disposed = false;
        private IntPtr _handle;
        public IntPtr Handle
        {
            get { return _handle; }
            set { _handle = value; }
        }
        public OSHandle()
        {
            this.Handle = IntPtr.Zero;
            int PROCESS_ALL_ACCESS = (0x1F0FFF);
            Process[] handle = Process.GetProcessesByName("EPS");
            Handle = (IntPtr)OpenProcess(PROCESS_ALL_ACCESS, true, handle[0].Id);
        }
        public OSHandle(IntPtr handle)
        {
            this.Handle = handle;
        }
        ~OSHandle() // If Dispose not executed by user
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!this.disposed && this.Handle != IntPtr.Zero)
            {
                if (disposing)
                {
                    //Some managed || unmanaged code
                    Console.WriteLine("dispose managed code");
                }
                // unmanaged code
                CloseHandle(Handle);
                Handle = IntPtr.Zero;
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }
    }
}
