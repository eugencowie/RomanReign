using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class HUD : IGameObject
    {
        Level m_level => GameScreen.Level;

        Sprite m_testSprite;

        public void Initialize(ContentManager content)
        {
            m_testSprite = new Sprite(content.Load<Texture2D>("Textures/HUD/Test"));
            m_testSprite.Position = new Vector2(20, 20);
        }

        public void Dispose()
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
