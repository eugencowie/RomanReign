﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Level : IGameObject
    {
        public HUD HUD;

        public Level()
        {
            HUD = new HUD();
        }

        public void Initialize(ContentManager content)
        {
            HUD.Initialize(content);
        }

        public void Dispose()
        {
            HUD.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            HUD.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            HUD.Draw(gameTime, spriteBatch);
        }
    }
}
