using System.IO;
using System.Xml.Serialization;

namespace RomanReign
{
    public enum Difficulty { Normal, Hard }

    public struct Resolution
    {
        public int Width;
        public int Height;
    }

    public class Config
    {
        public Difficulty Difficulty = Difficulty.Normal;
        public Resolution Resolution = new Resolution { Width = 1280, Height = 720 };

        #region Static fields and methods

        public static Config Data = new Config();

        public static void ReadConfig(string path)
        {
            Data = new Config();

            if (File.Exists(path))
            {
                XmlSerializer xml = new XmlSerializer(Data.GetType());
                using (StreamReader reader = new StreamReader(path))
                {
                    try { Data = (Config)xml.Deserialize(reader.BaseStream); }
                    catch { Data = new Config(); }
                }
            }
        }

        public static void WriteConfig(string path)
        {
            XmlSerializer xml = new XmlSerializer(Data.GetType());

            using (StreamWriter writer = new StreamWriter(path))
                xml.Serialize(writer.BaseStream, Data);
        }

        #endregion
    }
}
