using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Sprite
    {
        public Texture2D Texture = null;
        public Rectangle? SourceRect = null;
        public Vector2 Origin = Vector2.Zero;
        public Color Color = Color.White;

        public Vector2 Position = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public float UniformScale = 1f;

        public SpriteEffects Effects = SpriteEffects.None;

        public bool Visible = true;

        protected         Vector2 AbsoluteScale   => Scale * UniformScale;
        protected virtual Vector2 AbsoluteSrcSize => Texture.Bounds.Size.ToVector2();

        protected Vector2 AbsoluteOrigin  => AbsoluteSrcSize * Origin;
        protected Vector2 AbsoluteTopLeft => Position - (AbsoluteOrigin * AbsoluteScale);
        protected Vector2 AbsoluteSize    => AbsoluteSrcSize * AbsoluteScale;

        public Rectangle Bounds
        {
            get { return new Rectangle((int)AbsoluteTopLeft.X, (int)AbsoluteTopLeft.Y, (int)AbsoluteSize.X, (int)AbsoluteSize.Y); }
        }

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
                    SourceRect,
                    Color,
                    0f,
                    AbsoluteOrigin,
                    AbsoluteScale,
                    Effects,
                    0f);
            }
        }

        public void SetOpacity(float opacity)
        {
            byte alpha = (byte)(opacity * 255);
            Color.A = alpha;
        }

        public void ScaleToSize(float width, float height)
        {
            Scale.X = width / Texture.Width;
            Scale.Y = height / Texture.Height;

            UniformScale = 1f;
        }

        public void ScaleToSize(Vector2 size)
        {
            ScaleToSize(size.X, size.Y);
        }

        public void SetAbsoluteOrigin(float absoluteX, float absoluteY)
        {
            Origin.X = absoluteX / Texture.Width;
            Origin.Y = absoluteY / Texture.Height;
        }

        public void SetAbsoluteOrigin(Vector2 absoluteOrigin)
        {
            SetAbsoluteOrigin(absoluteOrigin.X, absoluteOrigin.Y);
        }
    }
}
