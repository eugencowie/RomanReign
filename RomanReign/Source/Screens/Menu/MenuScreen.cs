using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    /// <summary>
    /// The main (or primary) menu screen, which contains buttons for starting/quitting the
    /// game as well as buttons to show the options menu screen or the credits menu screen.
    /// </summary>
    class MenuScreen : IScreen
    {
        RomanReignGame m_game;

        InputManager m_input => m_game.InputManager;
        ScreenManager m_screens => m_game.ScreenManager;
        Rectangle m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        Sprite m_background;
        Sprite m_heading;

        Sprite m_startButton;
        Sprite m_optionsButton;
        Sprite m_creditsButton;
        Sprite m_exitButton;

        bool m_isCovered;

        public MenuScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            m_background = new Sprite(content.Load<Texture2D>("Textures/Menu/Background_Menu"));
            m_background.ScaleToSize(m_viewport.Size.ToVector2());

            // These next sprites are all special because we want the origin of the sprite
            // texture (i.e. the 0,0 coordinate) to be in the center of the sprite (which
            // makes positioning them easier). To do this, we set the origin property. The
            // origin property is a Vector2 where 0,0 is top-left and 1,1 is bottom-right.

            m_heading = new Sprite(content.Load<Texture2D>("Textures/Menu/Heading_Menu")) {
                Position = new Vector2(m_viewport.Center.X, 100),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_startButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Start")) {
                Position = new Vector2(m_viewport.Center.X, 300),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_optionsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Options")) {
                Position = new Vector2(m_viewport.Center.X, 400),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_creditsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Credits")) {
                Position = new Vector2(m_viewport.Center.X, 500),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_exitButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Exit")) {
                Position = new Vector2(m_viewport.Center.X, 600),
                Origin = new Vector2(0.5f, 0.5f)
            };
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            // The options and credits screens will be overlaid on top of this screen, so we must
            // check to see whether this screen is currently being covered by any other any screens
            // before we run our update code.
            if (!m_isCovered)
            {
                foreach (Sprite button in new [] { m_startButton, m_optionsButton, m_creditsButton, m_exitButton })
                {
                    bool mouseOver = button.Bounds.Contains(m_input.Mouse.Position);
                    float opacity = mouseOver ? 0.5f : 1;
                    button.SetOpacity(opacity);
                }

                // If the left mouse button has just been pressed and released...
                if (m_input.IsMouseButtonJustReleased(MouseButtons.Left))
                {
                    // ...and the mouse is positioned over the start button, switch to the game screen.
                    if (m_startButton.Bounds.Contains(m_input.Mouse.Position))
                    {
                        m_screens.SwitchTo(new GameScreen(m_game));
                    }

                    // ...and the mouse is positioned over the options button, overlay the options screen.
                    if (m_optionsButton.Bounds.Contains(m_input.Mouse.Position))
                    {
                        m_screens.Push(new OptionsScreen(m_game));
                    }

                    // ...and the mouse is positioned over the credits button, overlay the credits screen.
                    if (m_creditsButton.Bounds.Contains(m_input.Mouse.Position))
                    {
                        m_screens.Push(new CreditsScreen(m_game));
                    }

                    // ...and the mouse is positioned over the exit button, quit the game.
                    if (m_exitButton.Bounds.Contains(m_input.Mouse.Position))
                    {
                        m_game.Exit();
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw the background, regardless of whether or not there are any other screens covering this one.
            m_background.Draw(spriteBatch);

            // But only draw the heading/buttons/etc if there are no other screens covering this one.
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

        public void Covered()
        {
            m_isCovered = true;
        }

        public void Uncovered()
        {
            m_isCovered = false;
        }
    }
}
