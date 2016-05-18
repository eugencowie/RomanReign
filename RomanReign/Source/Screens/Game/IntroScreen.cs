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
        static bool m_shown;

        RomanReignGame m_game;

        Sprite m_background1;
        Sprite m_background2;
        Sprite m_background3;
        Sprite m_background4;

        SoundEffectInstance WalkingSound;

        float m_elapsedTime;

        public IntroScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            // Make sure that this screen is only ever shown once per session.

            if (m_shown)
            {
                m_game.Screens.Pop();
            }

            m_shown = true;

            // Load the background sprite and scale it to cover the entire screen.
            m_background1 = new Sprite(content.Load<Texture2D>("Textures/Game/bg_intro_1")) {
                Size = m_game.Viewport.Size.ToVector2()
            };

            // Load the second background sprite.
            m_background2 = new Sprite(content.Load<Texture2D>("Textures/Game/bg_intro_2")) {
                Size = m_game.Viewport.Size.ToVector2()
            };

            // Load the second background sprite.
            m_background3 = new Sprite(content.Load<Texture2D>("Textures/Game/bg_intro_3")) {
                Size = m_game.Viewport.Size.ToVector2()
            };

            // Load the second background sprite.
            m_background4 = new Sprite(content.Load<Texture2D>("Textures/Game/bg_intro_4")) {
                Size = m_game.Viewport.Size.ToVector2()
            };

            WalkingSound = content.Load<SoundEffect>("Audio/sfx_player_walking_long").CreateInstance();
            WalkingSound.Play();
        }

        public void UnloadContent()
        {
                WalkingSound.Stop();
        }

        public void Update(GameTime gameTime)
        {
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

            // Display the each image sequentially with a delay of 1.5 seconds between each one.
            if (m_elapsedTime < 1.5f)
            {
                m_background1.Draw(spriteBatch);
            }
            else if (m_elapsedTime < 3f)
            {
                m_background2.Draw(spriteBatch);
            }
            else if (m_elapsedTime < 4.5f)
            {
                m_background3.Draw(spriteBatch);
            }
            else if (m_elapsedTime < 6f)
            {
                m_background4.Draw(spriteBatch);
            }

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
