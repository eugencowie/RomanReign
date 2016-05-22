using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    class Pickup
    {
        public RectangleF Bounds => m_physicsBody.Bounds;

        GameScreen m_screen;

        Sprite m_pickupSprite;
        DynamicBody m_physicsBody;

        public Pickup(GameScreen screens, ContentManager content, Vector2 position)
        {
            m_screen = screens;

            m_pickupSprite = new Sprite(content.Load<Texture2D>("Textures/HUD/heart")) {
                Position = position
            };
            m_pickupSprite.SetRelativeOrigin(0.5f, 0.5f);
            m_pickupSprite.SetRelativeScale(0.5f, 0.5f);

            m_physicsBody = new DynamicBody {
                Position = position,
                Size = m_pickupSprite.Size,
                Origin = m_pickupSprite.Origin
            };
            m_screen.Physics.AddDynamicBody(m_physicsBody);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_pickupSprite.Position = m_physicsBody.Position;
            m_pickupSprite.Draw(spriteBatch);
        }
    }
}
