using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintScreen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public bool folderSelected = false;

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            Program.dirName = folderBrowserDialog1.SelectedPath;
            Console.Out.WriteLine(Program.dirName);
            textBox1.Text = Program.dirName;
            popupMessage("Directory set to: " + Program.dirName, 1500, ToolTipIcon.Info);
            Settings.WorkingDir = Program.dirName;
            folderSelected = true;
            setCounter();
        }

        public void popupMessage(String text, int duration,ToolTipIcon icon)
        {
            if (this.enablePopupsToolStripMenuItem.Checked)
            {

                if (this.nativePopupsToolStripMenuItem.Checked)
                {
                    notifyIcon1.BalloonTipText = text;
                    notifyIcon1.BalloonTipIcon = icon;
                    notifyIcon1.BalloonTipTitle = "Info";
                    notifyIcon1.ShowBalloonTip(duration);
                }
                else
                {

                    // Legacy version
                    //AutoClosingMessageBox.Show(text, "Alert", duration);
                    legacyPopup(text, "Alert", duration);
                }
            }
        }

        private void legacyPopup(String text, String caption,int duration)
        {
            
            ToastForm tf = new ToastForm(duration);
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

        public void popupMessage(String text)
        {
            popupMessage(text, 1000, ToolTipIcon.Info);
        }

        public void setCounter()
        {
            string[] files = Directory.GetFiles(Program.dirName);
            int counter = 0;
            foreach (String s in files)
            {
                
                String fileName = Path.GetFileName(s);
                //logOutput(fileName);
                try
                {
                    String num = fileName.Split('.')[0];
                    int tmp = int.Parse(num);
                    if (tmp > counter)
                    {
                        counter = tmp;
                    }

                    //logOutput(num + "num int: " + tmp);
                }
                catch(Exception e)
                {
                    //logOutput(e.ToString());
                }
            }
            Program.counter = counter+1;
            logOutput("Setting image counter to: " + Program.counter);
            //popupMessage("Setting image counter")
        }
        public void logOutput(String output)
        {
           // outputBox.Text += output + System.Environment.NewLine;
            outputBox.AppendText(output + System.Environment.NewLine);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1_MouseDoubleClick(null, null);
        }

        private void changeFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Settings.LoadFromXML(Settings.FILE_LOCATION);
            nativePopupsToolStripMenuItem.Checked = !Settings.LegacyPopups;
            enablePopupsToolStripMenuItem.Checked = Settings.EnablePopups;
            Program.dirName = Settings.WorkingDir;
            if (Program.dirName != null)
            {
                folderSelected = false;
            }
        }
    }
    }
