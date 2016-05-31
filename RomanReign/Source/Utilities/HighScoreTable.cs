using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RomanReign
{
    public class HighScoreEntry
    {
        public string Name = string.Empty;
        public int Score = 0;
    }

    public class HighScoreTable
    {
        public List<HighScoreEntry> OnePlayer = new List<HighScoreEntry>();
        public List<HighScoreEntry> TwoPlayer = new List<HighScoreEntry>();
        public List<HighScoreEntry> ThreePlayer = new List<HighScoreEntry>();
        public List<HighScoreEntry> FourPlayer = new List<HighScoreEntry>();

        #region Static fields and methods

        public static HighScoreTable Data = new HighScoreTable();

        public static void ReadHighScores(string path)
        {
            if (!File.Exists(path))
                return;

            XmlSerializer xml = new XmlSerializer(Data.GetType());
            using (StreamReader reader = new StreamReader(path))
            {
                try { Data = (HighScoreTable)xml.Deserialize(reader.BaseStream); }
                catch { Data = new HighScoreTable(); }
            }
        }

        public static void WriteHighScores(string path)
        {
            XmlSerializer xml = new XmlSerializer(Data.GetType());

            using (StreamWriter writer = new StreamWriter(path))
                xml.Serialize(writer.BaseStream, Data);
        }

        public static HighScoreEntry GetLowestScore(int players)
        {
            List<HighScoreEntry> scores = GetScores(players);

            return scores?.Count > 0 ? scores.Last() : new HighScoreEntry();
        }

        public static HighScoreEntry GetHighestScore(int players)
        {
            List<HighScoreEntry> scores = GetScores(players);

            return scores?.Count > 0 ? scores.First() : new HighScoreEntry();
        }

        public static void AddHighScore(int players, string name, int score)
        {
            switch (players)
            {
                case 1:
                    Data.OnePlayer.Add(new HighScoreEntry { Name = name, Score = score });
                    Data.OnePlayer = Data.OnePlayer.OrderByDescending(hs => hs.Score).ToList();
                    while (Data.OnePlayer.Count > 10)
                        Data.OnePlayer.RemoveAt(Data.OnePlayer.Count - 1);
                    break;
                case 2:
                    Data.TwoPlayer.Add(new HighScoreEntry { Name = name, Score = score });
                    Data.TwoPlayer = Data.TwoPlayer.OrderByDescending(hs => hs.Score).ToList();
                    while (Data.TwoPlayer.Count > 10)
                        Data.TwoPlayer.RemoveAt(Data.TwoPlayer.Count - 1);
                    break;
                case 3:
                    Data.ThreePlayer.Add(new HighScoreEntry { Name = name, Score = score });
                    Data.ThreePlayer = Data.ThreePlayer.OrderByDescending(hs => hs.Score).ToList();
                    while (Data.ThreePlayer.Count > 10)
                        Data.ThreePlayer.RemoveAt(Data.ThreePlayer.Count - 1);
                    break;
                case 4:
                    Data.FourPlayer.Add(new HighScoreEntry { Name = name, Score = score });
                    Data.FourPlayer = Data.FourPlayer.OrderByDescending(hs => hs.Score).ToList();
                    while (Data.FourPlayer.Count > 10)
                        Data.FourPlayer.RemoveAt(Data.FourPlayer.Count - 1);
                    break;
            }
        }

        public static List<HighScoreEntry> GetScores(int players)
        {
            switch (players)
            {
                case 1:
                    return Data.OnePlayer;

                case 2:
                    return Data.TwoPlayer;

                case 3:
                    return Data.ThreePlayer;

                case 4:
                    return Data.FourPlayer;

                default:
                    return null;
            }
        }

        #endregion
    }
}
