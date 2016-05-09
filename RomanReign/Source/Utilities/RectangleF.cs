using Microsoft.Xna.Framework;

namespace RomanReign
{
    class RectangleF
    {
        public Vector2 Location;
        public Vector2 Size;

        public Vector2 Center => Location + (Size / 2f);

        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;

        public float X
        {
            get { return Location.X; }
            set { Location.X = value; }
        }

        public float Y
        {
            get { return Location.Y; }
            set { Location.Y = value; }
        }

        public float Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }

        public float Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }

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

        public bool Contains(Vector2 point)
        {
            return (Left < point.X && point.X < Right) && (Top < point.Y && point.Y < Bottom);
        }

        public bool Contains(Point point)
        {
            return Contains(point.ToVector2());
        }

        public bool Intersects(RectangleF other)
        {
            return Left < other.Right && Right > other.Left && Top < other.Bottom && Bottom > other.Top;
        }

        public bool Intersects(Rectangle other)
        {
            return Intersects(new RectangleF(other));
        }

        public Rectangle ToRect()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }
    }
}
