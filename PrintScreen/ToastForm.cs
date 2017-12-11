using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintScreen
{
    public partial class ToastForm : Form
    {
        int timeoutTime = 0;
        public ToastForm(int timeoutTime)
        {
            InitializeComponent();
            this.timeoutTime = timeoutTime;
        }

        private void ToastForm_Load(object sender, EventArgs e)
        {
            Timer t = new Timer();
            t.Interval = timeoutTime;
            t.Tick += new EventHandler(timerTick);
            t.Start();
        }

        private void timerTick(object o, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
