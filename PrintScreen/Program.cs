using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintScreen
{
    static class Program
    {
        public static int counter=0;
        public static String dirName = "";
        static Form1 form1;
        static ScreenCapturer sc;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.Out.WriteLine(Settings.FILE_LOCATION);
            sc = new ScreenCapturer();
            KeyboardHook kh = new KeyboardHook(true);
            kh.KeyDown += Kh_KeyDown;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(form1=new Form1());

            Console.Out.WriteLine("Form closed");
            kh.Dispose();
            Console.In.Read();
        }

        private static void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            if (key.Equals(Keys.PrintScreen))
            {
                if (form1.folderSelected)
                {
                    //Console.WriteLine("Screen shot: " + (counter++) + ".png");
                    //saveImage((counter++) + ".png");
                    String fileName = dirName + "\\" + counter + ".png";
                    Bitmap image = sc.Capture(enmScreenCaptureMode.Window);
                    image = writeOnImage2(image, counter + ".png");
                    image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                    form1.logOutput("Saved screenshot as: " + fileName);

                    //AutoClosingMessageBox.Show("Saved screenshot as: "+fileName,"Alert",1000);

                    form1.popupMessage("Saved screenshot as: " + fileName,1500,ToolTipIcon.Info);

                    image.Dispose();
                    counter++;

                }
                else
                {
                    form1.logOutput("Output folder not set yet.");
                    form1.popupMessage("Output folder not set yet.", 1500,ToolTipIcon.Error);
                }
            }
        }

        private static void ShowToast()
        {
            TileBindingContentAdaptive bindingContent = new TileBindingContentAdaptive()
            {
                PeekImage = new TilePeekImage()
                {
                    Source = "Assets/PeekImage.jpg"
                },

                Children =
    {
        new AdaptiveText()
        {
            Text = "Notifications library",
            HintStyle = AdaptiveTextStyle.Body
        },

        new AdaptiveText()
        {
            Text = "Generate notifications easily!",
            HintWrap = true,
            HintStyle = AdaptiveTextStyle.CaptionSubtle
        }
    }
            };
            
        }

        private static void saveImage(String fileName)
        {
            if (Clipboard.ContainsImage())
            {
                Console.Out.WriteLine("Grabbed image");
                Image image = (Image)Clipboard.GetDataObject().GetData(DataFormats.Bitmap);

                try
                {
                    image = writeOnImage2(image, fileName);
                }catch (Exception e)
                {
                    Console.Out.WriteLine(e.ToString());
                }
                Console.Out.WriteLine("Processed image");

                String desktop = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                image.Save(dirName +"\\"+fileName, System.Drawing.Imaging.ImageFormat.Png);
                Console.Out.WriteLine("File saved as: "+dirName + "\\" + fileName);
                form1.logOutput("Saved screenshot as: "+dirName + "\\" + fileName);



                image.Dispose();
            }
            
        }

        private static Image writeOnImage(Image image,String fileName)
        {
            int x = image.Width / 2;
            Graphics g = Graphics.FromImage(image);
            Font shadowFont = new Font("Arial", 22, FontStyle.Regular, GraphicsUnit.Pixel);
            Font textFont = new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel);
            Point shadow = new Point(x, 20);
            Point text = new Point(x, 22);
            //LinearGradientBrush blackBrush = new LinearGradientBrush(Color.Black);
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            //g.DrawString(fileName, shadowFont, blackBrush, shadow, sf);
            //g.DrawString(fileName, textFont, whiteBrush, text, sf);
            g.Dispose();

            return image;
        }

        private static Image writeOnImage2(Image image, String fileName)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            int x = image.Width / 2;
            int fontSize = 16;
            int offset = 20;
            Pen pen = new Pen(Pens.Black.Brush);
            pen.Width = 1;

            Image canvas = new Bitmap(image.Width,image.Height+offset);
            Graphics g = Graphics.FromImage(canvas);
            g.Clear(Color.White);
            g.DrawImage(image, 0, offset);
            GraphicsPath p = new GraphicsPath();
            p.AddString(
                fileName,             // text to draw
                FontFamily.GenericMonospace,  // or any other font family
                (int)FontStyle.Bold,      // font style (bold, italic, etc.)
                g.DpiY * fontSize / 72,       // em size
                new Point(x, 0+(offset/2)),              // location where to draw text
                sf);          // set options here (e.g. center alignment)
            g.DrawPath(pen, p);
            g.Dispose();

            return canvas;
        }
        private static Bitmap writeOnImage2(Bitmap image, String fileName)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            int x = image.Width / 2;
            int fontSize = 16;
            int offset = 20;
            Pen pen = new Pen(Pens.Black.Brush);
            pen.Width = 1;

            Bitmap canvas = new Bitmap(image.Width, image.Height + offset);
            Graphics g = Graphics.FromImage(canvas);
            g.Clear(Color.White);
            g.DrawImage(image, 0, offset);
            GraphicsPath p = new GraphicsPath();
            p.AddString(
                fileName,             // text to draw
                FontFamily.GenericMonospace,  // or any other font family
                (int)FontStyle.Bold,      // font style (bold, italic, etc.)
                g.DpiY * fontSize / 72,       // em size
                new Point(x, 0 + (offset / 2)),              // location where to draw text
                sf);          // set options here (e.g. center alignment)
            g.DrawPath(pen, p);
            g.Dispose();

            return canvas;
        }
    }
}
