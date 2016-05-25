using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// The intro cutscene screen, which displays a sequence of sprites.
    /// </summary>
    class IntroScreen : IScreen
    {
        RomanReignGame m_game;

        Sprite m_background;
        Sprite m_player;

        SoundEffectInstance m_walkingSound;

        float m_elapsedTime;

        bool m_paused;

        public IntroScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            m_background = new Sprite(content.Load<Texture2D>("Maps/test")) {
                Position = m_game.Viewport.Center.ToVector2()
            };
            m_background.SetRelativeOrigin(0.5f, 0.5f);

            m_player = new Sprite(content.Load<Texture2D>("Textures/Game/player1_walking"));
            m_player.SourceRect = new Rectangle(0, 0, (int)(m_player.Texture.Width / 4f), m_player.Texture.Height);
            m_player.SetRelativeOrigin(0.5f, 0.5f);

            m_walkingSound = content.Load<SoundEffect>("Audio/sfx_player_walking_long").CreateInstance();
            m_walkingSound.Volume = 0.5f * Config.Data.Volume.SfxNormal;
            m_walkingSound.Play();

            m_game.Screens.Push(new TutorialScreen(m_game));
        }

        public void UnloadContent()
        {
            m_walkingSound?.Stop();
        }

        public void Update(GameTime gameTime)
        {
            if (m_paused)
                return;

            // Display the each image sequentially with a delay of 0.75 seconds between each one.
            if (m_elapsedTime < 0.75f)
            {
                m_player.Position = new Vector2(695, 505);
                m_player.SetRelativeScale(0.3f);
            }
            else if (m_elapsedTime < 1.5f)
            {
                m_player.Position = new Vector2(700, 520);
                m_player.SetRelativeScale(0.4f);
            }
            else if (m_elapsedTime < 2.25f)
            {
                m_player.Position = new Vector2(705, 535);
                m_player.SetRelativeScale(0.5f);
            }
            else if (m_elapsedTime < 3f)
            {
                m_player.Position = new Vector2(710, 550);
                m_player.SetRelativeScale(0.6f);
            }
            else if (m_elapsedTime < 3.75f)
            {
                m_player.Position = new Vector2(715, 565);
                m_player.SetRelativeScale(0.7f);
            }
            else if (m_elapsedTime < 4.5f)
            {
                m_player.Position = new Vector2(720, 580);
                m_player.SetRelativeScale(0.8f);
            }
            else if (m_elapsedTime < 5.25f)
            {
                m_player.Position = new Vector2(725, 595);
                m_player.SetRelativeScale(0.9f);
            }
            else if (m_elapsedTime < 6f)
            {
                m_player.Position = new Vector2(730, 610);
                m_player.SetRelativeScale(1f);
            }

            // If the elapsed time reaches 6 seconds or the escape key is pressed, remove this screen.
            if (m_elapsedTime > 6f ||
                m_game.Input.IsJustReleased(Keys.Escape) ||
                m_game.Input.IsJustReleased(Buttons.Start) ||
                m_game.Input.IsJustReleased(Buttons.A))
            {
                m_game.Screens.Pop();
            }

            // Increase the elapsed time variable every frame.
            m_elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_background.Draw(spriteBatch);

            m_player.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Covered(IScreen other)
        {
            m_paused = true;
        }

        public void Uncovered(IScreen other)
        {
            m_paused = false;
        }
    }
}
