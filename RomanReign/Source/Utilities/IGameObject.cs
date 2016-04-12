using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    interface IGameObject
    {
        void LoadContent(ContentManager content);
        void UnloadContent();

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
