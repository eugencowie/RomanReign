using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// This is the main class for the game.
    /// </summary>
    public class RomanReignGame : Game
    {
        SpriteBatch m_spriteBatch;
        ScreenManager m_screenManager;

        public RomanReignGame()
        {
            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            m_screenManager = new ScreenManager(Content, m_spriteBatch);
            m_screenManager.SwitchTo(new SplashScreen(this, m_screenManager));
        }
        
        protected override void UnloadContent()
        {
            m_screenManager.Dispose();
        }
        
        protected override void Update(GameTime gameTime)
        {
            m_screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
