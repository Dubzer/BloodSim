using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shop
{
    public class Cursor
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        public Cursor()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
