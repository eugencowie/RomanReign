using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// The credits menu screen, which displays the game credits.
    /// </summary>
    class CreditsScreen : IScreen
    {
        RomanReignGame m_game;

        InputManager m_input => m_game.InputManager;
        ScreenManager m_screens => m_game.ScreenManager;
        Rectangle m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        Sprite m_heading;
        Sprite m_backButton;

        public CreditsScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            // Load the heading sprite.
            m_heading = new Sprite(content.Load<Texture2D>("Textures/Menu/Heading_Credits"));
            m_heading.Origin = m_heading.Texture.Bounds.Center.ToVector2();
            m_heading.Position.X = m_viewport.Center.X;
            m_heading.Position.Y = 100;

            // Load the back button sprite.
            m_backButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Back"));
            m_backButton.Origin = m_backButton.Texture.Bounds.Center.ToVector2();
            m_backButton.Position.X = m_viewport.Center.X;
            m_backButton.Position.Y = 600;
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
