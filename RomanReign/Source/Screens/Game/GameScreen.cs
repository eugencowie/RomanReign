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

        bool m_isCovered;

        public GameScreen(Game game, ScreenManager screenManager)
        {
            m_game = game;
            m_screenManager = screenManager;
        }

        public void Initialize(ContentManager content)
        {
            m_screenManager.Push(new IntroScreen(m_game, m_screenManager));
        }

        public void Dispose()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (!m_isCovered)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    m_screenManager.SwitchTo(new MenuScreen(m_game, m_screenManager));
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
