using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// This is the first screen which is displayed when the game is run. It displays a
    /// splash screen texture for three seconds before switching to the main menu screen.
    /// </summary>
    class SplashScreen : IScreen
    {
        // The game and screen manager variables are present in all of the screens. They
        // provide access to various important functions and properties, for example the
        // size of the screen.

        RomanReignGame m_game;

        InputManager  m_input    => m_game.InputManager;
        ScreenManager m_screens  => m_game.ScreenManager;
        Rectangle     m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        // The sprite class provides several useful properties for dealing with textures
        // such as scale and a bounding box for collision detection. Below we declare a
        // sprite for the background as well as a floating point number to represent the
        // number of seconds since the screen was loaded.

        Sprite m_background;
        float  m_elapsedTime;

        /// <summary>
        /// The constructor here simply sets the game and screen manager variables.
        /// </summary>
        public SplashScreen(RomanReignGame game)
        {
            m_game = game;
        }

        /// <summary>
        /// Load the background sprite and scale it to cover the entire screen.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            m_background = new Sprite(content.Load<Texture2D>("Textures/Menu/Background_Splash"));

            m_background.ScaleToSize(m_viewport.Size.ToVector2());
        }

        /// <summary>
        ///
        /// </summary>
        public void UnloadContent()
        {
        }

        /// <summary>
        /// If the elapsed time reaches three seconds or the escape key is pressed, switch
        /// to the main menu screen. Increase the elapsed time variable every frame.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (m_elapsedTime > 3f || m_input.IsKeyJustReleased(Keys.Escape))
            {
                m_screens.SwitchTo(new MenuScreen(m_game));
            }

            m_elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Draw the background sprite.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_background.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        ///
        /// </summary>
        public void Covered()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public void Uncovered()
        {
        }
    }
}
