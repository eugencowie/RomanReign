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

        Sprite m_fadeBackground;
        Sprite m_exitButton;

        public PauseScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            // Load the background sprite and scale it to cover the entire screen.
            m_fadeBackground = new Sprite(content.Load<Texture2D>("Textures/Game/bg_pause"));
            m_fadeBackground.ScaleToSize(m_game.Viewport.Size.ToVector2());

            // Load the exit button sprite.
            m_exitButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_exit_white"));
            m_exitButton.Origin = new Vector2(0.5f, 0.5f);
            m_exitButton.Position.X = m_game.Viewport.Center.X;
            m_exitButton.Position.Y = 400;
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            // If the escape key is pressed and released then remove this screen to return to the game.
            if (m_game.Input.IsKeyJustReleased(Keys.Escape) ||
                m_game.Input.IsButtonJustReleased(Buttons.Start) ||
                m_game.Input.IsButtonJustReleased(Buttons.B))
            {
                m_game.Screens.Pop();
            }

            // If the left mouse button has just been pressed and released...
            if (m_game.Input.IsMouseButtonJustReleased(MouseButtons.Left))
            {
                // ...and the mouse is positioned over the exit button then switch to the main menu.
                if (m_exitButton.Bounds.Contains(m_game.Input.Mouse.Position))
                {
                    m_game.Screens.SwitchTo(new MenuScreen(m_game));
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
