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
        Game m_game;
        ScreenManager m_screenManager;

        Sprite m_background;
        Sprite m_heading;

        Sprite m_startButton;
        Sprite m_optionsButton;
        Sprite m_creditsButton;
        Sprite m_exitButton;

        bool m_isCovered;

        public MenuScreen(Game game, ScreenManager screenManager)
        {
            m_game = game;
            m_screenManager = screenManager;
        }

        public void LoadContent(ContentManager content)
        {
            Rectangle viewport = m_game.GraphicsDevice.Viewport.Bounds;

            // Load the background sprite and scale it to cover the entire screen.
            m_background = new Sprite(content.Load<Texture2D>("Textures/Menu/Background_Menu"));
            m_background.ScaleToSize(viewport.Size.ToVector2());

            // Load the heading sprite, set the origin to be the center of the sprite, and set the
            // position to be centered on the X axis and 100 pixels on the Y axis.
            m_heading = new Sprite(content.Load<Texture2D>("Textures/Menu/Heading_Menu"));
            m_heading.Origin = m_heading.Texture.Bounds.Center.ToVector2();
            m_heading.Position.X = viewport.Center.X;
            m_heading.Position.Y = 100;

            // Load the start button sprite and do the same as the previous sprite.
            m_startButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Start"));
            m_startButton.Origin = m_startButton.Texture.Bounds.Center.ToVector2();
            m_startButton.Position.X = viewport.Center.X;
            m_startButton.Position.Y = 300;

            // Load the options button sprite.
            m_optionsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Options"));
            m_optionsButton.Origin = m_optionsButton.Texture.Bounds.Center.ToVector2();
            m_optionsButton.Position.X = viewport.Center.X;
            m_optionsButton.Position.Y = 400;

            // Load the credits button sprite.
            m_creditsButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Credits"));
            m_creditsButton.Origin = m_creditsButton.Texture.Bounds.Center.ToVector2();
            m_creditsButton.Position.X = viewport.Center.X;
            m_creditsButton.Position.Y = 500;

            // Load the exit button sprite.
            m_exitButton = new Sprite(content.Load<Texture2D>("Textures/Menu/Button_Exit"));
            m_exitButton.Origin = m_exitButton.Texture.Bounds.Center.ToVector2();
            m_exitButton.Position.X = viewport.Center.X;
            m_exitButton.Position.Y = 600;
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
                // If the left mouse button has just been pressed and released...
                if (Input.IsMouseButtonJustReleased(MouseButtons.Left))
                {
                    // ...and the mouse is positioned over the start button, switch to the game screen.
                    if (m_startButton.Bounds.Contains(Input.Mouse.Position))
                    {
                        m_screenManager.SwitchTo(new GameScreen(m_game, m_screenManager));
                    }

                    // ...and the mouse is positioned over the options button, overlay the options screen.
                    if (m_optionsButton.Bounds.Contains(Input.Mouse.Position))
                    {
                        m_screenManager.Push(new OptionsScreen(m_game, m_screenManager));
                    }

                    // ...and the mouse is positioned over the credits button, overlay the credits screen.
                    if (m_creditsButton.Bounds.Contains(Input.Mouse.Position))
                    {
                        m_screenManager.Push(new CreditsScreen(m_game, m_screenManager));
                    }

                    // ...and the mouse is positioned over the exit button, quit the game.
                    if (m_exitButton.Bounds.Contains(Input.Mouse.Position))
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
