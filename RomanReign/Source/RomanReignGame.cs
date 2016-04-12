using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    /// <summary>
    /// This is the main class for the game. It is a slightly modified version of the Game1
    /// class which is generated when a new MonoGame project is created.
    /// </summary>
    class RomanReignGame : Game
    {
        // These public variables allow other classes to access certain local variables in
        // this class (i.e. the input manager and screen manager local variables which are
        // declared in the next section of the code).

        public InputManager  InputManager  => m_inputManager;
        public ScreenManager ScreenManager => m_screenManager;

        // The initial Game1 class only had a sprite batch to start with. We've added
        // a screen manager which we will use to manage all of our active screens for
        // us as well as an input manager which we can use to check for certain input
        // events (such as a key being pressed and then released).

        SpriteBatch   m_spriteBatch;
        InputManager  m_inputManager;
        ScreenManager m_screenManager;

        /// <summary>
        /// The constructor is run when the program is launched. The only changes we have
        /// made to it are to specify our desired resolution and enable the mouse pointer.
        /// </summary>
        public RomanReignGame()
        {
            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                IsFullScreen = false
            };

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// This function is run when it's time to load the content for our game. In it we
        /// create the sprite batch and input/screen manager objects, and we then tell the
        /// screen manager to immediately switch to our splash screen.
        /// </summary>
        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            m_inputManager = new InputManager();
            m_screenManager = new ScreenManager(Content, m_spriteBatch);

            m_screenManager.SwitchTo(new SplashScreen(this));
        }
        
        /// <summary>
        /// This function is run when the game is exiting.  It is possible that our screens
        /// might have code that needs to be run before the game exits, for example to save
        /// the game. By using the screen manager's UnloadContent function we can make sure
        /// that any such code *will* be run before the game exits.
        /// </summary>
        protected override void UnloadContent()
        {
            m_screenManager.UnloadContent();
        }
        
        /// <summary>
        /// In addition to updating all of the active screens, this function also runs
        /// the Begin() and End() functions of the input manager which are required to
        /// make sure that the input manager works correctly.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            m_inputManager.Begin();

            m_screenManager.Update(gameTime);

            m_inputManager.End();

            base.Update(gameTime);
        }

        /// <summary>
        /// This function simply clears the screen and then tells the screen manager to
        /// draw all of the active screens.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
