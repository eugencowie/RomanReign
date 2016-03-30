using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// This is the first screen which is displayed when the game is run. It displays a splash
    /// screen for three seconds before switching to the main menu screen.
    /// </summary>
    class SplashScreen : IScreen
    {
        Game m_game;
        ScreenManager m_screenManager;

        Sprite m_background;

        float m_elapsedTime;

        public SplashScreen(Game game, ScreenManager screenManager)
        {
            m_game = game;
            m_screenManager = screenManager;
        }

        public void Initialize(ContentManager content)
        {
            Rectangle viewport = m_game.GraphicsDevice.Viewport.Bounds;

            // Load the background sprite and scale it to cover the entire screen.
            m_background = new Sprite(content.Load<Texture2D>("Textures/Menu/Background_Splash"));
            m_background.ScaleToSize(viewport.Size.ToVector2());

            m_elapsedTime = 0;
        }

        public void Dispose()
        {
        }

        public void Update(GameTime gameTime)
        {
            // If the elapsed time reaches three seconds, switch to the main menu screen.
            if (m_elapsedTime > 3f || Input.IsKeyJustReleased(Keys.Enter))
            {
                m_screenManager.SwitchTo(new MenuScreen(m_game, m_screenManager));
            }

            m_elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_background.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Covered()
        {
        }

        public void Uncovered()
        {
        }
    }
}
