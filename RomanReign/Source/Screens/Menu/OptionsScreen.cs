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
        Sprite m_musicLeftButton;
        Sprite m_musicRightButton;

        Sprite m_sfxButton;
        Sprite m_sfxLeftButton;
        Sprite m_sfxRightButton;

        Sprite m_backButton;

        Texture2D m_buttonBackground;
        Texture2D m_buttonBase;

        enum SelectedButton { None, Music, Sfx, Back, Final }
        SelectedButton m_selectedButton;

        enum SelectedDirection { None, Left, Right }
        SelectedDirection m_selectedDirection;

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

            float posX = m_game.Viewport.Right - (m_musicButton.Bounds.Size.X / 2f) - 80;
            float posY = m_game.Viewport.Height - (m_musicButton.Bounds.Size.Y / 2f) - 80 - (75 * 2);

            m_musicButton.Position = new Vector2(posX, posY);

            m_musicLeftButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_arrow_left")) {
                Position = new Vector2(posX - (m_musicButton.Bounds.Size.X / 2f) - 35, posY)
            };

            m_musicRightButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_arrow_right")) {
                Position = new Vector2(posX + (m_musicButton.Bounds.Size.X / 2f) + 35, posY)
            };

            m_sfxButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_options_sfx")) {
                Position = new Vector2(posX, posY += 75)
            };

            m_sfxLeftButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_arrow_left")) {
                Position = new Vector2(posX - (m_musicButton.Bounds.Size.X / 2f) - 35, posY)
            };

            m_sfxRightButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_arrow_right")) {
                Position = new Vector2(posX + (m_musicButton.Bounds.Size.X / 2f) + 35, posY)
            };

            m_backButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_back")) {
                Position = new Vector2(posX, posY += 75)
            };

            foreach (Sprite button in new[] {
                m_musicLeftButton,
                m_musicRightButton,
                m_sfxButton,
                m_sfxLeftButton,
                m_sfxRightButton,
                m_backButton })
            {
                button?.SetRelativeScale(0.75f, 0.75f);
                button?.SetRelativeOrigin(0.5f, 0.5f);
            }

            m_buttonBackground = content.Load<Texture2D>("Textures/Menu/btn_background");
            m_buttonBase = content.Load<Texture2D>("Textures/Menu/btn_options_base");

            m_font = content.Load<SpriteFont>("Fonts/menu");

            m_selectedButton = SelectedButton.None;
            m_selectedDirection = SelectedDirection.None;

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
                m_selectedDirection = SelectedDirection.None;

                if (m_musicLeftButton.Bounds.Contains(m_game.Input.Mouse.Position))
                {
                    m_selectedButton = SelectedButton.Music;
                    m_selectedDirection = SelectedDirection.Left;
                }

                if (m_musicRightButton.Bounds.Contains(m_game.Input.Mouse.Position))
                {
                    m_selectedButton = SelectedButton.Music;
                    m_selectedDirection = SelectedDirection.Right;
                }

                if (m_sfxLeftButton.Bounds.Contains(m_game.Input.Mouse.Position))
                {
                    m_selectedButton = SelectedButton.Sfx;
                    m_selectedDirection = SelectedDirection.Left;
                }

                if (m_sfxRightButton.Bounds.Contains(m_game.Input.Mouse.Position))
                {
                    m_selectedButton = SelectedButton.Sfx;
                    m_selectedDirection = SelectedDirection.Right;
                }

                if (m_backButton.Bounds.Contains(m_game.Input.Mouse.Position))
                    m_selectedButton = SelectedButton.Back;

                m_musicButton.SetOpacity(1f);
                m_sfxButton.SetOpacity(1f);

                m_musicLeftButton.SetOpacity(m_selectedButton == SelectedButton.Music && m_selectedDirection == SelectedDirection.Left ? 0.5f : 1f);
                m_musicRightButton.SetOpacity(m_selectedButton == SelectedButton.Music && m_selectedDirection == SelectedDirection.Right ? 0.5f : 1f);

                m_sfxLeftButton.SetOpacity(m_selectedButton == SelectedButton.Sfx && m_selectedDirection == SelectedDirection.Left ? 0.5f : 1f);
                m_sfxRightButton.SetOpacity(m_selectedButton == SelectedButton.Sfx && m_selectedDirection == SelectedDirection.Right ? 0.5f : 1f);
            }

            if (m_game.Input.MostRecentInputType == InputType.Gamepad)
            {
                m_selectedDirection = SelectedDirection.None;

                if (m_game.Input.IsJustPressed(Buttons.DPadUp) || m_game.Input.IsJustPressed(Buttons.LeftThumbstickUp))
                    m_selectedButton--;

                if (m_game.Input.IsJustPressed(Buttons.DPadDown) || m_game.Input.IsJustPressed(Buttons.LeftThumbstickDown))
                    m_selectedButton++;

                if (m_selectedButton == SelectedButton.None)
                    m_selectedButton = SelectedButton.None + 1;

                if (m_selectedButton == SelectedButton.Final)
                    m_selectedButton = SelectedButton.Final - 1;

                if (m_game.Input.IsJustPressed(Buttons.DPadLeft) || m_game.Input.IsJustPressed(Buttons.LeftThumbstickLeft))
                    m_selectedDirection = SelectedDirection.Left;

                if (m_game.Input.IsJustPressed(Buttons.DPadRight) || m_game.Input.IsJustPressed(Buttons.LeftThumbstickRight))
                    m_selectedDirection = SelectedDirection.Right;

                m_musicButton.SetOpacity(m_selectedButton == SelectedButton.Music ? 0.5f : 1f);
                m_sfxButton.SetOpacity(m_selectedButton == SelectedButton.Sfx ? 0.5f : 1f);

                m_musicLeftButton.SetOpacity(m_selectedButton == SelectedButton.Music && m_selectedDirection == SelectedDirection.Left ? 0.5f : 1f);
                m_musicRightButton.SetOpacity(m_selectedButton == SelectedButton.Music && m_selectedDirection == SelectedDirection.Right ? 0.5f : 1f);

                m_sfxLeftButton.SetOpacity(m_selectedButton == SelectedButton.Sfx && m_selectedDirection == SelectedDirection.Left ? 0.5f : 1f);
                m_sfxRightButton.SetOpacity(m_selectedButton == SelectedButton.Sfx && m_selectedDirection == SelectedDirection.Right ? 0.5f : 1f);
            }

            m_backButton.SetOpacity(m_selectedButton == SelectedButton.Back ? 0.5f : 1f);

            if (m_game.Input.IsJustReleased(MouseButtons.Left) ||
                m_game.Input.IsJustReleased(Buttons.A) ||
                m_game.Input.IsJustPressed(Buttons.DPadLeft) ||
                m_game.Input.IsJustPressed(Buttons.DPadRight) ||
                m_game.Input.IsJustPressed(Buttons.LeftThumbstickLeft) ||
                m_game.Input.IsJustPressed(Buttons.LeftThumbstickRight))
            {
                if (m_selectedButton == SelectedButton.Music)
                {
                    if (m_selectedDirection == SelectedDirection.Left)
                        m_game.Config.Data.Volume.Music -= 10;

                    if (m_selectedDirection == SelectedDirection.Right)
                        m_game.Config.Data.Volume.Music += 10;

                    m_selectedSound.Play(0.25f * m_game.Config.Data.Volume.MusicNormal, 0f, 0f);
                }

                if (m_selectedButton == SelectedButton.Sfx)
                {
                    if (m_selectedDirection == SelectedDirection.Left)
                        m_game.Config.Data.Volume.Sfx -= 10;

                    if (m_selectedDirection == SelectedDirection.Right)
                        m_game.Config.Data.Volume.Sfx += 10;

                    m_selectedSound.Play(0.25f * m_game.Config.Data.Volume.SfxNormal, 0f, 0f);
                }

                if (m_selectedButton == SelectedButton.Back)
                {
                    m_game.Screens.Pop();
                    m_selectedSound.Play(0.25f * m_game.Config.Data.Volume.SfxNormal, 0f, 0f);
                }
            }

            if (m_game.Input.IsJustReleased(Keys.Escape) || m_game.Input.IsJustReleased(Buttons.B))
            {
                m_game.Screens.Pop();
                m_selectedSound.Play(0.25f * m_game.Config.Data.Volume.SfxNormal, 0f, 0f);
            }
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (Sprite button in new[] {
                m_musicLeftButton,
                m_musicRightButton,
                m_sfxLeftButton,
                m_sfxRightButton,
                m_backButton })
            {
                spriteBatch.Draw(m_buttonBackground, button.Bounds, Color.White);
                button.Draw(spriteBatch);
            }

            spriteBatch.Draw(m_buttonBackground, m_musicButton.Bounds, Color.White);
            spriteBatch.Draw(m_buttonBase, m_musicButton.Bounds, m_musicButton.Color);
            var bar = new RectangleF(m_musicButton.Bounds);
            bar.Width *= m_game.Config.Data.Volume.MusicNormal;
            spriteBatch.Draw(m_buttonBackground, bar.ToRect(), Color.Lime);
            m_musicButton.Draw(spriteBatch);

            spriteBatch.Draw(m_buttonBackground, m_sfxButton.Bounds, Color.White);
            spriteBatch.Draw(m_buttonBase, m_sfxButton.Bounds, m_sfxButton.Color);
            bar = new RectangleF(m_sfxButton.Bounds);
            bar.Width *= m_game.Config.Data.Volume.SfxNormal;
            spriteBatch.Draw(m_buttonBackground, bar.ToRect(), Color.Lime);
            m_sfxButton.Draw(spriteBatch);

#if DEBUG
            //spriteBatch.DrawString(m_font, "Music volume: " + Config.Data.Volume.Music + "%", new Vector2(100, 100), Color.Black);
            //spriteBatch.DrawString(m_font, "Sound effect volume: " + Config.Data.Volume.Sfx + "%", new Vector2(100, 200), Color.Black);
#endif

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
