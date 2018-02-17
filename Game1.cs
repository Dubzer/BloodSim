using System;
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
        static public List<Wall> wallList = new List<Wall>();

        //static public Spawner spawner = new Spawner();

        HUD hud = new HUD();

        static public int oxygenPoints;

        public Cell er1 = new Eritro(random);

        public Cell le1 = new Leiko(random);
        public Background background = new Background();

        Song music;

        bool musicPlayed = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.ToggleFullScreen();

            for (int i = 0; i < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 200 + 1; i++)
            {
                wallList.Add(new Wall());
            }

            //ba2.position = new Vector2(1920,600);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
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

            background.LoadContent(Content);
            music = Content.Load<Song>("Sounds/music1");

            hud.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for (int i = 0; i < eritroList.Count; i++)
            {
                eritroList[i].Update(gameTime, new Rectangle(100, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + eritroList[i].boundingBox.Height /*- eritroList[i].boundingBox.Height*/, 2, 2), wallList);
            }

            for (int i = 0; i < bacteriumList.Count; i++)
            {
                bacteriumList[i].Update(gameTime, cellList);
            }

            for (int i = 0; i < leikoList.Count; i++)
            {
                leikoList[i].Update(gameTime, bacteriumList, wallList);
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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

                hud.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // УПРАВЛЕНИЕ ИГРОВЫМ ПРОЦЕССОМ //
        protected void SpawnBacterium(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Bacterium bacteriumExample = new Bacterium();
                bacteriumExample.position =
                    new Vector2(random.Next(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 4,
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - bacteriumExample.boundingBox.Width),
                    random.Next(0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));

                bacteriumList.Add(bacteriumExample);
            }
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
    }
}
