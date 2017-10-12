namespace Clickstreamer.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class Keyboard
    {
        public class KeyboardEventArgs : EventArgs
        {
            private int code;

            public KeyboardEventArgs(int code)
            {
                this.code = code;
            }

            public int Code { get { return this.code; } }
        }

        public static event EventHandler<KeyboardEventArgs> KeyboardAction;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static IntPtr hookPointer = IntPtr.Zero;

        public static void Subscribe()
        {
            Keyboard.hookPointer = Interop.SetHook(
                () => Keyboard.SetWindowsHookEx(
                    idHook: Keyboard.WH_KEYBOARD_LL,
                    lpfn: Keyboard.HookCallback, 
                    hMod: Interop.GetModuleHandle(Interop.User32),
                    dwThreadId: 0));
        }

        public static void Unsubscribe()
        {
            Interop.UnhookWindowsHookEx(hookPointer);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                KeyboardAction(new object(), new KeyboardEventArgs(vkCode));
            }

            return Interop.CallNextHookEx(hookPointer, nCode, wParam, lParam);
        }

        [DllImport(Interop.User32Module, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
    }
}