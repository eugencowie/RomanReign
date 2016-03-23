using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    class GameScreen : IScreen
    {
        Game m_game;
        ScreenManager m_screenManager;

        public GameScreen(Game game, ScreenManager screenManager)
        {
            m_game = game;
            m_screenManager = screenManager;
        }

        public void Initialize(ContentManager content)
        {
        }

        public void Dispose()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                m_screenManager.SwitchTo(new MenuScreen(m_game, m_screenManager));
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public void Covered()
        {
        }

        public void Uncovered()
        {
        }
    }
}
