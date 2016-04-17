using Microsoft.Xna.Framework;

namespace RomanReign
{
    class Camera
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin   = Vector2.Zero;
        public float   Rotation = 0f;
        public float   Zoom     = 1f;

        private Vector2   TopLeft => Position - (Origin * Zoom);
        private Vector2   Size    => m_game.GraphicsDevice.Viewport.Bounds.Size.ToVector2() * Zoom;
        public  Rectangle Bounds  => new Rectangle((int)TopLeft.X, (int)TopLeft.Y, (int)Size.X, (int)Size.Y);

        GameScreen m_screen;
        RomanReignGame m_game;

        public Camera(GameScreen screen, RomanReignGame game)
        {
            m_screen = screen;
            m_game = game;
        }

        public void Update()
        {
            Position = m_screen.Player.Position;

            // Constrain the camera to the bounds of the map sprite.
            if (Bounds.Left < 0)
            {
                Position.X = Bounds.Width / 2f;
            }
            if (Bounds.Right > m_screen.Map.Bounds.Right)
            {
                Position.X = m_screen.Map.Bounds.Right - (Bounds.Width / 2f);
            }
            if (Bounds.Top < 0)
            {
                Position.Y = Bounds.Height / 2f;
            }
            if (Bounds.Bottom > m_screen.Map.Bounds.Bottom)
            {
                Position.Y = m_screen.Map.Bounds.Bottom - (Bounds.Height / 2f);
            }
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(Origin, 0f)) *
                Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1f);
        }
    }
}
