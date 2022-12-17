using System.Diagnostics;
using System.Runtime.InteropServices;

namespace move_with_mouse_click
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            // Application.AddMessageFilter(this);
            var offset = RectangleToScreen(ClientRectangle);
            CLIENT_RECT_OFFSET = offset.Y - Location.Y;
            initRichText();
            using (var process = Process.GetCurrentProcess())
            {
                using (var module = process.MainModule)
                {
                    var handle = GetModuleHandle(module.ModuleName);
                    _hook = SetWindowsHookEx(
                        HookType.WH_MOUSE_LL,
                        lpfn: callback,
                        GetModuleHandle(module.ModuleName),
                        0);
                }
            }
            Disposed += (sender, e) => UnhookWindowsHookEx(_hook);

            // A little hack to keep window on time while CTM is enabled.
            checkBoxEnableCTM.CheckedChanged += (sender, e) =>
            {
                TopMost = checkBoxEnableCTM.Checked;
            };
        }
        readonly int CLIENT_RECT_OFFSET;
        IntPtr _hook;
        private IntPtr callback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                switch ((int)wParam)
                {
                    case WM_LBUTTONDOWN:
                        if (checkBoxEnableCTM.Checked)
                        {
                            onClickToMove(MousePosition);
                        }
                        break;
                    case WM_LBUTTONUP:
                        // N O O P
                        break;
                    default:
                        break;
                }
            }
            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
        private void onClickToMove(Point mousePosition)
        {
            if (checkBoxEnableCTM.ClientRectangle.Contains(checkBoxEnableCTM.PointToClient(mousePosition)))
            {
                // We really have to exclude this control, don't we?
            }
            else
            {
                // Try this. Offset the new `mousePosition` so that the cursor lands
                // in the middle of the button when the move is over. This feels
                // like a semi-intuitive motion perhaps. This means we have to
                // subtract the relative position of the button from the new loc.
                var clientNew = PointToClient(mousePosition);

                var centerButton =
                    new Point(
                        checkBoxEnableCTM.Location.X + checkBoxEnableCTM.Width / 2,
                        checkBoxEnableCTM.Location.Y + checkBoxEnableCTM.Height / 2);

                var offsetToNow = new Point(
                    mousePosition.X - centerButton.X,
                    mousePosition.Y - centerButton.Y - CLIENT_RECT_OFFSET);

                BeginInvoke(() =>
                {
                    Location = offsetToNow;                    
                    richTextBox.Select(0, 0); // Cosmetic fix selection artifact
                });
            }
        }

        private void initRichText()
        {
            richTextBox.Rtf = 
@"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Calibri;}}
{\colortbl ;\red0\green187\blue77;}
{\*\generator Riched20 10.0.22621}\viewkind4\uc1 
\pard\sa200\sl276\slmult1\cf1\i\f0\fs24\lang9 Now with Multiscreen Support!!.\cf0\i0\fs22\par
}
 ";
        }

        #region P I N V O K E
        public enum HookType : int { WH_MOUSE = 7, WH_MOUSE_LL = 14 }
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        [Flags]
        internal enum MOUSEEVENTF : uint
        {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }

        delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,
           IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll"), Obsolete("https://stackoverflow.com/q/22744531/5438626")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEHOOKSTRUCT
        {
            public Point pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
        }
        // https://www.youtube.com/watch?v=Lt3H5swUl8Q

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        internal struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }
        public class Win32Consts
        {
            // For use with the INPUT struct, see SendInput for an example
            public const int INPUT_MOUSE = 0;
            public const int INPUT_KEYBOARD = 1;
            public const int INPUT_HARDWARE = 2;
        }

        internal struct MOUSEINPUT
        {
            public Int32 X;
            public Int32 Y;
            public UInt32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }
        #endregion P I N V O K E
    }
}