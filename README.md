As I understand it, the desired behavior is to enable the "Click to Move" (one way or another) and then click anywhere on a multiscreen surface and have the form follow the mouse to the new position. One solution that seems to work in my brief testing is to pinvoke the [SetWindowsHookEx](http://pinvoke.net/default.aspx/user32/SetWindowsHookEx.html) to install a global low level hook for [WH_MOUSE_LL](https://learn.microsoft.com/en-us/windows/win32/winmsg/about-hooks#wh_mouse_ll) in order to intercept `WM_LBUTTONDOWN`.
***

**Low-level global mouse hook**

        public MainForm()
        {
            InitializeComponent();
            // Application.AddMessageFilter(this);
            var offset = RectangleToScreen(ClientRectangle);
            CLIENT_RECT_OFFSET = offset.Y - Location.Y;
            initRichText();
            using (var process = Process.GetCurrentProcess())
            {
                using (var module = process.MainModule!)
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

            // A little hack to keep window on time while Click-to-Move is enabled.
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
\pard\sa200\sl276\slmult1\cf1\i\f0\fs24\lang9 Now with Multiscreen Support!!\cf0\i0\fs22\par
}
 ";
        }

In the code I used to test this answer, it seemed intuitive to center the button where the click takes place. This offset is easy to change if it doesn't suit you. A screenshot doesn't really capture the behavior very well, so I've put the example up on GitHub feel free to [Clone](https://github.com/IVSoftware/move-with-mouse-click.git) it.


[![screenshot][1]][1]

**WinApi**


    #region P I N V O K E
    public enum HookType : int { WH_MOUSE = 7, WH_MOUSE_LL = 14 }
    const int WM_LBUTTONDOWN = 0x0201;

    delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, int dwThreadId);

    [DllImport("user32.dll")]
    static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,
        IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
    #endregion P I N V O K E

  [1]: https://i.stack.imgur.com/r4b3S.png