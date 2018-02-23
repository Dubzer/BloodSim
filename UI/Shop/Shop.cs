#region File Description
//-----------------------------------------------------------------------------
// Shop.cs
//
// Created by Dubzer t.me/d_lnk
//-----------------------------------------------------------------------------
#endregion
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BloodSim
{
    class Shop
    {
        private SpriteFont robotoBold, robotoRegular;

        private Texture2D topTexture;
        private Rectangle topRectangle;
        public static int money = 1400;
        MouseState _currentMouseState;
        MouseState _previousMouseState;
        Card card0 = new Card(new Vector2(0, 100), "item0",100 , "Эритроцит",
                                                                          "Эритроциты - клетки крови" + 
                                                                          "\n" + "позвоночных животных " + 
                                                                          "\n" + "(включая человека) и " + 
                                                                          "\n" + "гемолимфы некоторых " +
                                                                          "\n" + "беспозвоночных");
        Card card1 = new Card(new Vector2(0, 300), "item1",200, "Лейкоцит",
                                                                  "Лейкоцит - белые кровяные" +
                                                                  "\n" + "клетки; неоднородная " +
                                                                  "\n" + "группа различных по  " +
                                                                  "\n" + "внешнему виду и " +
                                                                  "\n" + "функциям клеток " +
                                                                  "\n" + "крови человека");
        Texture2D backgound = null;
        private Rectangle backgroundRectangle;

        public Shop()
        {
            Debug.Print("12");
        }
        public void Update(GameTime gameTime)
        {
            card0.Update(gameTime);
            card1.Update(gameTime);
            backgroundRectangle = new Rectangle(0, 0, 360, Game1.gameHeight);
            topRectangle = new Rectangle(0, 0, 360, 90);

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();


        }
        public void Draw(SpriteBatch spriteBatch)
        {
  
            spriteBatch.Draw(backgound, backgroundRectangle, Color.White);
            card0.Draw(spriteBatch);
            card1.Draw(spriteBatch);
            spriteBatch.Draw(topTexture, topRectangle, Color.White);
            spriteBatch.DrawString(robotoBold, "Магазин", new Vector2(10, 4), Color.Black);
            spriteBatch.DrawString(robotoRegular, "У вас: " + money + "$", new Vector2(12, 48), Color.Black);
        }
        public void LoadContent(ContentManager Content)
        {
            backgound = Content.Load<Texture2D>("background");
            topTexture = Content.Load<Texture2D>("top");
            card0.LoadContent(Content);
            card1.LoadContent(Content);
            robotoBold = Content.Load<SpriteFont>("robotoBold");
            robotoRegular = Content.Load<SpriteFont>("robotoRegular");
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
        }
    }
}
