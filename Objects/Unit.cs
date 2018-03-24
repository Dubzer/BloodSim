using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BloodSim
{
    public class Unit
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle boundingBox;

        public float hp = 100;

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(hp > 0)
            {
                if(texture != null)
                {
                    spriteBatch.Draw(texture, boundingBox, Color.White);
                }
            }
        }
    }
}