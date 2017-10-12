namespace Clickstreamer.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class Mouse
    {
        // TODO: finish this model
        public class MouseEventArgs : EventArgs
        {
            private int x, y;

            public MouseEventArgs()
            {
                this.x = x;
                this.y = y;
            }

            public int X { get { return this.x; } }

            public int Y { get { return this.y; } }
        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        
        private static IntPtr hookPointer = IntPtr.Zero;

        private const int WH_MOUSE_LL = 14, // Installs a hook procedure that monitors low-level mouse input events. For more information, see the LowLevelMouseProc hook procedure.
            WH_MOUSE = 7; // Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure.

        public static void Subscribe()
        {
            Mouse.hookPointer = Interop.SetHook(
                () => Mouse.SetWindowsHookEx(
                    idHook: Mouse.WH_MOUSE_LL,
                    lpfn: Mouse.HookCallback,
                    hMod: Interop.GetModuleHandle(Interop.User32),
                    dwThreadId: 0));
        }

        public static void Unsubscribe()
        {
            Interop.UnhookWindowsHookEx(hookPointer);
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                // TODO: create meaningful event args with all the mouse data
            }

            return Interop.CallNextHookEx(hookPointer, nCode, wParam, lParam);
        }

        [DllImport(Interop.User32Module, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);
    }
}