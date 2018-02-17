using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

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
                spriteBatch.Draw(texture, boundingBox, Color.White);
            }
        }
    }
}
