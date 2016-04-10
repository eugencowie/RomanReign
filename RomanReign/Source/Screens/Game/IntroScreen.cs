using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    /// <summary>
    /// The intro cutscene screen, which displays a sequence of sprites.
    /// </summary>
    class IntroScreen : IScreen
    {
        Game m_game;
        ScreenManager m_screenManager;

        Sprite m_background1;
        Sprite m_background2;
        Sprite m_background3;
        Sprite m_background4;

        float m_elapsedTime;

        public IntroScreen(Game game, ScreenManager screenManager)
        {
            m_game = game;
            m_screenManager = screenManager;
        }

        public void LoadContent(ContentManager content)
        {
            Rectangle viewport = m_game.GraphicsDevice.Viewport.Bounds;

            // Load the background sprite and scale it to cover the entire screen.
            m_background1 = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Intro1"));
            m_background1.ScaleToSize(viewport.Size.ToVector2());

            // Load the second background sprite.
            m_background2 = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Intro2"));
            m_background2.ScaleToSize(viewport.Size.ToVector2());

            // Load the second background sprite.
            m_background3 = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Intro3"));
            m_background3.ScaleToSize(viewport.Size.ToVector2());

            // Load the second background sprite.
            m_background4 = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Intro4"));
            m_background4.ScaleToSize(viewport.Size.ToVector2());
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            // If the elapsed time reaches 7.5 seconds or the escape key is pressed, remove this screen.
            if (m_elapsedTime > 7.5f || Input.IsKeyJustReleased(Keys.Escape))
            {
                m_screenManager.Pop();
            }

            // Increase the elapsed time variable every frame.
            m_elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Display the each image sequentially with a delay of 1.5 seconds between each one.
            if (m_elapsedTime < 1.5f)
            {
                m_background1.Draw(spriteBatch);
            }
            else if (m_elapsedTime < 3f)
            {
                m_background2.Draw(spriteBatch);
            }
            else if (m_elapsedTime < 4.5f)
            {
                m_background3.Draw(spriteBatch);
            }
            else if (m_elapsedTime < 6f)
            {
                m_background4.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void Covered()
        {
        }

        public void Uncovered()
        {
        }
    }
}
