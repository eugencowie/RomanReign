using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        // We need a sprite to use as the menu background.  This sprite should always be
        // drawn when we have any of the menu screens activated. For a brief description
        // of our sprite class, see Screens/Menu/SplashScreen.cs - or Utilties/Sprite.cs
        // if you want to see the actual code for the sprite class.

        Sprite m_background;

        // We have a sprite for the heading (or title) of the screen and then a whole
        // bunch of sprites for all of the various buttons.

        Sprite m_heading;
        Sprite m_startButton;
        Sprite m_optionsButton;
        Sprite m_creditsButton;
        Sprite m_exitButton;

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
            m_background = new Sprite(content.Load<Texture2D>("Textures/Menu/bg_menu"));
            m_background.ScaleToSize(m_game.Viewport.Size.ToVector2());

            // These next sprites are all special because we want the origin of the sprite
            // texture (i.e. the 0,0 coordinate) to be in the center of the sprite (which
            // makes positioning them easier). To do this, we set the origin property. The
            // origin property is a Vector2 where 0,0 is top-left and 1,1 is bottom-right.

            m_heading = new Sprite(content.Load<Texture2D>("Textures/Menu/title_menu")) {
                Position = new Vector2(m_game.Viewport.Center.X, 100),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_startButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_start")) {
                Position = new Vector2(m_game.Viewport.Center.X, 300),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_optionsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_options")) {
                Position = new Vector2(m_game.Viewport.Center.X, 400),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_creditsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_credits")) {
                Position = new Vector2(m_game.Viewport.Center.X, 500),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_exitButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_exit")) {
                Position = new Vector2(m_game.Viewport.Center.X, 600),
                Origin = new Vector2(0.5f, 0.5f)
            };
        }

        /// <summary>
        /// This function is called when the screen is removed from the screen manager, or
        /// if the game exits while the screen is active. We don't have anything that must
        /// be done when that happens, so we leave this function empty.
        /// </summary>
        public void UnloadContent()
        {
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // The options and credits screens will be overlaid on top of this screen, so
            // we must check to see whether this screen is currently being covered by any
            // other any screens before we run our update code.
            if (!m_isCovered)
            {
                // This is temporary and will be removed - it just makes any button that we
                // hover over slightly transparent.
                foreach (Sprite button in new [] { m_startButton, m_optionsButton, m_creditsButton, m_exitButton })
                {
                    bool mouseOver = button.Bounds.Contains(m_game.Input.Mouse.Position);
                    float opacity = mouseOver ? 0.5f : 1;
                    button.SetOpacity(opacity);
                }

                // Next, we check if the left mouse button has just been pressed and then
                // released. If so, we check to see if the mouse is over any of the buttons
                // and take any appropriate action.
                if (m_game.Input.IsMouseButtonJustReleased(MouseButtons.Left))
                {
                    if (m_startButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_game.Screens.SwitchTo(new GameScreen(m_game));

                    if (m_optionsButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_game.Screens.Push(new OptionsScreen(m_game));

                    if (m_creditsButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_game.Screens.Push(new CreditsScreen(m_game));

                    if (m_exitButton.Bounds.Contains(m_game.Input.Mouse.Position))
                        m_game.Exit();
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
            m_background.Draw(spriteBatch);

            // We only want to draw the heading/buttons/etc if there are no other screens
            // covering this one.
            if (!m_isCovered)
            {
                m_heading.Draw(spriteBatch);
                m_startButton.Draw(spriteBatch);
                m_optionsButton.Draw(spriteBatch);
                m_creditsButton.Draw(spriteBatch);
                m_exitButton.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// This function is called when the screen is covered up by another screen. All
        /// we do here is set the m_isCovered variable to true.
        /// </summary>
        public void Covered()
        {
            m_isCovered = true;
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed, making
        /// this the top-most screen. All we do here is set m_isCovered to false.
        /// </summary>
        public void Uncovered()
        {
            m_isCovered = false;
        }
    }
}
