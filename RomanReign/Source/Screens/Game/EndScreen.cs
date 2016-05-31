using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    class EndScreen : IScreen
    {
        RomanReignGame m_game;
        GameScreen m_screen;

        Sprite m_background;

        SpriteFont m_font;

        bool m_covered;

        public EndScreen(GameScreen screen, RomanReignGame game)
        {
            m_game = game;
            m_screen = screen;
        }

        public void LoadContent(ContentManager content)
        {
            // Load the background sprite and scale it to cover the entire screen.
            m_background = new Sprite(content.Load<Texture2D>("Textures/Game/bg_gameover")) {
                Size = m_game.Viewport.Size.ToVector2()
            };

            m_font = content.Load<SpriteFont>("Fonts/game");

            if (HighScoreTable.GetScores(m_screen.NumberOfPlayers).Count < 10 ||
                m_screen.Score >= HighScoreTable.GetLowestScore(m_screen.NumberOfPlayers).Score)
            {
                m_game.Screens.Push(new NameEntryScreen(m_screen, m_game));
            }
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (m_covered)
                return;

            if (m_game.Input.IsJustReleased(Buttons.A) || m_game.Input.IsJustReleased(Keys.Enter) ||
                m_game.Input.IsJustReleased(Buttons.B) || m_game.Input.IsJustReleased(Keys.Escape))
            {
                m_background.Visible = false;
                m_game.Screens.Push(new HighScoreScreen(m_screen, m_game));
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_background.Draw(spriteBatch);

            if (!m_covered)
            {
                string text =
                    $"You got to wave {m_screen.Wave}!\n\n" +
                    $"You killed {m_screen.Score} enemies!\n\n" +
                    (m_screen.Score >= HighScoreTable.GetLowestScore(m_screen.NumberOfPlayers).Score ? "NEW HIGH SCORE!\n\n" : "") +
                    $"The high score for {m_screen.NumberOfPlayers} player is {HighScoreTable.GetLowestScore(m_screen.NumberOfPlayers).Score}.";

                spriteBatch.DrawString(m_font, text, new Vector2(450, 350), Color.Black);
            }

            spriteBatch.End();
        }

        public void Covered(IScreen other)
        {
            m_covered = true;
        }

        public void Uncovered(IScreen other)
        {
            m_covered = false;
        }
    }
}
