﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BloodSim
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static public Random random = new Random(unchecked((int)(DateTime.Now.Ticks)));

        static public List<Cell> cellList = new List<Cell>();
        static public List<Bacterium> bacteriumList = new List<Bacterium>();
        static public List<Eritro> eritroList = new List<Eritro>();
        static public List<Leiko> leikoList = new List<Leiko>();
        static public List<Trombo> tromboList = new List<Trombo>();
        static public List<Wall> wallList = new List<Wall>();

        //static public Spawner spawner = new Spawner();
        int spawnTimer = 0;

        HUD hud = new HUD();

        static public int oxygenPoints;

        public Eritro er1 = new Eritro(random);
        public Leiko le1 = new Leiko(random);
        public Trombo tr1 = new Trombo(random);

        public Background background = new Background();

        Song music;

        bool musicPlayed = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            //graphics.ToggleFullScreen();

            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            for (int i = 0; i < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 200 + 1; i++)
            {
                wallList.Add(new Wall());
            }

            eritroList.Add(er1);
            cellList.Add(er1);
            tromboList.Add(tr1);

            //tr1.hp = 0;

            leikoList.Add(le1);
            cellList.Add(le1);
            cellList.Add(tr1);

            wallList[3].hp = 0;
            wallList[5].hp = 0;


            //ba2.position = new Vector2(1920,600);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < cellList.Count; i++)
            {
                cellList[i].LoadContent(Content);
            }
            for (int i = 0; i < bacteriumList.Count; i++)
            {
                bacteriumList[i].LoadContent(Content);
            }
            for (int i = 0; i < wallList.Count; i++)
            {
                wallList[i].LoadContent(Content);
            }
            for (int i = 0; i < tromboList.Count; i++)
            {
                tromboList[i].LoadContent(Content);
            }

            background.LoadContent(Content);
            music = Content.Load<Song>("Sounds/music1");

            hud.LoadContent(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            #region Обновление игровых объектов
            for (int i = 0; i < eritroList.Count; i++)
            {
                eritroList[i].Update(gameTime, new Rectangle(100, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + eritroList[i].boundingBox.Height /*- eritroList[i].boundingBox.Height*/, 2, 2), wallList);
            }

            for (int i = 0; i < bacteriumList.Count; i++)
            {
                bacteriumList[i].Update(gameTime, cellList, wallList);
            }

            for (int i = 0; i < leikoList.Count; i++)
            {
                leikoList[i].Update(gameTime, bacteriumList, wallList);
            }

            for (int i = 0; i < tromboList.Count; i++)
            {
                tromboList[i].Update(gameTime, wallList);
            }

            for (int i = 0; i < wallList.Count; i++)
            {
                wallList[i].Update(gameTime);
            }

            if (musicPlayed == false)
            {
                MediaPlayer.Play(music);
                MediaPlayer.Volume = .5f;

                musicPlayed = true;
            }

            hud.Update(gameTime, oxygenPoints);
            background.Update(gameTime);
            #endregion

            #region Очистка от уничтоженных объектов
            ClearAll();
            #endregion

            #region Создание бактерий
            spawnTimer++;
            if(spawnTimer == 500)
            {
                SpawnBacterium(1);
                spawnTimer = 0;
            }
            #endregion


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(14, 9, 32));

            spriteBatch.Begin();
            {
                background.Draw(spriteBatch);
                for (int i = 0; i < cellList.Count; i++)
                {
                    cellList[i].Draw(spriteBatch);
                }
                
                for (int i = 0; i < bacteriumList.Count; i++)
                {
                    bacteriumList[i].Draw(spriteBatch);
                }

                for (int i = 0; i < leikoList.Count; i++)
                {
                    leikoList[i].Draw(spriteBatch);
                }
                for (int i = 0; i < wallList.Count; i++)
                {
                    wallList[i].Draw(spriteBatch);
                }
                for (int i = 0; i < tromboList.Count; i++)
                {
                    tromboList[i].Draw(spriteBatch);
                }

                hud.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region УПРАВЛЕНИЕ ИГРОВЫМ ПРОЦЕССОМ
        protected void SpawnBacterium(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Bacterium bacteriumExample = new Bacterium(random)
                {
                    texture = Content.Load<Texture2D>("Textures/bacterium"),
                    death_soundEffect = Content.Load<SoundEffect>("Sounds/kill"),
                    bite_soundEffect = Content.Load<SoundEffect>("Sounds/bite")
                };
                bacteriumList.Add(bacteriumExample);
            }
        }

        protected void SpawnEritro(int amount)
        {
            Eritro eritroExample = new Eritro(random)
            {
                texture = Content.Load<Texture2D>("Textures/eritro"),
                death_soundEffect = Content.Load<SoundEffect>("Sounds/kill")
            };
            eritroList.Add(eritroExample);
            cellList.Add(eritroExample);
        }

        protected void SpawnLeiko(int amount)
        {
            Leiko leikoExample = new Leiko(random)
            {
                texture = Content.Load<Texture2D>("Textures/leiko"),
                death_soundEffect = Content.Load<SoundEffect>("Sounds/kill"),
                bite_soundEffect = Content.Load<SoundEffect>("Sounds/bite")
            };
            leikoList.Add(leikoExample);
            cellList.Add(leikoExample);
        }

        protected void SpawnTrombo(int amount)
        {

        }

        protected void ClearAll() // Очистка списков с уже уничтоженными клетками для уменьшения нагрузки
        {
            for (int i = 0; i < bacteriumList.Count; i++)
            {
                if (bacteriumList[i].hp <= 0)
                {
                    bacteriumList.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < eritroList.Count; i++)
            {
                if (eritroList[i].hp <= 0)
                {
                    eritroList.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < leikoList.Count; i++)
            {
                if (leikoList[i].hp <= 0)
                {
                    leikoList.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void RestartProgress() // Вернуть состояния всех объектов на исходные значения
        {
            for (int i = 0; i < bacteriumList.Count; i++)
            {
                bacteriumList.RemoveAt(i);
                i--;
            }

            for (int i = 1; i < eritroList.Count; i++) // Ведём счёт с единицы, чтобы, как минимум, один эритроцит продолжил своё мирное существование
            {
                eritroList.RemoveAt(i);
                i--;
            }

            for (int i = 0; i < wallList.Count; i++)
            {
                wallList[i].hp = 100;
            }

            for (int i = 0; i < leikoList.Count; i++)
            {
                leikoList.RemoveAt(i);
                i--;
            }
        }
        #endregion
    }
}
