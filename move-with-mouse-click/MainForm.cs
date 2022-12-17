using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace move_with_mouse_click
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
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

            checkBoxEnableCTM.CheckedChanged += (sender, e) =>
            {
                // A little hack to keep window on top while Click-to-Move is enabled.
                TopMost = checkBoxEnableCTM.Checked;
                checkBoxEnableCTM.ForeColor =
                    checkBoxEnableCTM.Checked ? 
                    Color.Salmon : 
                    Color.MediumSeaGreen;
            };

            // Compensate move offset with/without the title NC area.
            var offset = RectangleToScreen(ClientRectangle);
            CLIENT_RECT_OFFSET = offset.Y - Location.Y;

            buttonClose.Click += (sender, e) => Application.Exit();
            initRichText();
            initGlyphFont();
        }

        private void initGlyphFont()
        {
            glyphs.AddFontFile(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts", "move.ttf")
            );
            checkBoxEnableCTM.Font = new Font(glyphs.Families[0], 14F);
            checkBoxEnableCTM.Text = "\uE805";
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
                WindowState = FormWindowState.Normal; // JIC window happens to be maximized.
                Location = offsetToNow;
            }
            checkBoxEnableCTM.Checked = false; // Turn off after each move.
        }

        private void initRichText()
        {
            richTextBox.Rtf = 
@"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Calibri;}}
{\colortbl ;\red0\green187\blue77;}
{\*\generator Riched20 10.0.22621}\viewkind4\uc1 
\pard\sa200\sl276\slmult1\cf1\i\f0\fs24\lang9 Borderless Form \par Multiscreen Support.\cf0\i0\fs22\par
}
 ";
        }

        PrivateFontCollection glyphs = new PrivateFontCollection();
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
    }
}