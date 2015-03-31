using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Filewatcher.BLL
{
    internal static class NativeMethods
    {
        internal static IntPtr OpenFolder(string dir)
        {
            var handle = CreateFile(dir, FileAccess.Read, FileShare.Read, IntPtr.Zero, FileMode.Open, 0x02000000 | 0x00000080, IntPtr.Zero);
            var win32Error = Marshal.GetLastWin32Error();
            if (win32Error != 0)
                throw new Win32Exception();

            return handle;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CloseHandle(
            IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode,
            IntPtr lpSecurityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);
    }
}
