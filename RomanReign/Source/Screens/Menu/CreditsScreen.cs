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
        Game m_game;
        ScreenManager m_screenManager;

        Sprite m_heading;
        Sprite m_backButton;

        public CreditsScreen(Game game, ScreenManager screenManager)
        {
            m_game = game;
            m_screenManager = screenManager;
        }

        public void Initialize(ContentManager content)
        {
            Rectangle viewport = m_game.GraphicsDevice.Viewport.Bounds;

            // Load the heading sprite.
            m_heading = new Sprite(content.Load<Texture2D>("Textures/Menu/Heading_Credits"));
            m_heading.Origin = m_heading.Texture.Bounds.Center.ToVector2();
            m_heading.Position.X = viewport.Center.X;
            m_heading.Position.Y = 100;

            // Load the back button sprite.
            m_backButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Back"));
            m_backButton.Origin = m_backButton.Texture.Bounds.Center.ToVector2();
            m_backButton.Position.X = viewport.Center.X;
            m_backButton.Position.Y = 600;
        }

        public void Dispose()
        {
        }

        public void Update(GameTime gameTime)
        {
            // If the escape key is pressed and released then remove this screen to return to the main menu.
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                m_screenManager.Pop();
            }

            // If the left mouse button has just been pressed and released...
            if (Input.IsMouseButtonJustReleased(MouseButtons.Left))
            {
                // ...and the mouse is positioned over the back button then remove this screen.
                if (m_backButton.Bounds.Contains(Input.Mouse.Position))
                {
                    m_screenManager.Pop();
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
