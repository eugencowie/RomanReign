using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RomanReign
{
    /// <summary>
    /// This is the main menu screen, which contains buttons for starting/quitting the game
    /// as well as buttons to show the options menu screen or the credits menu screen. This
    /// screen is also responsible for drawing the menu background graphics which are drawn
    /// behind any other menu screens (such as options/credits screens).
    /// </summary>
    class MenuScreen : IScreen
    {
        // This variable allows us to access important functions and variables in the
        // main game class. You will see this variable in *all* of the screen classes.

        RomanReignGame m_game;

        // We need a video to use as the menu background. This video should always be
        // drawn when we have any of the menu screens activated.

        Video m_backgroundVideo;
        static VideoPlayer m_videoPlayer;

        // We need a whole bunch of sprites for all of the various buttons.

        Sprite m_startButton;
        Sprite m_optionsButton;
        Sprite m_creditsButton;
        Sprite m_exitButton;

        Texture2D m_buttonBackground;

        // This is used to identify the selected button.

        enum SelectedButton { None, Start, Options, Credits, Exit, Final }
        SelectedButton m_selectedButton;

        // Sound effect used when a button is pressed.

        SoundEffect m_selectedSound;

        // Unlike the splash screen, this screen is designed to be covered up by other menu
        // screens (such as the options screen or credits screen).  This means that we need
        // to know when the screen is covered and when it is not, which is what we use this
        // boolean variable for.

        bool m_isCovered;

        /// <summary>
        /// This constructor is run when the menu screen object is created.  Just like the
        /// other screens, the only thing it does is set up the m_game variable.
        /// </summary>
        public MenuScreen(RomanReignGame game)
        {
            m_game = game;
        }

        /// <summary>
        /// This function is run when we add this screen to the screen manager. In it, we
        /// load our background sprite as scale it to cover the entire screen. After that,
        /// we load the heading sprite and all of the button sprites.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            m_game.Audio.BackgroundMusic.TargetVolume = 0.5f;

            m_selectedSound = content.Load<SoundEffect>("Audio/sfx_menu_select");

            // Load the button textures.

            m_startButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_play"));
            m_startButton.SetRelativeScale(0.75f, 0.75f);
            m_startButton.SetRelativeOrigin(0.5f, 0.5f);

            float posX = m_game.Viewport.Right - (m_startButton.Bounds.Size.X / 2f) - 80;
            float posY = m_game.Viewport.Height - (m_startButton.Bounds.Size.Y / 2f) - 80 - (75 * 3);

            m_startButton.Position = new Vector2(posX, posY);

            m_optionsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_options")) {
                Position = new Vector2(posX, posY += 75)
            };

            m_creditsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_credits")) {
                Position = new Vector2(posX, posY += 75)
            };

            m_exitButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_quit")) {
                Position = new Vector2(posX, posY += 75)
            };

            foreach (Sprite button in new[] { m_optionsButton, m_creditsButton, m_exitButton })
            {
                button.SetRelativeScale(0.75f, 0.75f);
                button.SetRelativeOrigin(0.5f, 0.5f);
            }

            m_buttonBackground = content.Load<Texture2D>("Textures/Menu/btn_background");

            // Set the initial selected button to none.

            m_selectedButton = SelectedButton.None;

            // Load background video.

            m_backgroundVideo = content.Load<Video>("Video/main_menu");

            if (m_videoPlayer == null)
                m_videoPlayer = new VideoPlayer();

            try { m_videoPlayer.Play(m_backgroundVideo); }
            catch { }

            m_videoPlayer.Volume = 0f;
        }

        /// <summary>
        /// This function is called when the screen is removed from the screen manager, or
        /// if the game exits while the screen is active. We don't have anything that must
        /// be done when that happens, so we leave this function empty.
        /// </summary>
        public void UnloadContent()
        {
            m_game.Audio.BackgroundMusic.TargetVolume = 0.2f;

            m_videoPlayer.Stop();
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (m_videoPlayer.State != MediaState.Playing)
            {
                try { m_videoPlayer.Play(m_backgroundVideo); }
                catch { }

                m_videoPlayer.Volume = 0f;
            }

            // The options and credits screens will be overlaid on top of this screen, so
            // we must check to see whether this screen is currently being covered by any
            // other any screens before we run our update code.

            if (!m_isCovered)
            {
                // If the most recent input was using the keyboard/mouse, then check if the
                // mouse is over any of the buttons. If not then set the currently selected
                // button to none.

                if (m_game.Input.MostRecentInputType == InputType.KBM)
                {
                    m_selectedButton = SelectedButton.None;

                    if (m_startButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_selectedButton = SelectedButton.Start;

                    if (m_optionsButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_selectedButton = SelectedButton.Options;

                    if (m_creditsButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_selectedButton = SelectedButton.Credits;

                    if (m_exitButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_selectedButton = SelectedButton.Exit;
                }

                // If the most recent input was using a gamepad then make sure that the
                // currently selected button is not none. If for some reason it is then
                // default to the start button.

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

                // Set the opacity of each button to 0.5 (semi-transparent) if the button
                // is selected, otherwise 1 (fully opaque).

                m_startButton.SetOpacity(m_selectedButton == SelectedButton.Start ? 0.5f : 1f);
                m_optionsButton.SetOpacity(m_selectedButton == SelectedButton.Options ? 0.5f : 1f);
                m_creditsButton.SetOpacity(m_selectedButton == SelectedButton.Credits ? 0.5f : 1f);
                m_exitButton.SetOpacity(m_selectedButton == SelectedButton.Exit ? 0.5f : 1f);

                // Next, we check if the left mouse button has just been pressed and then
                // released. If so, we check to see if the mouse is over any of the buttons
                // and take any appropriate action.

                if (m_game.Input.IsJustReleased(MouseButtons.Left) || m_game.Input.IsJustReleased(Buttons.A))
                {
                    if (m_selectedButton == SelectedButton.Start)
                    {
                        m_game.Screens.Push(new PlayerSelectScreen(m_game));
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
                        m_game.Exit();
                        m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                    }
                }
            }
        }

        /// <summary>
        /// This function is called every frame while the screen is active. In it, we just
        /// draw all of the sprites.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // We always want to draw the background, regardless of whether or not there
            // are any other screens covering this one.

            Texture2D videoTexture = null;

            if (m_videoPlayer.State != MediaState.Stopped)
            {
                try { videoTexture = m_videoPlayer.GetTexture(); }
                catch { videoTexture = null; }
            }

            if (videoTexture != null)
            {
                spriteBatch.Draw(videoTexture, m_game.Viewport, Color.White);
            }

            // We only want to draw the heading/buttons/etc if there are no other screens
            // covering this one.

            if (!m_isCovered)
            {
                foreach (Sprite button in new[] { m_startButton, m_optionsButton, m_creditsButton, m_exitButton })
                {
                    spriteBatch.Draw(m_buttonBackground, button.Bounds, Color.White);
                    button.Draw(spriteBatch);
                }
            }

            spriteBatch.End();

            // Fix memory leak where video texture is not disposed.
            videoTexture?.Dispose();
        }

        /// <summary>
        /// This function is called when the screen is covered up by another screen. All
        /// we do here is set the m_isCovered variable to true.
        /// </summary>
        public void Covered(IScreen other)
        {
            m_isCovered = true;
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed, making
        /// this the top-most screen. All we do here is set m_isCovered to false.
        /// </summary>
        public void Uncovered(IScreen other)
        {
            m_isCovered = false;
        }
    }
}
