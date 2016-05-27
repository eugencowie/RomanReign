using Microsoft.Xna.Framework;
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
        // This variable allows us to access important functions and variables in the
        // main game class. You will see this variable in *all* of the screen classes.

        RomanReignGame m_game;

        // We need a sprite for the back button.

        Sprite m_backButton;

        Sprite m_background;

        Texture2D m_buttonBackground;

        SpriteFont m_font;

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
            m_background = new Sprite(content.Load<Texture2D>("Textures/Menu/bg_credits")) {
                Size = m_game.Viewport.Size.ToVector2()
            };

            // See Screens/Menu/MenuSreen.cs for an explanation of how sprite origins work.

            m_backButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_back"));
            m_backButton.SetRelativeScale(0.75f, 0.75f);
            m_backButton.SetRelativeOrigin(0.5f, 0.5f);

            float posX = m_game.Viewport.Right - (m_backButton.Bounds.Size.X / 2f) - 80;
            float posY = m_game.Viewport.Height - (m_backButton.Bounds.Size.Y / 2f) - 80;

            m_backButton.Position = new Vector2(posX, posY);

            m_buttonBackground = content.Load<Texture2D>("Textures/Menu/btn_background");

            m_font = content.Load<SpriteFont>("Fonts/menu");
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
            if (m_game.Input.MostRecentInputType == InputType.Gamepad)
                m_backButton.SetOpacity(0.5f);

            if (m_game.Input.MostRecentInputType == InputType.KBM)
            {
                bool mouseOver = m_backButton.Bounds.Contains(m_game.Input.Mouse.Position);
                m_backButton.SetOpacity(mouseOver ? 0.5f : 1f);
            }

            if (m_game.Input.IsJustReleased(Keys.Escape) ||
                m_game.Input.IsJustReleased(Buttons.B) ||
                m_game.Input.IsJustReleased(Buttons.A))
            {
                m_game.Screens.Pop();
            }

            if (m_game.Input.IsJustReleased(MouseButtons.Left))
            {
                if (m_backButton.Bounds.Contains(m_game.Input.Mouse.Position))
                {
                    m_game.Screens.Pop();
                }
            }
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_background.Draw(spriteBatch);

            spriteBatch.Draw(m_buttonBackground, m_backButton.Bounds, Color.White);
            m_backButton.Draw(spriteBatch);

            string text =
                "This game was developed by:\n\n" +
                "Eugén Cowie\n" +
                "Jordan Rawson\n" +
                "Gary Mulhall\n" +
                "Ross Thompson\n" +
                "Andrew Callaghan\n\n" +
                "Special thanks to:\n\n" +
                "West College Scotland\n" +
                "Lochfield Primary School";

            spriteBatch.DrawString(m_font, text, new Vector2(300, 150), Color.Black);

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
