using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    class GameScreen : IScreen
    {
        Game m_game;
        ScreenManager m_screenManager;

        Sprite m_background;
        float m_backgroundScale = 0.01f;

        bool m_isCovered;

        public GameScreen(Game game, ScreenManager screenManager)
        {
            m_game = game;
            m_screenManager = screenManager;
        }

        public void Initialize(ContentManager content)
        {
            Rectangle viewport = m_game.GraphicsDevice.Viewport.Bounds;

            // Load the background sprite and scale it to cover the entire screen.
            m_background = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Game"));
            m_background.ScaleToSize(viewport.Size.ToVector2());

            // Load the intro cutscene AFTER the game content has been loaded, so that when the
            // intro is finished the game can start immediately without needing to load anything.
            m_screenManager.Push(new IntroScreen(m_game, m_screenManager));
        }

        public void Dispose()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (!m_isCovered)
            {
                if (Input.IsKeyJustReleased(Keys.Escape))
                {
                    m_screenManager.Push(new PauseScreen(m_game, m_screenManager));
                }

                m_background.UniformScale += m_backgroundScale;

                if (m_background.UniformScale > 1.5f || m_background.UniformScale < 0.5f)
                    m_backgroundScale *= -1;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_background.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Covered()
        {
            m_isCovered = true;
        }

        public void Uncovered()
        {
            m_isCovered = false;
        }
    }
}
