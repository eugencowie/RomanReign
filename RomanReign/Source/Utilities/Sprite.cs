using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Sprite
    {
        public Texture2D Texture;
        public Rectangle? SourceRect = null;

        public Vector2 Origin = Vector2.Zero;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Size;

        public SpriteEffects Effects = SpriteEffects.None;
        public Color Color = Color.White;
        public bool Visible = true;

        public Rectangle Bounds => new RectangleF(Position - Origin, Size).ToRect();

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Size = Texture.Bounds.Size.ToVector2();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible && Texture != null)
            {
                spriteBatch.Draw(
                    Texture,
                    destinationRectangle: Bounds,
                    sourceRectangle: SourceRect,
                    color: Color,
                    effects: Effects);
            }
        }

        public void SetOpacity(float opacity)
        {
            byte alpha = (byte)(opacity * 255);
            Color.A = alpha;
        }

        public virtual void SetRelativeScale(Vector2 size)
        {
            if (SourceRect != null)
                Size = SourceRect.Value.Size.ToVector2() * size;
            else
                Size = Texture.Bounds.Size.ToVector2() * size;
        }

        public void SetRelativeScale(float width, float height)
        {
            SetRelativeScale(new Vector2(width, height));
        }

        public void SetRelativeScale(float scale)
        {
            SetRelativeScale(new Vector2(scale, scale));
        }

        public virtual void SetRelativeOrigin(Vector2 origin)
        {
            Origin = Size * origin;
        }

        public void SetRelativeOrigin(float originX, float originY)
        {
            SetRelativeOrigin(new Vector2(originX, originY));
        }
    }
}
