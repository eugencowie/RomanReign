using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Camera : IGameObject
    {
        public void LoadContent(ContentManager content)
        {
        }

        public void UnloadContent()
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
