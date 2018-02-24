using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BloodSim
{
    public class HUD
    {
        public bool spawned = false;

        // Oxygen
        public Texture2D oxygenBarTexture;
        public Rectangle oxygenBarRectangle;
        public Texture2D oxygenBarCell;
        public Rectangle oxygenBarCellRectangle;

        public HUD()
        {
            // Oxygen
            oxygenBarTexture = null;
            oxygenBarCell = null;
        }

        public void LoadContent(ContentManager content)
        {
            oxygenBarTexture = content.Load<Texture2D>("Textures/hpBar");
            oxygenBarCell = content.Load<Texture2D>("Textures/cell");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Oxygen
            spriteBatch.Draw(oxygenBarTexture, oxygenBarRectangle, Color.White);
            spriteBatch.Draw(oxygenBarCell, oxygenBarCellRectangle, Color.White);
        }

        public void Update(GameTime gameTime, int hp)
        {
            if (!spawned)
            {
                oxygenBarRectangle = new Rectangle(Game1.gameWidth / 2, Game1.gameHeight - Game1.gameHeight / 10, 0, Game1.gameHeight / 10);
                oxygenBarCellRectangle = new Rectangle(Game1.gameWidth / 2 - 200, Game1.gameHeight - Game1.gameHeight / 10, 400, Game1.gameHeight / 10);
                spawned = true;
            }

            // Oxygen
            oxygenBarRectangle = new Rectangle(Game1.gameWidth / 2 - 200, Game1.gameHeight - Game1.gameHeight / 10, hp, Game1.gameHeight / 10);
        }
    }
}
