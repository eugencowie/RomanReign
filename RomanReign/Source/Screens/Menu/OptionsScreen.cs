using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// This is the options menu screen, which allows the user to change game options. It
    /// is designed to be overlaid on top of another screen, which is why it does not have
    /// a background of its own.
    /// </summary>
    class OptionsScreen : IScreen
    {
        // This variable allows us to access important functions and variables in the
        // main game class. You will see this variable in *all* of the screen classes.

        RomanReignGame m_game;

        // We need a sprite for the back button.

        Sprite m_musicButton;
        Sprite m_sfxButton;
        Sprite m_backButton;

        Texture2D m_buttonBackground;

        enum SelectedButton { None, Music, Sfx, Back, Final }
        SelectedButton m_selectedButton;

        SpriteFont m_font;

        SoundEffect m_selectedSound;

        /// <summary>
        /// This constructor is run when the options menu screen object is created.
        /// </summary>
        public OptionsScreen(RomanReignGame game)
        {
            m_game = game;
        }

        /// <summary>
        /// This function is run when we add this screen to the screen manager.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            // See Screens/Menu/MenuSreen.cs for an explanation of how sprite origins work.

            m_musicButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_options_music"));
            m_musicButton.SetRelativeScale(0.75f, 0.75f);
            m_musicButton.SetRelativeOrigin(0.5f, 0.5f);

            float posX = m_game.Viewport.Right - (m_musicButton.Bounds.Size.X / 2f) - 50;
            float posY = m_game.Viewport.Height - (m_musicButton.Bounds.Size.Y / 2f) - 50 - (75 * 2);

            m_musicButton.Position = new Vector2(posX, posY);

            m_sfxButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_options_sfx")) {
                Position = new Vector2(posX, posY += 75)
            };

            m_backButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_back")) {
                Position = new Vector2(posX, posY += 75)
            };

            foreach (Sprite button in new[] { m_sfxButton, m_backButton })
            {
                button.SetRelativeScale(0.75f, 0.75f);
                button.SetRelativeOrigin(0.5f, 0.5f);
            }

            m_buttonBackground = content.Load<Texture2D>("Textures/Menu/btn_background");

            m_font = content.Load<SpriteFont>("Fonts/menu");

            m_selectedButton = SelectedButton.None;

            m_selectedSound = content.Load<SoundEffect>("Audio/sfx_menu_select");
        }

        /// <summary>
        /// This function is called when the screen is removed from the screen manager, or
        /// if the game exits while the screen is active.
        /// </summary>
        public void UnloadContent()
        {
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (m_game.Input.MostRecentInputType == InputType.KBM)
            {
                m_selectedButton = SelectedButton.None;

                if (m_musicButton.Bounds.Contains(m_game.Input.Mouse.Position))
                    m_selectedButton = SelectedButton.Music;

                if (m_sfxButton.Bounds.Contains(m_game.Input.Mouse.Position))
                    m_selectedButton = SelectedButton.Sfx;

                if (m_backButton.Bounds.Contains(m_game.Input.Mouse.Position))
                    m_selectedButton = SelectedButton.Back;
            }

            if (m_game.Input.MostRecentInputType == InputType.Gamepad)
            {
                if (m_game.Input.IsJustPressed(Buttons.DPadUp) || m_game.Input.IsJustPressed(Buttons.LeftThumbstickUp))
                {
                    m_selectedButton--;
                }

                if (m_game.Input.IsJustPressed(Buttons.DPadDown) || m_game.Input.IsJustPressed(Buttons.LeftThumbstickDown))
                {
                    m_selectedButton++;
                }

                if (m_selectedButton == SelectedButton.None)
                    m_selectedButton = SelectedButton.None + 1;

                if (m_selectedButton == SelectedButton.Final)
                    m_selectedButton = SelectedButton.Final - 1;
            }

            m_musicButton.SetOpacity(m_selectedButton == SelectedButton.Music ? 0.5f : 1f);
            m_sfxButton.SetOpacity(m_selectedButton == SelectedButton.Sfx ? 0.5f : 1f);
            m_backButton.SetOpacity(m_selectedButton == SelectedButton.Back ? 0.5f : 1f);

            if (m_game.Input.IsJustReleased(MouseButtons.Left) || m_game.Input.IsJustReleased(Buttons.A))
            {
                if (m_selectedButton == SelectedButton.Music)
                {
                    m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                }

                if (m_selectedButton == SelectedButton.Sfx)
                {
                    m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                }

                if (m_selectedButton == SelectedButton.Back)
                {
                    m_game.Screens.Pop();
                    m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                }
            }



            if (m_game.Input.IsJustReleased(Keys.Q))
            {
                Config.Data.Volume.Music -= 10;
                m_game.Audio.BackgroundMusic.Volume = m_game.Audio.BackgroundMusic.TargetVolume * Config.Data.Volume.MusicNormal;
            }

            if (m_game.Input.IsJustReleased(Keys.E))
            {
                Config.Data.Volume.Music += 10;
                m_game.Audio.BackgroundMusic.Volume = m_game.Audio.BackgroundMusic.TargetVolume * Config.Data.Volume.MusicNormal;
            }

            if (m_game.Input.IsJustReleased(Keys.A))
                Config.Data.Volume.Sfx -= 10;

            if (m_game.Input.IsJustReleased(Keys.D))
                Config.Data.Volume.Sfx += 10;
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (Sprite button in new[] { m_musicButton, m_sfxButton, m_backButton })
            {
                spriteBatch.Draw(m_buttonBackground, button.Bounds, Color.White);
                button.Draw(spriteBatch);
            }

            spriteBatch.DrawString(m_font, "Music volume: " + Config.Data.Volume.Music + "%", new Vector2(100, 100), Color.Black);
            spriteBatch.DrawString(m_font, "Sound effect volume: " + Config.Data.Volume.Sfx + "%", new Vector2(100, 200), Color.Black);

            spriteBatch.End();
        }

        /// <summary>
        /// This function is called when the screen is covered up by another screen.
        /// </summary>
        public void Covered(IScreen other)
        {
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed.
        /// </summary>
        public void Uncovered(IScreen other)
        {
        }
    }
}
