using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class DebugRenderer
    {
        public bool Enabled;

        SpriteBatch m_spriteBatch;
        Texture2D   m_debugPixel;

        public DebugRenderer(GraphicsDevice device, SpriteBatch spriteBatch)
        {
#if DEBUG
            m_spriteBatch = spriteBatch;

            m_debugPixel = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            Color[] color = new Color[1];
            color[0] = new Color(Color.White, 127);
            m_debugPixel.SetData(color);
#endif
        }

        public void Draw(Vector2 position, Vector2 size, Color? color=null)
        {
#if DEBUG
            Draw(new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y));
#endif
        }

        public void Draw(Rectangle rectangle, Color? color=null)
        {
#if DEBUG
            if (Enabled && m_spriteBatch != null && m_debugPixel != null)
            {
                m_spriteBatch.Draw(m_debugPixel, rectangle, color ?? Color.Red);
            }
#endif
        }
    }
}
