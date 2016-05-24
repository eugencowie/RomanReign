using Microsoft.Xna.Framework;
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

        Sprite m_backButton;

        Texture2D m_buttonBackground;

        SpriteFont m_font;

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

            m_backButton = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_back"));
            m_backButton.SetRelativeScale(0.75f, 0.75f);
            m_backButton.SetRelativeOrigin(0.5f, 0.5f);

            float posX = m_game.Viewport.Right - (m_backButton.Bounds.Size.X / 2f) - 50;
            float posY = m_game.Viewport.Height - (m_backButton.Bounds.Size.Y / 2f) - 50;

            m_backButton.Position = new Vector2(posX, posY);

            m_buttonBackground = content.Load<Texture2D>("Textures/Menu/btn_background");

            m_font = content.Load<SpriteFont>("Fonts/game");
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

            spriteBatch.Draw(m_buttonBackground, m_backButton.Bounds, Color.White);
            m_backButton.Draw(spriteBatch);

            spriteBatch.DrawString(m_font, "Music volume: " + Config.Data.Volume.Music + "%", new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(m_font, "Sound effect volume: " + Config.Data.Volume.Sfx + "%", new Vector2(100, 200), Color.White);

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
