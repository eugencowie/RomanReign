using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Hud
    {
        RomanReignGame m_game;
        GameScreen m_screen;

        SpriteFont m_gameFont;

        public Hud(GameScreen screen, RomanReignGame game, ContentManager content)
        {
            m_game = game;
            m_screen = screen;

            m_gameFont = content.Load<SpriteFont>("Fonts/Game");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(m_gameFont, "Lives: " + m_screen.Player.DisplayLives, new Vector2(200, 60), Color.White);

            spriteBatch.DrawString(m_gameFont, "Wave: " + m_screen.Wave, new Vector2(200, 100), Color.White);
            spriteBatch.DrawString(m_gameFont, "Enemies: " + m_screen.WaveEnemiesKilled + " / " + m_screen.WaveEnemies, new Vector2(200, 130), Color.White);

            if (m_screen.TimeSinceWaveStarted < GameScreen.WAVE_COOLDOWN)
            {
                spriteBatch.DrawString(m_gameFont, "NEW WAVE IN  " + (int)(GameScreen.WAVE_COOLDOWN - m_screen.TimeSinceWaveStarted + 1), new Vector2(500, 260), Color.White);
            }
        }
    }
}
