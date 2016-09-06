using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RomanReign
{
    public class LeaderboardEntry
    {
        public string Name = string.Empty;
        public int Score = 0;
    }

    public class Leaderboard
    {
        public List<LeaderboardEntry> OnePlayer = new List<LeaderboardEntry>();
        public List<LeaderboardEntry> TwoPlayer = new List<LeaderboardEntry>();
        public List<LeaderboardEntry> ThreePlayer = new List<LeaderboardEntry>();
        public List<LeaderboardEntry> FourPlayer = new List<LeaderboardEntry>();
    }

    public class LeaderboardManager
    {
        public Leaderboard Data = new Leaderboard();

        public LeaderboardManager(string path)
        {
            Read(path);
        }

        public void Read(string path)
        {
            if (!File.Exists(path))
                return;

            XmlSerializer xml = new XmlSerializer(Data.GetType());
            using (StreamReader reader = new StreamReader(path))
            {
                try { Data = (Leaderboard)xml.Deserialize(reader.BaseStream); }
                catch { Data = new Leaderboard(); }
            }
        }

        public void Write(string path)
        {
            XmlSerializer xml = new XmlSerializer(Data.GetType());

            using (StreamWriter writer = new StreamWriter(path))
                xml.Serialize(writer.BaseStream, Data);
        }

        public LeaderboardEntry GetLowestScore(int players)
        {
            List<LeaderboardEntry> scores = GetScores(players);

            return scores?.Count > 0 ? scores.Last() : new LeaderboardEntry();
        }

        public LeaderboardEntry GetHighestScore(int players)
        {
            List<LeaderboardEntry> scores = GetScores(players);

            return scores?.Count > 0 ? scores.First() : new LeaderboardEntry();
        }

        public void AddHighScore(int players, string name, int score)
        {
            switch (players)
            {
                case 1:
                    Data.OnePlayer.Add(new LeaderboardEntry { Name = name, Score = score });
                    Data.OnePlayer = Data.OnePlayer.OrderByDescending(hs => hs.Score).ToList();
                    while (Data.OnePlayer.Count > 10)
                        Data.OnePlayer.RemoveAt(Data.OnePlayer.Count - 1);
                    break;
                case 2:
                    Data.TwoPlayer.Add(new LeaderboardEntry { Name = name, Score = score });
                    Data.TwoPlayer = Data.TwoPlayer.OrderByDescending(hs => hs.Score).ToList();
                    while (Data.TwoPlayer.Count > 10)
                        Data.TwoPlayer.RemoveAt(Data.TwoPlayer.Count - 1);
                    break;
                case 3:
                    Data.ThreePlayer.Add(new LeaderboardEntry { Name = name, Score = score });
                    Data.ThreePlayer = Data.ThreePlayer.OrderByDescending(hs => hs.Score).ToList();
                    while (Data.ThreePlayer.Count > 10)
                        Data.ThreePlayer.RemoveAt(Data.ThreePlayer.Count - 1);
                    break;
                case 4:
                    Data.FourPlayer.Add(new LeaderboardEntry { Name = name, Score = score });
                    Data.FourPlayer = Data.FourPlayer.OrderByDescending(hs => hs.Score).ToList();
                    while (Data.FourPlayer.Count > 10)
                        Data.FourPlayer.RemoveAt(Data.FourPlayer.Count - 1);
                    break;
            }
        }

        public List<LeaderboardEntry> GetScores(int players)
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
    }
}
