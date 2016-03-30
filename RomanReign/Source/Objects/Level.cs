using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    /// <summary>
    /// This object provides access to all game-related objects such as the map, player, enemies, etc.
    /// </summary>
    class Level : IGameObject
    {
        public Camera Camera;

        public HUD HUD;

        public Level()
        {
            Camera = new Camera();

            HUD = new HUD();
        }

        public void Initialize(ContentManager content)
        {
            Camera.Initialize(content);

            HUD.Initialize(content);
        }

        public void Dispose()
        {
            // TODO: explanation of reverse releasing. It is probably unnecessary in this case.

            HUD.Dispose();

            Camera.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            HUD.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //
            // TODO: explanation of how the camera works.
            //

            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            spriteBatch.End();

            //
            // Now we want to draw the HUD without our coordinates being transformed using the
            // camera, so we need to call spriteBatch.Begin() again, this time without using
            // the camera transformation matrix.
            //

            spriteBatch.Begin();

            HUD.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
