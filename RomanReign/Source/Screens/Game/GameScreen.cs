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
        public Camera Camera => m_camera;
        public Hud Hud => m_hud;

        RomanReignGame m_game;
        InputManager m_input => m_game.InputManager;
        ScreenManager m_screens => m_game.ScreenManager;
        Rectangle m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        Camera m_camera;
        Hud m_hud;

        bool m_isCovered;

        public GameScreen(RomanReignGame game)
        {
            m_game = game;

            m_camera = new Camera();
            m_hud = new Hud(m_game);
        }

        public void LoadContent(ContentManager content)
        {
            m_hud.LoadContent(content);

            // Load the intro cutscene AFTER the game content has been loaded, so that when the
            // intro is finished the game can start immediately without needing to load anything.
            m_screens.Push(new IntroScreen(m_game));
        }

        public void UnloadContent()
        {
            m_hud.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (!m_isCovered)
            {
                if (m_input.IsKeyJustReleased(Keys.Escape))
                {
                    m_screens.Push(new PauseScreen(m_game));
                }

                m_hud.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //
            // TODO: explanation of how the camera works.
            //

            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            spriteBatch.End();

            //
            // Now we want to draw the HUD without our coordinates being transformed using the
            // camera, so we need to call spriteBatch.Begin() again, this time without using
            // the camera transformation matrix.
            //

            spriteBatch.Begin();

            m_hud.Draw(gameTime, spriteBatch);

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
