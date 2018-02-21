using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BloodSim
{
    public class Background : Unit
    {
        public Texture2D particlesTexture;
        public Rectangle particleRectangle;

        public Rectangle particleRectangle2;

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/background");
            particlesTexture = content.Load<Texture2D>("Textures/particles");
        }

        public Background()
        {
            boundingBox = new Rectangle(0,0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            particleRectangle = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 4, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            particleRectangle2 = new Rectangle(0, -GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 4, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            texture = null;
            particlesTexture = null;
        }

        public void Update(GameTime gameTime)
        {
            particleRectangle.Y += 1;
            particleRectangle2.Y += 1;

            if (particleRectangle.Y == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                particleRectangle.Y = -GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

            if (particleRectangle2.Y == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                particleRectangle2.Y = -GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(particlesTexture, particleRectangle, Color.White);
            spriteBatch.Draw(particlesTexture, particleRectangle2, Color.White);
        }
    }
}
