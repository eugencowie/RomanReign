using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RomanReign
{
    class Player
    {
        public Vector2 Position => m_physicsBody.Position;

        RomanReignGame m_game;
        GameScreen m_screen;

        AnimatedSprite m_walkingAnimation;
        RigidBody m_physicsBody;

        public Player(GameScreen screen, RomanReignGame game, ContentManager content)
        {
            m_game = game;
            m_screen = screen;

            m_walkingAnimation = new AnimatedSprite(4, 1, 8, content.Load<Texture2D>("Textures/Game/player_walking")) {
                Position = m_screen.Map.Info.PlayerSpawn,
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_physicsBody = new RigidBody {
                Position = m_screen.Map.Info.PlayerSpawn,
                Size = m_walkingAnimation.Bounds.Size.ToVector2(),
                Origin = new Vector2(0.5f, 0.5f),
                LinearDamping = new Vector2(0.2f, 0f)
            };

            m_game.Physics.AddRigidBody(m_physicsBody);
        }

        public void Update(GameTime gameTime)
        {
            if (m_game.Input.IsKeyJustPressed(Keys.Z))
            {
                m_physicsBody.Velocity.Y -= 800f;
            }

            if (m_game.Input.IsKeyDown(Keys.Left))
            {
                m_physicsBody.Velocity.X -= 175f;
                m_walkingAnimation.Effects = SpriteEffects.FlipHorizontally;
                m_walkingAnimation.Update(gameTime);
            }

            if (m_game.Input.IsKeyDown(Keys.Right))
            {
                m_physicsBody.Velocity.X += 175f;
                m_walkingAnimation.Effects = SpriteEffects.None;
                m_walkingAnimation.Update(gameTime);
            }

            m_walkingAnimation.Position = m_physicsBody.Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_walkingAnimation.Draw(spriteBatch);

            m_game.Debug.Draw(m_physicsBody.Bounds.ToRect(), Color.Blue);
        }
    }
}
