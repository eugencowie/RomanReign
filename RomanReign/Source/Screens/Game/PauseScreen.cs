using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// The pause screen, which is displayed when the game is paused.
    /// </summary>
    class PauseScreen : IScreen
    {
        RomanReignGame m_game;

        InputManager m_input => m_game.InputManager;
        ScreenManager m_screens => m_game.ScreenManager;
        Rectangle m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        Sprite m_fadeBackground;

        Sprite m_exitButton;

        public PauseScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            // Load the background sprite and scale it to cover the entire screen.
            m_fadeBackground = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Pause"));
            m_fadeBackground.ScaleToSize(m_viewport.Size.ToVector2());

            // Load the exit button sprite.
            m_exitButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Exit_White"));
            m_exitButton.Origin = new Vector2(0.5f, 0.5f);
            m_exitButton.Position.X = m_viewport.Center.X;
            m_exitButton.Position.Y = 400;
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            // If the escape key is pressed and released then remove this screen to return to the game.
            if (m_input.IsKeyJustReleased(Keys.Escape))
            {
                m_screens.Pop();
            }

            // If the left mouse button has just been pressed and released...
            if (m_input.IsMouseButtonJustReleased(MouseButtons.Left))
            {
                // ...and the mouse is positioned over the exit button then switch to the main menu.
                if (m_exitButton.Bounds.Contains(m_input.Mouse.Position))
                {
                    m_screens.SwitchTo(new MenuScreen(m_game));
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_fadeBackground.Draw(spriteBatch);

            m_exitButton.Draw(spriteBatch);

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
