using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sunshine
{
    public class Operator
    {
        private static string sunshineWindowTitle = "向日葵远程控制";

        // 查找窗口
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // 查找子窗口
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        // 设置窗口为前台窗口
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // 发送消息到指定窗口
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        const uint WM_SETTEXT = 0x000C; // 设置文本
        const uint BM_CLICK = 0x00F5; // 按钮点击

        // 设置鼠标位置
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        // 模拟鼠标点击
        [DllImport("user32.dll", SetLastError = true)]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;

        // 模拟键盘输入
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const int VK_RETURN = 0x0D; // Enter键

        // 激活窗口
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;
        private const int VK_CONTROL = 0x11; // Ctrl 键
        private const int VK_A = 0x41; // A 键
        private const int VK_DELETE = 0x2E; // Delete 键
        private const int KEYEVENTF_KEYUP = 0x0002; // 键抬起

        public static void SendKeysWithModifiers(byte modifier, byte key)
        {
            // 模拟按下修饰键 (Ctrl)
            keybd_event(modifier, 0, 0, UIntPtr.Zero);
            // 模拟按下目标键 (A)
            keybd_event(key, 0, 0, UIntPtr.Zero);
            // 模拟释放目标键 (A)
            keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            // 模拟释放修饰键 (Ctrl)
            keybd_event(modifier, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
        public static void ClearTextBox(int x, int y)
        {
            // 将鼠标移动到输入框位置并点击
            SetCursorPos(x, y);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);

            Thread.Sleep(100);

            // 发送 Ctrl+A 选择所有文本
            SendKeysWithModifiers(VK_CONTROL, VK_A);
            Thread.Sleep(100);

            // 发送 Delete 键删除所有文本
            keybd_event((byte)VK_DELETE, 0, 0, UIntPtr.Zero);
            keybd_event((byte)VK_DELETE, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static bool FocusWindow(string windowTitle)
        {
            try
            {
                IntPtr hWnd = FindWindow(null, windowTitle);
                if (hWnd == IntPtr.Zero)
                {
                    Console.WriteLine("未找到窗口");
                    return false;
                }
                ShowWindow(hWnd, SW_RESTORE);
                SetForegroundWindow(hWnd);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ClickButtonAt(int x, int y)
        {
            SetCursorPos(x, y); // 移动鼠标到指定坐标
            Thread.Sleep(100);  // 等待100毫秒
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, 0); // 按下鼠标左键
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);   // 松开鼠标左键
        }
        public static void EnterText(string text, int x, int y)
        {
            SetCursorPos(x, y);  // 移动鼠标到输入框
            Thread.Sleep(100);   // 等待100毫秒
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, 0); // 点击输入框
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);   // 松开鼠标

            Thread.Sleep(100);

            foreach (char c in text)
            {
                byte keyCode = GetVirtualKeyCode(c);
                keybd_event(keyCode, 0, 0, UIntPtr.Zero); // 输入字符
                Thread.Sleep(50); // 等待以模拟自然输入
                keybd_event(keyCode, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            }

            // 模拟按下回车键
            keybd_event((byte)VK_RETURN, 0, 0, UIntPtr.Zero);
        }

        private static byte GetVirtualKeyCode(char c)
        {
            // 大写字母
            if (c >= 'A' && c <= 'Z')
            {
                return (byte)(c - 'A' + 0x41);
            }
            // 小写字母
            if (c >= 'a' && c <= 'z')
            {
                return (byte)(c - 'a' + 0x41);
            }
            // 其他字符处理可添加
            return (byte)c;
        }

        public static void RunSunshine(string code, string pwds)
        {
            var flag = FocusWindow(sunshineWindowTitle);
            if (!flag)
            {
                throw new Exception("未找到窗口");
            }
            IntPtr hWnd = FindWindow(null, sunshineWindowTitle);

            // 假设输入框和按钮的相对坐标
            int buttonMenuX = 75;
            int buttonMenuY = 124;

            int inputBoxX = 300;
            int inputBoxY = 345;
            int inputBoxX2 = 500;
            int inputBoxY2 = 345;

            int buttonX = 680;
            int buttonY = 345;

            // 转换为桌面坐标
            var inputBoxScreenCoords = CoordinateOperations.ConvertToScreenCoordinates(hWnd, inputBoxX, inputBoxY);
            var inputBoxScreenCoords2 = CoordinateOperations.ConvertToScreenCoordinates(hWnd, inputBoxX2, inputBoxY2);
            var buttonMenuCoords = CoordinateOperations.ConvertToScreenCoordinates(hWnd, buttonMenuX, buttonMenuY);
            var buttonScreenCoords = CoordinateOperations.ConvertToScreenCoordinates(hWnd, buttonX, buttonY);

            //ClickButtonAt(buttonMenuCoords.X, buttonMenuCoords.Y);

            ClickButtonAt(inputBoxScreenCoords.X, inputBoxScreenCoords.Y);
            ClearTextBox(inputBoxScreenCoords.X, inputBoxScreenCoords.Y);
            EnterText(code, inputBoxScreenCoords.X, inputBoxScreenCoords.Y);

            Thread.Sleep(500);
            ClickButtonAt(inputBoxScreenCoords2.X, inputBoxScreenCoords2.Y);
            ClearTextBox(inputBoxScreenCoords2.X, inputBoxScreenCoords2.Y);
            EnterText(pwds, inputBoxScreenCoords2.X, inputBoxScreenCoords2.Y);

            ClickButtonAt(buttonScreenCoords.X, buttonScreenCoords.Y);
        }
    }
}
