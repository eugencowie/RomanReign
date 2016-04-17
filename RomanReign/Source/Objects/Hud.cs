using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Hud
    {
        RomanReignGame m_game;

        AnimatedSprite m_testSprite;

        public Hud(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            m_testSprite = new AnimatedSprite(4, 1, 8, content.Load<Texture2D>("Textures/Game/PlayerWalking")) {
                Position = new Vector2(100, 100),
                Origin = new Vector2(0.5f, 0.5f)
            };
        }

        public void Update(GameTime gameTime)
        {
            m_testSprite.Update(gameTime);

            // Make semi-transparent on mouseover.
            bool mouseOver = m_testSprite.Bounds.Contains(m_game.InputManager.Mouse.Position);
            float opacity = (mouseOver ? 0.5f : 1f);
            m_testSprite.SetOpacity(opacity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_testSprite.Draw(spriteBatch);
        }
    }
}
