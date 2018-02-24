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
            texture = null;
            particlesTexture = null;
            boundingBox = new Rectangle(0, 0, Game1.gameWidth, Game1.gameHeight);
            particleRectangle = new Rectangle(0, 0, Game1.gameWidth - Game1.gameWidth / 4, Game1.gameHeight);
            particleRectangle2 = new Rectangle(0, -Game1.gameHeight, Game1.gameWidth - Game1.gameWidth / 4, Game1.gameHeight);
        }

        public void Update(GameTime gameTime)
        {
            particleRectangle.Y += 1;
            particleRectangle2.Y += 1;

            if (particleRectangle.Y == Game1.gameHeight)
            {
                particleRectangle.Y = -Game1.gameHeight;
            }

            if (particleRectangle2.Y == Game1.gameHeight)
            {
                particleRectangle2.Y = -Game1.gameHeight;
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
