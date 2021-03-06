using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace RomanReign
{
    class Camera
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin   = Vector2.Zero;
        public float   Rotation = 0f;
        public float   Zoom     = 1f;

        private Vector2   AbsOrigin => m_game.GraphicsDevice.Viewport.Bounds.Size.ToVector2() * Origin;
        private Vector2   TopLeft   => Position - (AbsOrigin * Zoom);
        private Vector2   Size      => m_game.GraphicsDevice.Viewport.Bounds.Size.ToVector2() * Zoom;
        public  Rectangle Bounds    => new Rectangle((int)TopLeft.X, (int)TopLeft.Y, (int)Size.X, (int)Size.Y);

        GameScreen m_screen;
        RomanReignGame m_game;

        public Camera(GameScreen screen, RomanReignGame game)
        {
            m_screen = screen;
            m_game = game;
        }

        public void Update(GameTime gameTime)
        {
            if (m_screen.Players.Count > 0)
            {
                List<Vector2> targets = m_screen.Players.Select(player => player.Position).ToList();
                Vector2 average = targets.Aggregate(Vector2.Zero, (current, t) => current + t) / targets.Count;

                Position = Vector2.Lerp(Position, average, 10 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                Position = Vector2.Lerp(Position, m_screen.Map.Bounds.Center.ToVector2(), (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            // Constrain the camera to the bounds of the map sprite.
            if (Bounds.Left < m_screen.Map.Bounds.Left)
            {
                Position.X = m_screen.Map.Bounds.Left + (Bounds.Width / 2f);
            }
            if (Bounds.Right > m_screen.Map.Bounds.Right)
            {
                Position.X = m_screen.Map.Bounds.Right - (Bounds.Width / 2f);
            }
            if (Bounds.Top < m_screen.Map.Bounds.Top)
            {
                Position.Y = m_screen.Map.Bounds.Top + (Bounds.Height / 2f);
            }
            if (Bounds.Bottom > m_screen.Map.Bounds.Bottom)
            {
                Position.Y = m_screen.Map.Bounds.Bottom - (Bounds.Height / 2f);
            }
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(AbsOrigin, 0f)) *
                Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1f);
        }

        public Vector2 ScreenToWorld(Vector2 worldCoords)
        {
            Matrix mat =
                Matrix.CreateTranslation(new Vector3(-AbsOrigin, 0f)) *
                Matrix.CreateTranslation(new Vector3(Position, 0f)) *
                Matrix.CreateRotationZ(-Rotation) *
                Matrix.CreateScale(1f / Zoom, 1f / Zoom, 1f);

            return Vector2.Transform(worldCoords, mat);
        }

        public Vector2 WorldToScreen(Vector2 screenCoords)
        {
            return Vector2.Transform(screenCoords, GetViewMatrix());
        }
    }
}
