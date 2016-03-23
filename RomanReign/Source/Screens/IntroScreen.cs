﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
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

        public void Initialize(ContentManager content)
        {
            Rectangle viewport = m_game.GraphicsDevice.Viewport.Bounds;

            // Load the background sprite and scale it to cover the entire screen.
            m_background1 = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Intro1"));
            m_background1.Scale.X = (float)viewport.Width / m_background1.Texture.Width;
            m_background1.Scale.Y = (float)viewport.Height / m_background1.Texture.Height;

            m_background2 = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Intro2"));
            m_background2.Scale.X = (float)viewport.Width / m_background2.Texture.Width;
            m_background2.Scale.Y = (float)viewport.Height / m_background2.Texture.Height;

            m_background3 = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Intro3"));
            m_background3.Scale.X = (float)viewport.Width / m_background3.Texture.Width;
            m_background3.Scale.Y = (float)viewport.Height / m_background3.Texture.Height;

            m_background4 = new Sprite(content.Load<Texture2D>("Textures/Game/Background_Intro4"));
            m_background4.Scale.X = (float)viewport.Width / m_background4.Texture.Width;
            m_background4.Scale.Y = (float)viewport.Height / m_background4.Texture.Height;

            m_elapsedTime = 0;
        }

        public void Dispose()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (m_elapsedTime > 7.5f)
            {
                m_screenManager.Pop();
            }

            m_elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

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
