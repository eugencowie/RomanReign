using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Hud : IGameObject
    {
        Sprite m_testSprite;

        public void LoadContent(ContentManager content)
        {
            m_testSprite = new Sprite(content.Load<Texture2D>("Textures/HUD/Test"));
            m_testSprite.Position = new Vector2(20, 20);
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            m_testSprite.Draw(spriteBatch);
        }
    }
}
