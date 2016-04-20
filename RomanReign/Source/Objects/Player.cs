using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Player
    {
        public Vector2 Position => m_testAnimation.Position;
        public Rectangle Bounds => m_testAnimation.Bounds;

        RomanReignGame m_game;
        GameScreen m_screen;

        AnimatedSprite m_testAnimation;

        public Player(GameScreen screen, RomanReignGame game, ContentManager content)
        {
            m_game = game;
            m_screen = screen;

            m_testAnimation = new AnimatedSprite(4, 1, 8, content.Load<Texture2D>("Textures/Game/player_walking")) {
                Position = new Vector2(1000, 928),
                Origin = new Vector2(0.5f, 0.5f)
            };
        }

        public void Update(GameTime gameTime)
        {
            m_testAnimation.Position.X += 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_testAnimation.Update(gameTime);

            // Make semi-transparent on mouseover.
            Vector2 mouseWorldCoords = m_screen.Camera.ScreenToWorld(m_game.Input.Mouse.Position.ToVector2());
            bool mouseOver = m_testAnimation.Bounds.Contains(mouseWorldCoords);
            float opacity = (mouseOver ? 0.5f : 1f);
            m_testAnimation.SetOpacity(opacity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_testAnimation.Draw(spriteBatch);
        }
    }
}
