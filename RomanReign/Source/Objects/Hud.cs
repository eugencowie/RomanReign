using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Hud
    {
        Sprite m_testSprite;

        public Hud(ContentManager content)
        {
            m_testSprite = new Sprite(content.Load<Texture2D>("Textures/HUD/Test")) {
                Position = new Vector2(20, 20)
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_testSprite.Draw(spriteBatch);
        }
    }
}
