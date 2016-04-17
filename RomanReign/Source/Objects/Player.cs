using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Player
    {
        RomanReignGame m_game;

        AnimatedSprite m_testAnimation;

        public Vector2 Position => m_testAnimation.Position;

        public Player(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            m_testAnimation = new AnimatedSprite(4, 1, 8, content.Load<Texture2D>("Textures/Game/PlayerWalking")) {
                Position = new Vector2(1000, 928),
                Origin = new Vector2(0.5f, 0.5f)
            };
        }

        public void Update(GameTime gameTime)
        {
            m_testAnimation.Position.X += 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_testAnimation.Update(gameTime);

            // Make semi-transparent on mouseover.
            bool mouseOver = m_testAnimation.Bounds.Contains(m_game.InputManager.Mouse.Position);
            float opacity = (mouseOver ? 0.5f : 1f);
            m_testAnimation.SetOpacity(opacity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_testAnimation.Draw(spriteBatch);
        }
    }
}
