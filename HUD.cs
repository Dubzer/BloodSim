#region File Description
//-----------------------------------------------------------------------------
// HUD.cs
//
// Created by Judex Mars & Dubzer 
//-----------------------------------------------------------------------------
#endregion
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
        private Color oxygenBarColor, oxygenBarCellColor;
        private SpriteFont fontBold, fontBold23;
        private Texture2D uiElement;        //  Хз как это назвать
        private Vector2 uiElementPosition;
        private Rectangle uiElementRectangle;
        private Color color, fontColor;
        public HUD()
        {
            // Oxygen
            oxygenBarTexture = null;
            oxygenBarCell = null;
            color = new Color(0, 0, 0, 100);
            fontColor = new Color(230, 230, 230, 180);
            oxygenBarColor = new Color(255, 255, 255, 180);
            oxygenBarCellColor = new Color(0, 0, 0, 200);
            uiElementPosition = new Vector2(85, Game1.gameHeight - 65);
        }

        public void LoadContent(ContentManager Content)
        {
            oxygenBarTexture = Content.Load<Texture2D>("Textures/UI/background" /*"Textures/hpBar"*/);
            oxygenBarCell = Content.Load<Texture2D>("Textures/UI/background" /*"Textures/cell"*/);
            fontBold = Content.Load<SpriteFont>("Fonts/bold14");
            fontBold23 = Content.Load<SpriteFont>("Fonts/bold23");
            uiElement = Content.Load<Texture2D>("Textures/UI/UIelement");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Oxygen
            spriteBatch.Draw(oxygenBarCell, oxygenBarCellRectangle, oxygenBarCellColor);
            spriteBatch.Draw(oxygenBarTexture, oxygenBarRectangle, oxygenBarColor);
            spriteBatch.Draw(uiElement, uiElementRectangle, color);
            spriteBatch.DrawString(fontBold, Shop.money.ToString() + "R", new Vector2(112, Game1.gameHeight - 62), fontColor);
            spriteBatch.DrawString(fontBold23, "Кислород", new Vector2(oxygenBarCellRectangle.X + oxygenBarCellRectangle.Width / 2 - (fontBold23.MeasureString("Кислород").X / 2), oxygenBarCellRectangle.Y - fontBold23.MeasureString("Кислород").Y), fontColor);

        }

        public void Update(GameTime gameTime, int hp)
        {
            if (!spawned)
            {
                oxygenBarRectangle = new Rectangle(Game1.gameWidth / 2, Game1.gameHeight - oxygenBarRectangle.Height - 25, 0, 25);
                oxygenBarCellRectangle = new Rectangle(Game1.gameWidth / 2 - 200, Game1.gameHeight - oxygenBarRectangle.Height - 25, 400, 25);
                spawned = true;
            }
            uiElementRectangle = new Rectangle((int)uiElementPosition.X, (int)uiElementPosition.Y, 102, 32);
            // Oxygen
            oxygenBarRectangle = new Rectangle(Game1.gameWidth / 2 - 200, Game1.gameHeight - oxygenBarRectangle.Height - 25, hp, 25);
        }
    }
}
