namespace Clickstreamer.Win32
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public static class Interop
    {
        public const string User32 = "user32",
            Kernel32 = "kernel32",
            User32Module = "user32.dll",
            Kernel32Module = "kernel32.dll";

        public static IntPtr SetHook(Func<IntPtr> hookCreator)
        {
            IntPtr hook;
            try
            {
                hook = hookCreator();
            }
            catch
            {
                throw;
            }

            if (hook == IntPtr.Zero)
            {
                throw new Win32Exception(message: "Invalid hook handle");
            }

            return hook;
        }

        [DllImport(Interop.User32Module, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(Interop.User32Module, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(Interop.Kernel32Module, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}