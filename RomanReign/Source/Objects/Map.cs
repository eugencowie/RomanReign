using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace RomanReign
{
    class MapInfo
    {
        public Vector2 PlayerSpawn = Vector2.Zero;

        public List<Rectangle> CollisionList = new List<Rectangle>();
    }

    class Map
    {
        public MapInfo   Info   => m_info;
        public Rectangle Bounds => m_sprite.Bounds;

        RomanReignGame m_game;

        Sprite m_sprite;
        MapInfo m_info;

        List<StaticBody> m_physicsBodies = new List<StaticBody>();

        public Map(RomanReignGame game, ContentManager content, string mapPath)
        {
            m_game = game;

            m_sprite = new Sprite(content.Load<Texture2D>(mapPath));

            string infoFile = Path.Combine(content.RootDirectory, mapPath) + ".csv";
            m_info = LoadInfoFile(infoFile);

            foreach (var rect in m_info.CollisionList)
            {
                StaticBody body = new StaticBody {
                    Position = rect.Location.ToVector2(),
                    Size = rect.Size.ToVector2()
                };

                m_physicsBodies.Add(body);
                m_game.Physics.AddStaticBody(body);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_sprite.Draw(spriteBatch);

            foreach (var body in m_physicsBodies)
            {
                m_game.Debug.Draw(body.Bounds.ToRect());
            }
        }

        private MapInfo LoadInfoFile(string infoFile)
        {
            MapInfo mapInfo = new MapInfo();

            if (!File.Exists(infoFile))
                throw new ArgumentException(infoFile + " does not exist");

            foreach (string row in File.ReadAllLines(infoFile))
            {
                string[] columns = row.Split(',');

                if (columns.Length != 5)
                    throw new ArgumentException("invalid number of columns: " + columns.Length);

                string id = columns[0].ToLower().Trim();
                if (id == "playerspawn")
                {
                    int x, y;

                    try
                    {
                        int.TryParse(columns[1], out x);
                        int.TryParse(columns[2], out y);
                    }
                    catch
                    {
                        x = 0;
                        y = 0;
                    }

                    mapInfo.PlayerSpawn = new Vector2(x, y);
                }
                else if (id == "collision")
                {
                    int x, y, width, height;

                    try
                    {
                        int.TryParse(columns[1], out x);
                        int.TryParse(columns[2], out y);
                        int.TryParse(columns[3], out width);
                        int.TryParse(columns[4], out height);
                    }
                    catch
                    {
                        x = 0;
                        y = 0;
                        width = 0;
                        height = 0;
                    }

                    mapInfo.CollisionList.Add(new Rectangle(x, y, width, height));
                }
            }

            return mapInfo;
        }
    }
}
