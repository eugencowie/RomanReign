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
        RomanReignGame m_game;

        InputManager m_input => m_game.InputManager;
        ScreenManager m_screens => m_game.ScreenManager;
        Rectangle m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        // This field contains the current level. It can be access from ANY class simply by
        // using GameScreen.Level (for example, most game objects will contain a variable
        // which is set to point at this field so that they can access the level object).

        public static Level Level { get; private set; }

        bool m_isCovered;

        public GameScreen(RomanReignGame game)
        {
            m_game = game;

            Level = new Level();
        }

        public void LoadContent(ContentManager content)
        {
            // Load the level. This essentially loads every game object which is contained in the
            // level object, such as the map, player, enemies, etc.
            Level.Initialize(content);

            // Load the intro cutscene AFTER the game content has been loaded, so that when the
            // intro is finished the game can start immediately without needing to load anything.
            m_screens.Push(new IntroScreen(m_game));
        }

        public void UnloadContent()
        {
            Level.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            if (!m_isCovered)
            {
                if (m_input.IsKeyJustReleased(Keys.Escape))
                {
                    m_screens.Push(new PauseScreen(m_game));
                }

                // Update the level. This essentially updates every object within the level object.
                Level.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the level. This is the only screen where we do not call spriteBatch.Begin() and
            // spriteBatch.End(). Instead, those functions are called within the Level.Draw() function.
            //
            // This is because we need to call those spriteBatch functions multiple times with different
            // parameters in order to use the game camera. More details in the Level.Draw() function.
            //
            Level.Draw(gameTime, spriteBatch);
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
