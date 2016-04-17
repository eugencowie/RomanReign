using Microsoft.Xna.Framework;

namespace RomanReign
{
    class Camera
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin   = Vector2.Zero;
        public float   Rotation = 0f;
        public float   Zoom     = 1f;

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
