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
    public class Wall : Unit
    {
        public bool isVisible;

        public Wall()
        {
            texture = null;

            if (Game1.wallList.Count > 0)
            {
                position = new Vector2(Game1.gameWidth - Game1.gameWidth / 4, Game1.wallList[Game1.wallList.Count - 1].boundingBox.Y + Game1.wallList[Game1.wallList.Count - 1].boundingBox.Height);
                boundingBox = new Rectangle(Game1.gameWidth - Game1.gameWidth / 4, Game1.wallList[Game1.wallList.Count - 1].boundingBox.Y + Game1.wallList[Game1.wallList.Count - 1].boundingBox.Height, 100, 200);
            }
            else
            {
                position = new Vector2(Game1.gameWidth - Game1.gameWidth / 4, 0);
                boundingBox = new Rectangle(Game1.gameWidth - Game1.gameWidth / 4, 0, 100, 200);
            }

            Game1.wallList.Add(this);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/wall");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            boundingBox.X = (int)position.X;
            boundingBox.Y = (int)position.Y;
        }
    }
}
