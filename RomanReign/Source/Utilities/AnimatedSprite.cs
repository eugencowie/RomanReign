﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class AnimatedSprite : Sprite
    {
        int m_columns;
        int m_rows;
        int m_fps;

        int m_totalFrames;
        int m_currentFrame;

        float m_timeSinceLastFrame;

        public AnimatedSprite(int columns, int rows, int fps, Texture2D spritesheet)
            : base(spritesheet)
        {
            m_columns = columns;
            m_rows = rows;
            m_fps = fps;

            m_totalFrames = m_columns * m_rows;

            Size.X = Texture.Width / m_columns;
            Size.Y = Texture.Height / m_rows;

            UpdateSourceRect();
        }

        public void Update(GameTime gameTime)
        {
            if (m_timeSinceLastFrame >= (1f / m_fps))
            {
                m_currentFrame++;

                if (m_currentFrame >= m_totalFrames)
                    m_currentFrame = 0;

                UpdateSourceRect();

                m_timeSinceLastFrame = 0;
            }

            m_timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdateSourceRect()
        {
            int width = Texture.Width / m_columns;
            int height = Texture.Height / m_rows;

            int row = (int)(m_currentFrame / (float)m_columns);
            int column = m_currentFrame % m_columns;

            SourceRect = new Rectangle(width * column, height * row, width, height);
        }

        public override void SetRelativeOrigin(Vector2 origin)
        {
            Origin = new Vector2(Texture.Width / m_columns, Texture.Height / m_rows) * origin;
        }
    }
}
