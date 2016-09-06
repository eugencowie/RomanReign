using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    class NameEntryScreen : IScreen
    {
        RomanReignGame m_game;
        GameScreen m_screen;

        SpriteFont m_font;

        public NameEntryScreen(GameScreen screen, RomanReignGame game)
        {
            m_game = game;
            m_screen = screen;
        }

        public void LoadContent(ContentManager content)
        {
            m_font = content.Load<SpriteFont>("Fonts/game");

            m_game.Input.OnKeyJustReleased += OnKeyJustReleased;
        }

        public void UnloadContent()
        {
            m_game.Input.OnKeyJustReleased -= OnKeyJustReleased;
        }

        public void Update(GameTime gameTime)
        {
            if (m_game.Input.IsJustReleased(Buttons.A) ||
                m_game.Input.IsJustReleased(Keys.Enter))
            {
                if (m_screen.PlayerNames.Length > 0 && !string.IsNullOrWhiteSpace(m_screen.PlayerNames))
                {
                    m_game.Leaderboard.AddHighScore(m_screen.NumberOfPlayers, m_screen.PlayerNames, m_screen.Score);
                    m_game.Screens.Pop();
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            string text = "Enter your " + (m_screen.NumberOfPlayers == 1 ? "name" : "names") + " using the keyboard:";
            spriteBatch.DrawString(m_font, text, new Vector2(450, 350), Color.Black);

            spriteBatch.DrawString(m_font, m_screen.PlayerNames, new Vector2(450, 450), Color.Black);

            spriteBatch.End();
        }

        public void Covered(IScreen other)
        {
        }

        public void Uncovered(IScreen other)
        {
        }

        private void OnKeyJustReleased(Keys key)
        {
            if (key == Keys.Back && m_screen.PlayerNames.Length > 0)
                m_screen.PlayerNames = m_screen.PlayerNames.Remove(m_screen.PlayerNames.Length - 1);

            if (m_screen.PlayerNames.Length >= 20)
                return;

            if (key >= Keys.A && key <= Keys.Z)
                m_screen.PlayerNames += key.ToString()[0];
            else if (key >= Keys.D0 && key <= Keys.D9)
                m_screen.PlayerNames += key.ToString()[1];
            else if (key == Keys.OemComma)
                m_screen.PlayerNames += ",";
            else if (key == Keys.OemPeriod)
                m_screen.PlayerNames += ".";
            else if (key == Keys.OemPlus)
                m_screen.PlayerNames += "+";
            else if (key == Keys.Space)
                m_screen.PlayerNames += " ";
        }
    }
}
