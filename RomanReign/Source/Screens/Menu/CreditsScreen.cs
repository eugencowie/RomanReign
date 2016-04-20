﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// This is the credits menu screen, which displays the game credits. It is designed to
    /// be overlaid on top of another screen, so it does not have a background of its own.
    /// </summary>
    class CreditsScreen : IScreen
    {
        // These are the same set of variables that are present in all of the screen
        // classes. See Screens/Menu/SplashScreen.cs for more information about them.

        RomanReignGame m_game;

        InputManager  m_input    => m_game.Input;
        ScreenManager m_screens  => m_game.Screens;
        Rectangle     m_viewport => m_game.GraphicsDevice.Viewport.Bounds;

        // We need a sprite for the heading (or title) of the screen as well as a sprite
        // for the back button.

        Sprite m_heading;
        Sprite m_backButton;

        /// <summary>
        /// This constructor is run when the credits menu screen object is created.
        /// </summary>
        public CreditsScreen(RomanReignGame game)
        {
            m_game = game;
        }

        /// <summary>
        /// This function is run when we add this screen to the screen manager.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            // See Screens/Menu/MenuSreen.cs for an explanation of how sprite origins work.

            m_heading = new Sprite(content.Load<Texture2D>("Textures/Menu/title_credits")) {
                Position = new Vector2(m_viewport.Center.X, 100),
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_backButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_back")) {
                Position = new Vector2(m_viewport.Center.X, 600),
                Origin = new Vector2(0.5f, 0.5f)
            };
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
            if (m_input.IsKeyJustReleased(Keys.Escape))
            {
                m_screens.Pop();
            }

            if (m_input.IsMouseButtonJustReleased(MouseButtons.Left))
            {
                if (m_backButton.Bounds.Contains(m_input.Mouse.Position))
                {
                    m_screens.Pop();
                }
            }
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_heading.Draw(spriteBatch);
            m_backButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// This function is called when the screen is covered up by another screen.
        /// </summary>
        public void Covered()
        {
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed.
        /// </summary>
        public void Uncovered()
        {
        }
    }
}
