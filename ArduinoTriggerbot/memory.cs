namespace ArduinoTriggerbot
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    class memory
    {
        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        private static extern IntPtr op(int DesiredAccess, bool InheritHandle, int ProcessID);

        [DllImport("ntdll.dll", EntryPoint = "NtReadVirtualMemory")]
        public static extern bool rm(int Process, UInt64 Address, byte[] Buffer, int BufferSize, ref int ByteCount);

        public static Process InitializeCS2()
        {
            Process cs2Process;
            if (Process.GetProcessesByName("cs2").Length > 0)
                cs2Process = Process.GetProcessesByName("cs2")[0];
            else
                return null;

            foreach (ProcessModule module in cs2Process.Modules)
            {
                string moduleName = module.ModuleName;

                if (moduleName == "client.dll")
                {
                    client = (ulong)module.BaseAddress;
                }
            }

            processHandle = (int)op(0x10, false, cs2Process.Id);

            if (processHandle > 0 && client > 0)
                return cs2Process;
            else
                return null;
        }

        public static int processHandle = 0;
        public static UInt64 client;
        public static int bytesread = 0;

        public static T Read<T>(UInt64 Address) where T : struct
        {
            int ByteSize = Marshal.SizeOf(typeof(T));
            byte[] Buffer = new byte[ByteSize];
            rm(processHandle, Address, Buffer, Buffer.Length, ref bytesread);
            return ByteArrayToStructure<T>(Buffer);
        }

        private static T ByteArrayToStructure<T>(byte[] Bytes) where T : struct
        {
            GCHandle var_Handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
            try { return (T)Marshal.PtrToStructure(var_Handle.AddrOfPinnedObject(), typeof(T)); }
            finally { var_Handle.Free(); }
        }
    }
}
