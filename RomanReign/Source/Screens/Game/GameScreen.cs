using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// The game screen, which contains the actual game.
    /// </summary>
    class GameScreen : IScreen
    {
        // These public fields allow certain game objects to be accessed from outside
        // of this class.

        public Hud    Hud    => m_hud;
        public Camera Camera => m_camera;
        public Map    Map    => m_map;
        public Player Player => m_player;

        // These are the same set of variables that are present in all of the screen
        // classes. See Screens/Menu/SplashScreen.cs for more information about them.

        RomanReignGame m_game;

        InputManager  m_input    => m_game.Input;
        ScreenManager m_screens  => m_game.Screens;
        Rectangle     m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        // Game objects.

        Hud    m_hud;
        Camera m_camera;
        Map    m_map;
        Player m_player;

        // This screen is designed to be covered up by other screens (such as the pause
        // screen).  We need a variable to keep track of when the screen is covered and
        // when it is not, which is what we use this boolean variable for.

        bool m_isCovered;

        /// <summary>
        /// This constructor is run when the game screen object is created.
        /// </summary>
        public GameScreen(RomanReignGame game)
        {
            m_game = game;
        }

        /// <summary>
        /// This function is run when we add this screen to the screen manager.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            m_hud = new Hud(content);

            m_camera = new Camera(this, m_game) {
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_map = new Map(content, "Textures/Game/bg_game");

            m_player = new Player(this, m_game, content);

            // Load the intro cutscene AFTER the game content has been loaded, so that when the
            // intro is finished the game can start immediately without needing to load anything.
            m_screens.Push(new IntroScreen(m_game));
        }

        /// <summary>
        /// This function is called when the screen is removed from the screen manager, or
        /// if the game exits while the screen is active.
        /// </summary>
        public void UnloadContent()
        {
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (!m_isCovered)
            {
                if (m_input.IsKeyJustReleased(Keys.Escape))
                {
                    m_screens.Push(new PauseScreen(m_game));
                }

                m_player.Update(gameTime);

                m_camera.Update();
            }
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // TODO: explanation of how the camera works.

            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            m_map.Draw(spriteBatch);
            m_player.Draw(spriteBatch);

            spriteBatch.End();

            // Now we want to draw the HUD without our coordinates being transformed using the
            // camera, so we need to call spriteBatch.Begin() again, this time without using
            // the camera transformation matrix.

            spriteBatch.Begin();

            m_hud.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// This function is called when the screen is covered up by another screen.
        /// </summary>
        public void Covered()
        {
            m_isCovered = true;
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed.
        /// </summary>
        public void Uncovered()
        {
            m_isCovered = false;
        }
    }
}
