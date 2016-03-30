using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    class PauseScreen : IScreen
    {
        Game m_game;
        ScreenManager m_screenManager;

        Sprite m_fadeBackground;

        Sprite m_exitButton;

        public PauseScreen(Game game, ScreenManager screenManager)
        {
            m_game = game;
            m_screenManager = screenManager;
        }

        public void Initialize(ContentManager content)
        {
            Rectangle viewport = m_game.GraphicsDevice.Viewport.Bounds;

            // Load the background sprite and scale it to cover the entire screen.
            m_fadeBackground = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Pause"));
            m_fadeBackground.ScaleToSize(viewport.Size.ToVector2());

            m_exitButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Exit_White"));
            m_exitButton.Origin = m_exitButton.Texture.Bounds.Center.ToVector2();
            m_exitButton.Position.X = viewport.Center.X;
            m_exitButton.Position.Y = 400;
        }

        public void Dispose()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (Input.IsKeyJustReleased(Keys.Escape))
            {
                m_screenManager.Pop();
            }

            if (Input.IsMouseButtonJustReleased(MouseButtons.Left))
            {
                if (m_exitButton.Bounds.Contains(Input.Mouse.Position))
                {
                    m_screenManager.SwitchTo(new MenuScreen(m_game, m_screenManager));
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
