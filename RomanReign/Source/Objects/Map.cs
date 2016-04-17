using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Map
    {
        Sprite m_sprite;
        string m_mapPath;

        public Rectangle Bounds => m_sprite.Bounds;

        public Map(string mapPath)
        {
            m_mapPath = mapPath;
        }

        public void LoadContent(ContentManager content)
        {
            m_sprite = new Sprite(content.Load<Texture2D>(m_mapPath));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_sprite.Draw(spriteBatch);
        }
    }
}
