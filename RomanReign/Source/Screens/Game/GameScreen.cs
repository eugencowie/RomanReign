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
        // Allow game objects to be accessed from outside of this class.

        public Hud    Hud    => m_hud;
        public Camera Camera => m_camera;
        public Map    Map    => m_map;
        public Player Player => m_player;

        // Standard screen variables.

        RomanReignGame m_game;
        InputManager   m_input    => m_game.InputManager;
        ScreenManager  m_screens  => m_game.ScreenManager;
        Rectangle      m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        // Game objects.

        Hud    m_hud;
        Camera m_camera;
        Map    m_map;
        Player m_player;

        // Other

        bool m_isCovered;

        public GameScreen(RomanReignGame game)
        {
            m_game = game;

            m_hud = new Hud();

            m_camera = new Camera(this, m_game) {
                Origin = m_viewport.Center.ToVector2()
            };

            m_map = new Map("Textures/Game/Background_Game");

            m_player = new Player(m_game);
        }

        public void LoadContent(ContentManager content)
        {
            m_hud.LoadContent(content);
            m_map.LoadContent(content);
            m_player.LoadContent(content);

            // Load the intro cutscene AFTER the game content has been loaded, so that when the
            // intro is finished the game can start immediately without needing to load anything.
            m_screens.Push(new IntroScreen(m_game));
        }

        public void UnloadContent()
        {
        }

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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //
            // TODO: explanation of how the camera works.
            //

            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            m_map.Draw(spriteBatch);
            m_player.Draw(spriteBatch);

            spriteBatch.End();

            //
            // Now we want to draw the HUD without our coordinates being transformed using the
            // camera, so we need to call spriteBatch.Begin() again, this time without using
            // the camera transformation matrix.
            //

            spriteBatch.Begin();

            m_hud.Draw(spriteBatch);

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
