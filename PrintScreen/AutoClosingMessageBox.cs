using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintScreen
{
    public class AutoClosingMessageBox
    {
        ToastForm tf;
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            using (_timeoutTimer)
            {
                //   MessageBox.Show(text, caption);
                tf = new ToastForm(timeout);
                tf.FormBorderStyle = FormBorderStyle.None;
                tf.Text = caption;
                tf.label1.Text = text;
                //tf.label1.ic
                tf.StartPosition = FormStartPosition.Manual;
                tf.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - tf.Width,
                                       Screen.PrimaryScreen.WorkingArea.Height - tf.Height);
                tf.WindowState = FormWindowState.Minimized;
                //tf.Opacity = .5;
                tf.Show();
                tf.WindowState = FormWindowState.Normal;

                Lerp l = new Lerp(tf.Opacity, 0, 1, 1000);
                l.SetForm(tf);


            }
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
            //tf.Hide();
            //tf.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
