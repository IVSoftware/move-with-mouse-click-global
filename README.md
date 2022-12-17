As I understand it, the desired behavior is to enable the "Click to Move" (one way or another) and then click anywhere on a multiscreen surface and have the form follow the mouse to the new position. One solution that seems to work in my brief testing is to pinvoke the [SetWindowsHookEx](http://pinvoke.net/default.aspx/user32/SetWindowsHookEx.html) to install a global low level hook for [WH_MOUSE_LL](https://learn.microsoft.com/en-us/windows/win32/winmsg/about-hooks#wh_mouse_ll) in order to intercept `WM_LBUTTONDOWN`.

**This answer has been modified in order to track updates to the question.*
***

**Low-level global mouse hook**

        public MainForm()
        {
            InitializeComponent();
            using (var process = Process.GetCurrentProcess())
            {
                using (var module = process.MainModule!)
                {
                    var mname = module.ModuleName!;
                    var handle = GetModuleHandle(mname);
                    _hook = SetWindowsHookEx(
                        HookType.WH_MOUSE_LL,
                        lpfn: callback,
                        GetModuleHandle(mname),
                        0);
                }
            }

            // Unhook when this `Form` disposes.
            Disposed += (sender, e) => UnhookWindowsHookEx(_hook);

            // A little hack to keep window on top while Click-to-Move is enabled.
            checkBoxEnableCTM.CheckedChanged += (sender, e) =>
            {
                TopMost = checkBoxEnableCTM.Checked;
            };

            // Will need to offset the title NC area for the move.
            var offset = RectangleToScreen(ClientRectangle);
            CLIENT_RECT_OFFSET = offset.Y - Location.Y;
        }
        readonly int CLIENT_RECT_OFFSET;
        IntPtr _hook;
        private IntPtr callback(int code, IntPtr wParam, IntPtr lParam)
        {
            var next = IntPtr.Zero;
            if (code >= 0)
            {
                switch ((int)wParam)
                {
                    case WM_LBUTTONDOWN:
                        if (checkBoxEnableCTM.Checked)
                        {
                            _ = onClickToMove(MousePosition);
                            // This is a very narrow condition and the window is topmost anyway.
                            // So probably swallow this mouse click and skip other hooks in the chain.
                            return (IntPtr)1;
                        }
                        break;
                }
            }
            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
    }

***
**Perform the move**

    private async Task onClickToMove(Point mousePosition)
    {
        // Exempt clicks that occur on the 'Enable Click to Move` button itself.
        if (!checkBoxEnableCTM.ClientRectangle.Contains(checkBoxEnableCTM.PointToClient(mousePosition)))
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

            // Allow the pending mouse messages to pump. 
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            Location = offsetToNow;
            checkBoxEnableCTM.Checked = false; // Turn off after each move.
        }
    }

In the code I used to test this answer, it seemed intuitive to center the button where the click takes place (this offset is easy to change if it doesn't suit you). Here's the result of the multiscreen test:


[![Form][1]][1]

[![multiscreen][2]][2]

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


  [1]: https://i.stack.imgur.com/tLFqX.png
  [2]: https://i.stack.imgur.com/8I2wy.jpg
