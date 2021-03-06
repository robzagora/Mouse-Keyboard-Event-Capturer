﻿namespace Clickstreamer.Win32.Mouse
{
    using System;
    using System.Runtime.InteropServices;
    using Clickstreamer.Events;
    using Clickstreamer.Extensions;

    public class Mouse : IDataObserver<MouseEventArgs>
    {
        private const int WH_MOUSE_LL = 14, // Installs a hook procedure that monitors low-level mouse input events. For more information, see the LowLevelMouseProc hook procedure.
            WH_MOUSE = 7; // Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure.

        private readonly LowLevelMouseProc hookProc;

        private IntPtr hookPointer = IntPtr.Zero;

        public Mouse()
        {
            this.hookProc = this.HookCallback;
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        public event EventHandler<MouseEventArgs> Event;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        public void Subscribe()
        {
            this.hookPointer = Interop.SetHook(
                () => Mouse.SetWindowsHookEx(
                    idHook: Mouse.WH_MOUSE_LL,
                    lpfn: this.hookProc,
                    hMod: Interop.GetModuleHandle(Interop.User32),
                    dwThreadId: 0));
        }
        
        public void Unsubscribe()
        {
            Interop.UnhookWindowsHookEx(this.hookPointer);
        }

        [DllImport(Interop.User32Module, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MouseMessages message = (MouseMessages)Marshal.ReadInt32(wParam);
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                this.Event(
                    this,
                    new MouseEventArgs(
                        hookStruct.pt.X,
                        hookStruct.pt.Y,
                        hookStruct.flags,
                        hookStruct.mouseData,
                        hookStruct.time,
                        message.GetName()));
            }

            return Interop.CallNextHookEx(this.hookPointer, nCode, wParam, lParam);
        }
        
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644970(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
    }
}