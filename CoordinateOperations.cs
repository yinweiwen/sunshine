using System;
using System.Runtime.InteropServices;

namespace Sunshine
{
    public class CoordinateOperations
    {
        // 获取窗口位置
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        // 窗口坐标结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // 屏幕坐标转换
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        // 点坐标结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public static POINT ConvertToScreenCoordinates(IntPtr hWnd, int relativeX, int relativeY)
        {
            POINT point = new POINT { X = relativeX, Y = relativeY };
            ClientToScreen(hWnd, ref point);
            return point;
        }
    }
}
