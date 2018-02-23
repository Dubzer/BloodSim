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
using System;

namespace BloodSim
{
    class Shop
    {
        public event Action OnClick0;
        public event Action OnClick1;

        private SpriteFont fontRegular, fontBold;

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
        Card card2 = new Card(new Vector2(0, 500), "item1", 250, "Лейкоцит",
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
            card2.Update(gameTime);
            backgroundRectangle = new Rectangle(0, 0, 360, Game1.gameHeight);
            topRectangle = new Rectangle(0, 0, 360, 90);

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (card0.hasBeenClicked)
            {
                card0.hasBeenClicked = false;
                OnClick0();
            }

            if (card1.hasBeenClicked)
            {
                card1.hasBeenClicked = false;
                OnClick1();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
  
            spriteBatch.Draw(backgound, backgroundRectangle, Color.White);
            card0.Draw(spriteBatch);
            card1.Draw(spriteBatch);
            card2.Draw(spriteBatch);
            spriteBatch.Draw(topTexture, topRectangle, Color.White);
            spriteBatch.DrawString(fontBold, "Магазин", new Vector2(10, 4), Color.Black);
            spriteBatch.DrawString(fontRegular, "У вас: " + money + "$", new Vector2(12, 48), Color.Black);
        }
        public void LoadContent(ContentManager Content)
        {
            backgound = Content.Load<Texture2D>("background");
            topTexture = Content.Load<Texture2D>("top");
            card0.LoadContent(Content);
            card1.LoadContent(Content);
            card2.LoadContent(Content);
            #region Fonts
            fontRegular = Content.Load<SpriteFont>("regular");
            fontBold = Content.Load<SpriteFont>("bold");
            #endregion
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
        }
    }
}
