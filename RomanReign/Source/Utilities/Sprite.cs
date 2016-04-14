using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Sprite
    {
        public Texture2D Texture = null;
        public Vector2 Origin = Vector2.Zero;
        public Color Color = Color.White;

        public Vector2 Position = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public float UniformScale = 1f;

        public SpriteEffects Effects = SpriteEffects.None;

        public bool Visible = true;

        Vector2 m_absoluteOrigin => Texture.Bounds.Size.ToVector2() * Origin;
        Vector2 m_topLeft => Position - (m_absoluteOrigin * (Scale * UniformScale));
        Vector2 m_size => Texture.Bounds.Size.ToVector2() * (Scale * UniformScale);

        public Rectangle Bounds => new Rectangle((int)m_topLeft.X, (int)m_topLeft.Y, (int)m_size.X, (int)m_size.Y);

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible && Texture != null)
            {
                spriteBatch.Draw(
                    Texture,
                    Position,
                    null,
                    Color,
                    0f,
                    m_absoluteOrigin,
                    Scale * UniformScale,
                    Effects,
                    0f);
            }
        }

        public void SetAbsoluteOrigin(float absoluteX, float absoluteY)
        {
            Origin.X = absoluteX / Texture.Width;
            Origin.Y = absoluteY / Texture.Height;
        }

        public void ScaleToSize(float width, float height)
        {
            Scale.X = width / Texture.Width;
            Scale.Y = height / Texture.Height;
        }

        public void SetOpacity(float opacity)
        {
            byte alpha = (byte)(opacity * 255);
            Color.A = alpha;
        }

        public void SetAbsoluteOrigin(Vector2 absoluteOrigin)
        {
            SetAbsoluteOrigin(absoluteOrigin.X, absoluteOrigin.Y);
        }

        public void ScaleToSize(Vector2 size)
        {
            ScaleToSize(size.X, size.Y);
        }
    }
}
