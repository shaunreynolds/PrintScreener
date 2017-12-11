using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PrintScreen
{
    class Settings
    {
        public static readonly String FILE_LOCATION = System.Environment.SpecialFolder.MyDocuments.ToString() + "\\Settings.xml";
        public static bool LegacyPopups = false;
        public static bool EnablePopups = false;
        public static String WorkingDir = "";

        public static void LoadFromXML(String fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName);
                while (sr.Peek() > 0)
                {
                    String line = sr.ReadLine();
                    String key = line.Split('=')[0];
                    String value = line.Split('=')[1];

                    if (key.Equals("LegacyPopups"))
                    {
                        LegacyPopups = bool.Parse(value);
                    }

                    if (key.Equals("EnablePopups"))
                    {
                        EnablePopups = bool.Parse(value);
                    }

                    if (key.Equals("WorkingDir"))
                    {
                        WorkingDir = value;
                    }
                }

            }catch(Exception e)
            {
                Console.Out.WriteLine(e);
            }
        }

        public static void SaveToXML(String fileName)
        {
            Directory.CreateDirectory(System.Environment.SpecialFolder.MyDocuments.ToString());
            TextWriter tw = new StreamWriter(fileName);
            tw.WriteLine("LegacyPopups={0}", LegacyPopups);
            tw.WriteLine("EnablePopups=" + EnablePopups);
            tw.WriteLine("WorkingDir=" + WorkingDir);
            tw.Flush();
            tw.Close();
        }
    }
}
