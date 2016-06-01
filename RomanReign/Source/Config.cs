using Microsoft.Xna.Framework;
using System.IO;
using System.Xml.Serialization;

namespace RomanReign
{
    public enum Difficulty { Normal, Hard }

    public struct Resolution
    {
        public int Width;
        public int Height;
        public bool Fullscreen;
    }

    public struct Volume
    {
        int m_music;
        int m_sfx;

        public int Music
        {
            get { return m_music; }
            set { m_music = MathHelper.Clamp(value, 0, 100); }
        }

        public int Sfx
        {
            get { return m_sfx; }
            set { m_sfx = MathHelper.Clamp(value, 0, 100); }
        }

        [XmlIgnore]
        public float MusicNormal
        {
            get { return Music / 100f; }
            set { Music = (int)(value * 100f); }
        }

        [XmlIgnore]
        public float SfxNormal
        {
            get { return Sfx / 100f; }
            set { Sfx = (int)(value * 100f); }
        }
    }

    public struct Internal
    {
        public int WaveLimit;
    }

    public class Config
    {
        public Difficulty Difficulty = Difficulty.Normal;
        public Resolution Resolution = new Resolution { Width = 1280, Height = 720, Fullscreen = true };
        public Volume Volume = new Volume { Music = 50, Sfx = 50 };

        public Internal Internal = new Internal { WaveLimit = 15 };

        #region Static fields and methods

        public static Config Data = new Config();

        public static void ReadConfig(string path)
        {
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
