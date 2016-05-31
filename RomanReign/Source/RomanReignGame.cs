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
        // The initial Game1 class only had a sprite batch to start with. We've added
        // a screen manager which we will use to manage all of our active screens for
        // us, an input manager which will allow us to check for certain input events
        // such as a key being pressed and then released,  and a debug renderer which
        // we can use to draw collision rectangles.

        public Rectangle Viewport => GraphicsDevice.Viewport.Bounds;

        public InputManager Input;
        public ScreenManager Screens;
        public DebugRenderer Debug;
        public AudioManager Audio;

        private SpriteBatch m_spriteBatch;

        /// <summary>
        /// The constructor is run when the program is launched. The only changes we have
        /// made to it are to specify our desired resolution and enable the mouse pointer.
        /// </summary>
        public RomanReignGame()
        {
            Config.ReadConfig("config.xml");

            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Config.Data.Resolution.Width,
                PreferredBackBufferHeight = Config.Data.Resolution.Height,
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
            HighScoreTable.ReadHighScores("highscores.xml");

            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            Input = new InputManager();
            Screens = new ScreenManager(Content, m_spriteBatch);
            Debug = new DebugRenderer(GraphicsDevice, m_spriteBatch);
            Audio = new AudioManager();

            Screens.SwitchTo(new SplashScreen(this));
        }

        /// <summary>
        /// This function is run when the game is exiting.  It is possible that our screens
        /// might have code that needs to be run before the game exits, for example to save
        /// the game. By using the screen manager's UnloadContent function we can make sure
        /// that any such code *will* be run before the game exits.
        /// </summary>
        protected override void UnloadContent()
        {
            Screens.UnloadContent();

            HighScoreTable.WriteHighScores("highscores.xml");

            Config.WriteConfig("config.xml");
        }

        /// <summary>
        /// In addition to updating all of the active screens, this function also runs
        /// the Begin() and End() functions of the input manager which are required to
        /// make sure that the input manager works correctly.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            Audio.Update(gameTime);

            Screens.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This function simply clears the screen and then tells the screen manager to
        /// draw all of the active screens.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Screens.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
