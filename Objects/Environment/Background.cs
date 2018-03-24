using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BloodSim
{
    public class Background : Unit
    {
        public Texture2D mainMenuTexture;

        public Texture2D particlesTexture;
        public Rectangle particleRectangle;

        public Rectangle particleRectangle2;

        public override void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/background");
            mainMenuTexture = Content.Load<Texture2D>("Textures/UI/background");
            particlesTexture = Content.Load<Texture2D>("Textures/particles");
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
            switch (Game1.gameState)
            {
                case Game1.State.MainMenu:
                    spriteBatch.Draw(mainMenuTexture, boundingBox, Color.White);
                    break;

                default:
                    base.Draw(spriteBatch);
                    break;
            }

            spriteBatch.Draw(particlesTexture, particleRectangle, Color.White);
            spriteBatch.Draw(particlesTexture, particleRectangle2, Color.White);
        }
    }
}
