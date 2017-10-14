namespace Clickstreamer.Win32.Keyboard
{
    using System;
    using System.Runtime.InteropServices;
    using Clickstreamer.Events;

    public class Keyboard : IDataObserver<KeyboardEventArgs>
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        
        private readonly LowLevelKeyboardProc hookProc;

        private IntPtr hookPointer = IntPtr.Zero;
        
        public Keyboard()
        {
            this.hookProc = this.HookCallback;
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public event EventHandler<KeyboardEventArgs> Event;

        public void Subscribe()
        {
            this.hookPointer = Interop.SetHook(
                () => Keyboard.SetWindowsHookEx(
                    idHook: Keyboard.WH_KEYBOARD_LL,
                    lpfn: this.hookProc, 
                    hMod: Interop.GetModuleHandle(Interop.User32),
                    dwThreadId: 0));
        }
        
        public void Unsubscribe()
        {
            Interop.UnhookWindowsHookEx(this.hookPointer);
        }

        [DllImport(Interop.User32Module, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                this.Event(
                    this, 
                    new KeyboardEventArgs(
                        hookStruct.vkCode,
                        hookStruct.scanCode,
                        hookStruct.flags,
                        hookStruct.time));
            }

            return Interop.CallNextHookEx(this.hookPointer, nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
    }
}