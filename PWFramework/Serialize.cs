using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PWFramework
{
    public static class Serialize
    {
        private static XmlSerializer formatter = new XmlSerializer(typeof(Offsets));

        public static void Deserializable()
        {
            using (FileStream fs = new FileStream("offsets.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    Offsets.setInstance((Offsets)formatter.Deserialize(fs));
                }
                catch
                {
                    MessageBox.Show(String.Format("В папке {0}\nне найден файл настроек", Environment.CurrentDirectory));
                }
            }
        }

        public static void Serializable()
        {
            using (FileStream fs = new FileStream(Environment.CurrentDirectory + "\\offsets.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Offsets.getInstance());
                fs.Close();
            }
        }
    }
}
