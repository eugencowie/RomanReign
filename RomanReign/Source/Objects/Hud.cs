using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Hud
    {
        RomanReignGame m_game;
        GameScreen m_screen;

        Texture2D m_heartTexture;

        SpriteFont m_gameFont;

        public Hud(GameScreen screen, RomanReignGame game, ContentManager content)
        {
            m_game = game;
            m_screen = screen;

            m_heartTexture = content.Load<Texture2D>("Textures/HUD/heart");

            m_gameFont = content.Load<SpriteFont>("Fonts/Game");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var player in m_screen.Players)
            {
                int displayLives = player.DisplayLives - 1;

                int padding = 5;
                Vector2 heartScale = new Vector2(0.3f);
                Vector2 scaledHeartSize = m_heartTexture.Bounds.Size.ToVector2() * heartScale;

                float totalWidth = (displayLives * scaledHeartSize.X) + (padding * (displayLives - 1));

                Vector2 playerPos = new Vector2(player.Bounds.Center.X, player.Bounds.Top);
                Vector2 playerScreenPos = m_screen.Camera.WorldToScreen(playerPos);
                Vector2 heartPos = new Vector2(playerScreenPos.X - (totalWidth / 2f), playerScreenPos.Y);

                for (int i = 0; i < displayLives; i++)
                {
                    spriteBatch.Draw(m_heartTexture, heartPos, color: Color.White, scale: heartScale);
                    heartPos.X += scaledHeartSize.X + padding;
                }
            }

            foreach (Enemy enemy in m_screen.Enemies)
            {
                int displayLives = enemy.DisplayLives - 1;

                int padding = 5;

                Vector2 heartScale = new Vector2(0.2f);
                Vector2 scaledHeartSize = m_heartTexture.Bounds.Size.ToVector2() * heartScale;

                float totalWidth = (displayLives * scaledHeartSize.X) + (padding * (displayLives - 1));

                Vector2 pos = new Vector2(enemy.Bounds.Center.X, enemy.Bounds.Top);
                Vector2 screenPos = m_screen.Camera.WorldToScreen(pos);
                Vector2 heartPos = new Vector2(screenPos.X - (totalWidth / 2f), screenPos.Y - 20);

                for (int i = 0; i < displayLives; i++)
                {
                    spriteBatch.Draw(m_heartTexture, heartPos, color: enemy.Color, scale: heartScale);
                    heartPos.X += scaledHeartSize.X + padding;
                }
            }

            spriteBatch.DrawString(m_gameFont, "Wave: " + m_screen.Wave, new Vector2(200, 100), Color.White);
            spriteBatch.DrawString(m_gameFont, "Enemies: " + m_screen.WaveEnemiesKilled + " / " + m_screen.WaveEnemies, new Vector2(200, 130), Color.White);

            if (m_screen.TimeSinceWaveStarted < GameScreen.WAVE_COOLDOWN)
            {
                spriteBatch.DrawString(m_gameFont, "NEW WAVE IN  " + (int)(GameScreen.WAVE_COOLDOWN - m_screen.TimeSinceWaveStarted + 1), new Vector2(500, 260), Color.White);
            }
        }
    }
}
