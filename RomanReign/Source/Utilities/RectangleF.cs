using Microsoft.Xna.Framework;

namespace RomanReign
{
    class RectangleF
    {
        public float X, Y, Width, Height;

        public Vector2 Center   => new Vector2(X + (Width / 2f), Y + (Height / 2f));

        public Vector2 Position => new Vector2(X, Y);
        public Vector2 Size     => new Vector2(Width, Height);

        public float Left   => Position.X;
        public float Top    => Position.Y;
        public float Right  => Position.X + Width;
        public float Bottom => Position.Y + Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 position, Vector2 size)
            : this(position.X, position.Y, size.X, size.Y)
        {
        }

        public RectangleF(Rectangle rect)
            : this(rect.X, rect.Y, rect.Width, rect.Height)
        {
        }

        public bool Intersects(RectangleF other)
        {
            return Left < other.Right && Right > other.Left && Top < other.Bottom && Bottom > other.Top;
        }

        public Rectangle ToRect()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }
    }
}
