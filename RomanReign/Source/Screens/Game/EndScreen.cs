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

        int m_numberOfPlayers;

        public EndScreen(GameScreen screen, RomanReignGame game, int numberOfPlayers)
        {
            m_game = game;
            m_screen = screen;
            m_numberOfPlayers = numberOfPlayers;
        }

        public void LoadContent(ContentManager content)
        {
            // Load the background sprite and scale it to cover the entire screen.
            m_background = new Sprite(content.Load<Texture2D>("Textures/Game/bg_gameover")) {
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
                m_game.Screens.SwitchTo(new GameScreen(m_game, m_numberOfPlayers, true));
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

            string text =
                $"You got to wave {m_screen.Wave}!\n\n" +
                $"You killed {m_screen.Score} enemies!\n\n" +
                (m_screen.Score >= m_screen.HighScore ? "NEW HIGH SCORE!\n\n" : "") +
                $"The high score for {m_screen.NumberOfPlayers} player is {m_screen.HighScore}.";

            spriteBatch.DrawString(m_font, text, new Vector2(450, 350), Color.Black);

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
