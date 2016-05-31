using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    class HighScoreScreen : IScreen
    {
        RomanReignGame m_game;
        GameScreen m_screen;

        Sprite m_background;

        SpriteFont m_font;

        public HighScoreScreen(GameScreen screen, RomanReignGame game)
        {
            m_game = game;
            m_screen = screen;
        }

        public void LoadContent(ContentManager content)
        {
            m_background = new Sprite(content.Load<Texture2D>("Textures/Game/bg_gameover_scores")) {
                Size = m_game.Viewport.Size.ToVector2()
            };

            m_font = content.Load<SpriteFont>("Fonts/game");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (m_game.Input.IsJustReleased(Buttons.A) || m_game.Input.IsJustReleased(Keys.Enter))
            {
                m_game.Screens.SwitchTo(new GameScreen(m_game, m_screen.NumberOfPlayers, true));
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

            spriteBatch.DrawString(m_font, "HIGH SCORES", new Vector2(450, 150), Color.Black);

            string names = string.Join("\n", HighScoreTable.GetScores(m_screen.NumberOfPlayers).Select(s => s.Name));
            spriteBatch.DrawString(m_font, names, new Vector2(450, 250), Color.Black);

            string scores = string.Empty;
            foreach (int score in HighScoreTable.GetScores(m_screen.NumberOfPlayers).Select(s => s.Score))
            {
                if (score < 1000)
                    scores += "0";
                if (score < 100)
                    scores += "0";
                if (score < 10)
                    scores += "0";

                scores += score + "\n";
            }

            spriteBatch.DrawString(m_font, scores, new Vector2(800, 250), Color.Black);

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
