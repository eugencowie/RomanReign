using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    class EndScreen : IScreen
    {
        RomanReignGame m_game;

        Sprite m_background;

        public EndScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            // Load the background sprite and scale it to cover the entire screen.
            m_background = new Sprite(content.Load<Texture2D>("Textures/Game/bg_gameover")) {
                Size = m_game.Viewport.Size.ToVector2()
            };
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (m_game.Input.IsJustReleased(Buttons.A) || m_game.Input.IsJustReleased(Keys.Enter))
            {
                m_game.Screens.SwitchTo(new GameScreen(m_game));
            }

            if (m_game.Input.IsJustReleased(Buttons.B) || m_game.Input.IsJustReleased(Keys.Escape))
            {
                m_game.Screens.SwitchTo(new MenuScreen(m_game));
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_background.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Covered(IScreen other)
        {
        }

        public void Uncovered(IScreen other)
        {
        }
    }
}
