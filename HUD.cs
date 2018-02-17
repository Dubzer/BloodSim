using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BloodSim
{
    public class HUD
    {
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
            oxygenBarRectangle = new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10);
            oxygenBarCellRectangle = new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - 200, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10, 400, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10);
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
            // Oxygen
            oxygenBarRectangle = new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - 200, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10, hp, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10);
        }
    }
}
