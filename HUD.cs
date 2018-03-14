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
        private SpriteFont fontBold;
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
            fontColor = new Color(255, 255, 255, 180);
            uiElementPosition = new Vector2(85, Game1.gameHeight - 65);
        }

        public void LoadContent(ContentManager Content)
        {
            oxygenBarTexture = Content.Load<Texture2D>("Textures/hpBar");
            oxygenBarCell = Content.Load<Texture2D>("Textures/cell");
            fontBold = Content.Load<SpriteFont>("Fonts/bold14");
            uiElement = Content.Load<Texture2D>("Textures/UI/UIelement");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Oxygen
            spriteBatch.Draw(oxygenBarTexture, oxygenBarRectangle, Color.White);
            spriteBatch.Draw(oxygenBarCell, oxygenBarCellRectangle, Color.White);
            spriteBatch.Draw(uiElement, uiElementRectangle, color);
            spriteBatch.DrawString(fontBold, Shop.money.ToString() + "R", new Vector2(112, Game1.gameHeight - 62), fontColor);

        }

        public void Update(GameTime gameTime, int hp)
        {
            if (!spawned)
            {
                oxygenBarRectangle = new Rectangle(Game1.gameWidth / 2, Game1.gameHeight - Game1.gameHeight / 10, 0, Game1.gameHeight / 10);
                oxygenBarCellRectangle = new Rectangle(Game1.gameWidth / 2 - 200, Game1.gameHeight - Game1.gameHeight / 10, 400, Game1.gameHeight / 10);
                spawned = true;
            }
            uiElementRectangle = new Rectangle((int)uiElementPosition.X, (int)uiElementPosition.Y, 102, 32);
            // Oxygen
            oxygenBarRectangle = new Rectangle(Game1.gameWidth / 2 - 200, Game1.gameHeight - Game1.gameHeight / 10, hp, Game1.gameHeight / 10);
        }
    }
}
