using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// This is the first screen which is displayed when the game is run.  It displays a
    /// splash screen texture for three seconds before switching to the main menu screen.
    /// </summary>
    class SplashScreen : IScreen
    {
        // This variable allows us to access important functions and variables in the
        // main game class. You will see this variable in *all* of the screen classes.

        RomanReignGame m_game;

        // These variables are basically "shortcuts" to certain variables within the main
        // game class. They are only used so that we don't have to type out the whole bit
        // of code (for example to get the viewport size we can simply use the m_viewport
        // variable instead of m_game.GraphicsDevice.Viewport.Bounds).

        InputManager  m_input    => m_game.InputManager;
        ScreenManager m_screens  => m_game.ScreenManager;
        Rectangle     m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        // Our sprite class provides several useful functions for dealing with textures. In
        // the next bit of code, we declare a sprite variable for the background as well as
        // a floating point number to hold the number of seconds since the screen was shown.

        Sprite m_background;
        float  m_elapsedTime;

        /// <summary>
        /// This constructor is run when the splash screen object is created. The only
        /// thing it does is set up the m_game variable so that we can access it later.
        /// </summary>
        public SplashScreen(RomanReignGame game)
        {
            m_game = game;
        }

        /// <summary>
        /// This function is run when we add this screen to the screen manager. In it, we
        /// load the background sprite texture and scale it to cover the entire screen.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            m_background = new Sprite(content.Load<Texture2D>("Textures/Menu/bg_splash"));

            m_background.ScaleToSize(m_viewport.Size.ToVector2());
        }

        /// <summary>
        /// This function is called when the screen is removed from the screen manager, or
        /// if the game exits while the screen is active. We don't have anything that must
        /// be done when that happens, so we leave this function empty.
        /// </summary>
        public void UnloadContent()
        {
        }

        /// <summary>
        /// This function is called every frame while the screen is active. In it, we check
        /// if the elapsed time has reached three seconds or the escape key was pressed, if
        /// so then we switch to the main menu screen.  At the end we make sure to actually
        /// increase the elapsed time variable every frame.
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
        /// This function is called every frame while the screen is active. In it, we just
        /// draw the splash screen background sprite.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_background.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// This function is called when the screen is covered up by another screen. We
        /// are not 'pushing' any other screens on top of this one, so we don't need to
        /// do anything in this function.
        /// </summary>
        public void Covered()
        {
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed, making
        /// this the top-most screen. This screen should never have any screens on top of
        /// it so we do not need to do anything in this function.
        /// </summary>
        public void Uncovered()
        {
        }
    }
}
