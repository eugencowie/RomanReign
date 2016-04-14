using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// The options menu screen, which allows the user to change game options.
    /// </summary>
    class OptionsScreen : IScreen
    {
        RomanReignGame m_game;

        InputManager m_input => m_game.InputManager;
        ScreenManager m_screens => m_game.ScreenManager;
        Rectangle m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        Sprite m_heading;
        Sprite m_backButton;

        public OptionsScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            // See Screens/Menu/MenuSreen.cs for an explanation of how sprite origins work.

            m_heading = new Sprite(content.Load<Texture2D>("Textures/Menu/Heading_Options")) {
                Position = new Vector2(m_viewport.Center.X, 100),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_backButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Back")) {
                Position = new Vector2(m_viewport.Center.X, 600),
                Origin = new Vector2(0.5f, 0.5f)
            };
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            // If the escape key is pressed and released then remove this screen to return to the main menu.
            if (m_input.IsKeyJustReleased(Keys.Escape))
            {
                m_screens.Pop();
            }

            // If the left mouse button has just been pressed and released...
            if (m_input.IsMouseButtonJustReleased(MouseButtons.Left))
            {
                // ...and the mouse is positioned over the back button then remove this screen.
                if (m_backButton.Bounds.Contains(m_input.Mouse.Position))
                {
                    m_screens.Pop();
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_heading.Draw(spriteBatch);

            m_backButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Covered()
        {
        }

        public void Uncovered()
        {
        }
    }
}
