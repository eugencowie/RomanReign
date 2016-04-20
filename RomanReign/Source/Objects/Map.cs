using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Map
    {
        public Rectangle Bounds => m_sprite.Bounds;

        Sprite m_sprite;

        public Map(ContentManager content, string mapPath)
        {
            m_sprite = new Sprite(content.Load<Texture2D>(mapPath));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_sprite.Draw(spriteBatch);
        }
    }
}
