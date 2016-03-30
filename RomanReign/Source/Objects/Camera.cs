using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Camera : IGameObject
    {
        Level m_level => GameScreen.Level;

        public void Initialize(ContentManager content)
        {
        }

        public void Dispose()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public Matrix GetViewMatrix()
        {
            // TODO
            return Matrix.Identity;
        }
    }
}
