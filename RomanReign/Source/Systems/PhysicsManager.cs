using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace RomanReign
{
    enum CollisionResponse { Block, NoBlock }

    /// <summary>
    /// Represents a static immovable body.
    /// </summary>
    class StaticBody
    {
        public string Name;
        public object UserData;

        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Size;

        public RectangleF Bounds => new RectangleF(Position - Origin, Size);

        public void SetRelativeOrigin(Vector2 origin)
        {
            Origin = Size * origin;
        }

        public void SetRelativeOrigin(float originX, float originY)
        {
            SetRelativeOrigin(new Vector2(originX, originY));
        }
    }

    /// <summary>
    /// Represents a dynamic moving body.
    /// </summary>
    class DynamicBody : StaticBody
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 LinearDamping;

        public delegate CollisionResponse OnCollisionDelegate(StaticBody other);
        public event OnCollisionDelegate OnCollision;

        public CollisionResponse HandleCollision(StaticBody other)
        {
            return OnCollision?.Invoke(other) ?? CollisionResponse.Block;
        }
    }

    /// <summary>
    /// Manages a list a physics bodies and applies physics to them.
    /// </summary>
    class PhysicsManager
    {
        List<StaticBody> m_staticBodies;
        List<DynamicBody> m_dynamicBodies;

        Vector2 m_gravity;

        public PhysicsManager()
        {
            m_staticBodies = new List<StaticBody>();
            m_dynamicBodies = new List<DynamicBody>();

            m_gravity = new Vector2(0f, 1500f);
        }

        /// <summary>
        /// Updates all dynamic bodies.
        /// </summary>
        public void Update(float timestep)
        {
            for (int i = m_dynamicBodies.Count - 1; i >= 0; i--)
            {
                m_dynamicBodies[i].Velocity -= m_dynamicBodies[i].Velocity * m_dynamicBodies[i].LinearDamping;

                m_dynamicBodies[i].Velocity += m_gravity * timestep;
                m_dynamicBodies[i].Velocity += m_dynamicBodies[i].Acceleration * timestep;

                Vector2 movement = m_dynamicBodies[i].Velocity * timestep;

                if (!AttemptMoveY(m_dynamicBodies[i], movement.Y))
                {
                    m_dynamicBodies[i].Velocity.Y = 0;
                    m_dynamicBodies[i].Acceleration.Y = 0;
                }

                if (!AttemptMoveX(m_dynamicBodies[i], movement.X))
                {
                    m_dynamicBodies[i].Velocity.X = 0;
                    m_dynamicBodies[i].Acceleration.X = 0;
                }
            }
        }

        public void AddRigidBody(DynamicBody body)
        {
            m_dynamicBodies.Add(body);
        }

        public void AddStaticBody(StaticBody body)
        {
            m_staticBodies.Add(body);
        }

        /// <summary>
        /// Attempts to move a dynamic body on the x axis. Returns true if successful. If unsuccessful,
        /// moves the body as far as it can and then returns false.
        /// </summary>
        private bool AttemptMoveX(DynamicBody body, float x)
        {
            if (x == 0)
            {
                return true;
            }

            Vector2 newPosition = body.Bounds.Location + new Vector2(x, 0f);

            RectangleF newBounds = new RectangleF(newPosition.X, newPosition.Y, body.Size.X, body.Size.Y);
            bool intersects = m_staticBodies.Any(b => b.Bounds.Intersects(newBounds));

            if (!intersects)
            {
                body.Position.X += x;
                return true;
            }
            else
            {
                StaticBody other = m_staticBodies.First(r => r.Bounds.Intersects(newBounds));
                switch (body.HandleCollision(other))
                {
                    case CollisionResponse.Block:
                        if (x > 0)
                        {
                            float diff = newBounds.Right - other.Bounds.Left;
                            body.Position.X += x - diff;
                        }
                        else
                        {
                            float diff = other.Bounds.Right - newBounds.Left;
                            body.Position.X += x + diff;
                        }
                        return false;

                    case CollisionResponse.NoBlock:
                        body.Position.X += x;
                        return true;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Attempts to move a dynamic body on the y axis. Returns true if successful. If unsuccessful,
        /// moves the body as far as it can and then returns false.
        /// </summary>
        private bool AttemptMoveY(DynamicBody body, float y)
        {
            if (y == 0)
            {
                return true;
            }

            Vector2 newPosition = body.Bounds.Location + new Vector2(0f, y);

            RectangleF newBounds = new RectangleF(newPosition.X, newPosition.Y, body.Size.X, body.Size.Y);
            bool intersects = m_staticBodies.Any(b => b.Bounds.Intersects(newBounds));

            if (!intersects)
            {
                // Dirty hack to reset player.OnGround because I'm too lazy to implement
                // a "no collision" event.
                var player = body.UserData as Player;
                if (player != null)
                    player.OnGround = false;

                // Same for enemies.
                var enemy = body.UserData as Enemy;
                if (enemy != null)
                    enemy.OnGround = false;

                body.Position.Y += y;
                return true;
            }
            else
            {
                StaticBody other = m_staticBodies.First(r => r.Bounds.Intersects(newBounds));
                switch (body.HandleCollision(other))
                {
                    case CollisionResponse.Block:
                        if (y > 0)
                        {
                            float diff = newBounds.Bottom - other.Bounds.Top;
                            body.Position.Y += y - diff;
                        }
                        else
                        {
                            float diff = other.Bounds.Bottom - newBounds.Top;
                            body.Position.Y += y + diff;
                        }
                        return false;

                    case CollisionResponse.NoBlock:
                        body.Position.Y += y;
                        return true;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
