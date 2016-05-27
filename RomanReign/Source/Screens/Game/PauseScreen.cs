using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// The pause screen, which is displayed when the game is paused.
    /// </summary>
    class PauseScreen : IScreen
    {
        RomanReignGame m_game;

        Sprite m_fadeBackground;

        Sprite m_resumeButton;
        Sprite m_optionsButton;
        Sprite m_creditsButton;
        Sprite m_exitButton;

        Texture2D m_buttonBackground;

        enum SelectedButton { None, Resume, Options, Credits, Exit, Final }
        SelectedButton m_selectedButton;

        SoundEffect m_selectedSound;

        bool m_isCovered;

        public PauseScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            m_selectedSound = content.Load<SoundEffect>("Audio/sfx_menu_select");

            // Load the background sprite and scale it to cover the entire screen.

            m_fadeBackground = new Sprite(content.Load<Texture2D>("Textures/Game/bg_pause")) {
                Size = m_game.Viewport.Size.ToVector2()
            };

            // Load the button sprites.

            m_resumeButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_resume"));
            m_resumeButton.SetRelativeScale(0.75f, 0.75f);
            m_resumeButton.SetRelativeOrigin(0.5f, 0.5f);

            float posX = m_game.Viewport.Right - (m_resumeButton.Bounds.Size.X / 2f) - 80;
            float posY = m_game.Viewport.Height - (m_resumeButton.Bounds.Size.Y / 2f) - 80 - (75 * 3);

            m_resumeButton.Position = new Vector2(posX, posY);

            m_optionsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_options")) {
                Position = new Vector2(posX, posY += 75)
            };

            m_creditsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_credits")) {
                Position = new Vector2(posX, posY += 75)
            };

            m_exitButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_menu")) {
                Position = new Vector2(posX, posY += 75)
            };

            foreach (Sprite button in new[] { m_optionsButton, m_creditsButton, m_exitButton })
            {
                button.SetRelativeScale(0.75f, 0.75f);
                button.SetRelativeOrigin(0.5f, 0.5f);
            }

            m_buttonBackground = content.Load<Texture2D>("Textures/Menu/btn_background");

            m_selectedButton = SelectedButton.None;
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (!m_isCovered)
            {
                if (m_game.Input.MostRecentInputType == InputType.KBM)
                {
                    m_selectedButton = SelectedButton.None;

                    if (m_resumeButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_selectedButton = SelectedButton.Resume;

                    if (m_optionsButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_selectedButton = SelectedButton.Options;

                    if (m_creditsButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_selectedButton = SelectedButton.Credits;

                    if (m_exitButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_selectedButton = SelectedButton.Exit;
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

                m_resumeButton.SetOpacity(m_selectedButton == SelectedButton.Resume ? 0.5f : 1f);
                m_optionsButton.SetOpacity(m_selectedButton == SelectedButton.Options ? 0.5f : 1f);
                m_creditsButton.SetOpacity(m_selectedButton == SelectedButton.Credits ? 0.5f : 1f);
                m_exitButton.SetOpacity(m_selectedButton == SelectedButton.Exit ? 0.5f : 1f);

                if (m_game.Input.IsJustReleased(MouseButtons.Left) || m_game.Input.IsJustReleased(Buttons.A))
                {
                    if (m_selectedButton == SelectedButton.Resume)
                    {
                        m_game.Screens.Pop();
                        m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                    }

                    if (m_selectedButton == SelectedButton.Options)
                    {
                        m_game.Screens.Push(new OptionsScreen(m_game));
                        m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                    }

                    if (m_selectedButton == SelectedButton.Credits)
                    {
                        m_game.Screens.Push(new CreditsScreen(m_game));
                        m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                    }

                    if (m_selectedButton == SelectedButton.Exit)
                    {
                        m_game.Screens.SwitchTo(new MenuScreen(m_game));
                        m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                    }
                }
            }

            // If the escape key is pressed and released then remove this screen to return to the game.
            if (m_game.Input.IsJustReleased(Keys.Escape) ||
                m_game.Input.IsJustReleased(Buttons.Start) ||
                m_game.Input.IsJustReleased(Buttons.B))
            {
                m_game.Screens.Pop();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_fadeBackground.Draw(spriteBatch);

            if (!m_isCovered)
            {
                foreach (Sprite button in new[] { m_resumeButton, m_optionsButton, m_creditsButton, m_exitButton })
                {
                    spriteBatch.Draw(m_buttonBackground, button.Bounds, Color.White);
                    button.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        public void Covered(IScreen other)
        {
            m_isCovered = true;
        }

        public void Uncovered(IScreen other)
        {
            m_isCovered = false;
        }
    }
}
