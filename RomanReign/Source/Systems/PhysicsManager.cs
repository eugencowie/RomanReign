using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace RomanReign
{
    class StaticBody
    {
        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Size;

        Vector2 m_topLeft => Position - (Size * Origin);
        public RectangleF Bounds => new RectangleF(m_topLeft.X, m_topLeft.Y, Size.X, Size.Y);
    }

    class RigidBody : StaticBody
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 LinearDamping;
    }

    class PhysicsManager
    {
        List<StaticBody> m_staticBodies;
        List<RigidBody> m_rigidBodies;

        Vector2 m_gravity;

        public PhysicsManager()
        {
            m_staticBodies = new List<StaticBody>();
            m_rigidBodies = new List<RigidBody>();

            m_gravity = new Vector2(0f, 1500f);
        }

        public void Update(float timestep)
        {
            for (int i = m_rigidBodies.Count - 1; i >= 0; i--)
            {
                m_rigidBodies[i].Velocity -= m_rigidBodies[i].Velocity * m_rigidBodies[i].LinearDamping;

                m_rigidBodies[i].Velocity += m_gravity * timestep;
                m_rigidBodies[i].Velocity += m_rigidBodies[i].Acceleration * timestep;

                Vector2 movement = m_rigidBodies[i].Velocity * timestep;

                if (!AttemptMoveY(m_rigidBodies[i], movement.Y))
                {
                    m_rigidBodies[i].Velocity.Y = 0;
                    m_rigidBodies[i].Acceleration.Y = 0;
                }

                if (!AttemptMoveX(m_rigidBodies[i], movement.X))
                {
                    m_rigidBodies[i].Velocity.X = 0;
                    m_rigidBodies[i].Acceleration.X = 0;
                }
            }
        }

        public void AddRigidBody(RigidBody body)
        {
            m_rigidBodies.Add(body);
        }

        public void AddStaticBody(StaticBody body)
        {
            m_staticBodies.Add(body);
        }

        private bool AttemptMoveX(RigidBody body, float x)
        {
            if (x == 0)
                return true;

            Vector2 newPosition = body.Bounds.Position + new Vector2(x, 0f);

            RectangleF newBounds = new RectangleF(newPosition.X, newPosition.Y, body.Size.X, body.Size.Y);
            bool intersects = m_staticBodies.Any(b => b.Bounds.Intersects(newBounds));

            if (!intersects)
            {
                body.Position.X += x;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AttemptMoveY(RigidBody body, float y)
        {
            if (y == 0)
                return true;

            Vector2 newPosition = body.Bounds.Position + new Vector2(0f, y);

            RectangleF newBounds = new RectangleF(newPosition.X, newPosition.Y, body.Size.X, body.Size.Y);
            bool intersects = m_staticBodies.Any(b => b.Bounds.Intersects(newBounds));

            if (!intersects)
            {
                body.Position.Y += y;
                return true;
            }
            else
            {
                StaticBody other = m_staticBodies.Where(r => r.Bounds.Intersects(newBounds)).First();
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
            }
        }
    }
}
