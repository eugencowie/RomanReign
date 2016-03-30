using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    /// <summary>
    /// This is the main XNA class for the game.
    /// </summary>
    public class RomanReignGame : Game
    {
        SpriteBatch m_spriteBatch;
        ScreenManager m_screenManager;

        public RomanReignGame()
        {
            // Initialise the XNA graphics device.
            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                //IsFullScreen = true
            };

            // Make the mouse cursor visible. Note: does not work in fullscreen.
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }
        
        protected override void LoadContent()
        {
            // Create the sprite batch and screen manager.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_screenManager = new ScreenManager(Content, m_spriteBatch);

            // Switch to the splash screen as soon as the program is loaded.
            m_screenManager.SwitchTo(new SplashScreen(this, m_screenManager));
        }
        
        protected override void UnloadContent()
        {
            // When the game is exited, this function makes sure that every currently active screen's
            // Dispose() function is run.  Otherwise, the game would simply exit and any code in the
            // Dispose() function of any screen (for example, saving the game) would never get run.
            m_screenManager.Dispose();
        }
        
        protected override void Update(GameTime gameTime)
        {
            // This input helper has Begin() and End() functions just like a SpriteBatch, which
            // need to be called before and after the screenManager.Update() function.
            Input.Begin();

            // This function runs the Update() function of every currently active screen.
            m_screenManager.Update(gameTime);

            // See above.
            Input.End();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen.
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // This function runs the Draw() function of every currently active screen.
            m_screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
