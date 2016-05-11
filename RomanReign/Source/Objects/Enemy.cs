using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomanReign
{
    class Enemy
    {
        public Vector2 Position => m_physicsBody.Position;

        public bool IsJumping => m_isJumping;
        public bool IsDropping => m_isDropping;

        public bool OnGround
        {
            get { return m_onGround; }
            set { m_onGround = value; }
        }

        RomanReignGame m_game;
        GameScreen m_screen;

        AnimatedSprite m_walkingAnimation;
        AnimatedSprite m_attackAnimation;

        DynamicBody m_physicsBody;

        List<InputAction> m_jumpActions = new List<InputAction>();
        List<InputAction> m_dropActions = new List<InputAction>();
        List<InputAction> m_moveLeftActions = new List<InputAction>();
        List<InputAction> m_moveRightActions = new List<InputAction>();
        List<InputAction> m_attackActions = new List<InputAction>();

        const float m_attackCooldown = 0.5f;
        float m_timeSinceAttack = m_attackCooldown;

        bool m_triggerJump;
        bool m_triggerDrop;

        bool m_isJumping;
        bool m_isDropping;

        bool m_isAttacking;

        bool m_onGround;

        public Enemy(GameScreen screen, RomanReignGame game, ContentManager content, Property<Vector2> spawnPoint)
        {
            m_game = game;
            m_screen = screen;

            // Load walking animation.

            m_walkingAnimation = new AnimatedSprite(4, 1, 8, true, content.Load<Texture2D>("Textures/Game/enemy_walking")) {
                Position = spawnPoint.Value
            };
            m_walkingAnimation.SetRelativeOrigin(0.5f, 0.5f);

            // Load attack animation.

            m_attackAnimation = new AnimatedSprite(4, 1, 8, false, content.Load<Texture2D>("Textures/Game/enemy_attack")) {
                Position = spawnPoint.Value
            };
            m_attackAnimation.SetRelativeOrigin(0.5f, 0.5f);

            // Create physics body.

            m_physicsBody = new DynamicBody {
                Name = spawnPoint.Name,
                Position = spawnPoint.Value,
                Size = m_walkingAnimation.Bounds.Size.ToVector2(),
                LinearDamping = new Vector2(0.2f, 0f),
                UserData = this
            };
            m_physicsBody.SetRelativeOrigin(0.5f, 0.5f);
            m_physicsBody.OnCollision += other => {
                if (other.Name == "wall" && (IsJumping || IsDropping))
                    return CollisionResponse.NoBlock;
                if (other.Name == "ground" && !IsJumping && !IsDropping)
                    m_onGround = true;
                return CollisionResponse.Block;
            };

            m_game.Physics.AddRigidBody(m_physicsBody);

            // AI

            m_jumpActions.Add(() =>
                m_screen.Player.Position.Y < m_physicsBody.Position.Y - 50 &&
                m_screen.Player.OnGround &&
                Math.Abs(m_physicsBody.Velocity.Y) < 0.001f);

            m_dropActions.Add(() =>
                m_screen.Player.Position.Y > m_physicsBody.Position.Y + 50 &&
                m_screen.Player.OnGround &&
                Math.Abs(m_physicsBody.Velocity.Y) < 0.001f);

            m_moveRightActions.Add(() =>
                m_screen.Player.Position.X > m_physicsBody.Position.X + 80);

            m_moveLeftActions.Add(() =>
                m_screen.Player.Position.X < m_physicsBody.Position.X - 80);

            m_attackActions.Add(() =>
                m_timeSinceAttack > m_attackCooldown &&
                Math.Abs((m_screen.Player.Position - m_physicsBody.Position).Length()) < 80);
        }

        public void Update(GameTime gameTime)
        {
            if (m_jumpActions.Any(a => a()))
            {
                if (!m_isJumping)
                    m_triggerJump = true;
            }
            if (m_physicsBody.Velocity.Y > 0)
            {
                m_isJumping = false;
            }

            if (m_dropActions.Any(a => a()))
            {
                if (!m_isDropping)
                    m_triggerDrop = true;
            }
            if (m_physicsBody.Velocity.Y < 0)
            {
                m_isDropping = false;
            }

            if (m_triggerJump)
            {
                m_physicsBody.Velocity.Y -= 800f;
                m_triggerJump = false;
                m_isJumping = true;
            }

            if (m_triggerDrop)
            {
                m_triggerDrop = false;
                m_isDropping = true;
            }

            if (m_moveLeftActions.Any(a => a()))
            {
                m_physicsBody.Velocity.X -= 75f;
                m_walkingAnimation.Effects = SpriteEffects.FlipHorizontally;
                m_attackAnimation.Effects = SpriteEffects.FlipHorizontally;
                m_walkingAnimation.Update(gameTime);
            }

            if (m_moveRightActions.Any(a => a()))
            {
                m_physicsBody.Velocity.X += 75f;
                m_walkingAnimation.Effects = SpriteEffects.None;
                m_attackAnimation.Effects = SpriteEffects.None;
                m_walkingAnimation.Update(gameTime);
            }

            if (m_attackActions.Any(a => a()))
            {
                m_isAttacking = true;
            }

            m_walkingAnimation.Position = m_physicsBody.Position;
            m_attackAnimation.Position = m_physicsBody.Position;

            m_walkingAnimation.Visible = !m_isAttacking;
            m_attackAnimation.Visible = m_isAttacking;

            if (m_isAttacking)
            {
                m_attackAnimation.Update(gameTime);

                if (m_attackAnimation.IsFinished())
                {
                    m_attackAnimation.Reset();
                    m_isAttacking = false;
                    m_timeSinceAttack = 0;
                }
            }

            m_timeSinceAttack += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_walkingAnimation.Draw(spriteBatch);
            m_attackAnimation.Draw(spriteBatch);

            m_game.Debug.Draw(m_physicsBody.Bounds.ToRect(), Color.Blue);
        }
    }
}
