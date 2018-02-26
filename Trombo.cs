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

namespace BloodSim
{
    public class Trombo : Cell
    {
        public Rectangle currentTarget;
        public Random random;

        public SoundEffect death_soundEffect;
        bool dead = false;

        public event Action OnDeath;

        public Trombo(Random random)
        {
            texture = null;
            this.random = random;

            position = new Vector2(random.Next(0, Game1.gameWidth / 4), 500);
            currentTarget = new Rectangle(random.Next(0, Game1.gameWidth / 4), Game1.gameHeight + 200, 2, 2);

            OnDeath += Die;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/trombo");
            death_soundEffect = content.Load<SoundEffect>("Sounds/kill");
        }

        public void Die()
        {
            SoundEffect.MasterVolume = 0.5f;
            death_soundEffect.Play();

            dead = true;
        }

        public void Update(GameTime gameTime, List<Wall> wallList)
        {
            if (hp > 0)
            {
                for (int i = 0; i < wallList.Count; i++)
                {
                    if (boundingBox.Intersects(wallList[i].boundingBox) && (wallList[i].hp < 100))
                    {
                        wallList[i].hp++;
                        hp -= 2;
                    }

                    Vector2 dis = new Vector2(wallList[i].position.X, wallList[i].position.Y) - position;
                    float length = (float)Math.Sqrt(dis.X + dis.Y);

                    if ((length < 5000) && (wallList[i].hp < 100))
                    {
                        currentTarget = new Rectangle((int)wallList[i].position.X + wallList[i].boundingBox.Width / 2, (int)wallList[i].position.Y + wallList[i].boundingBox.Height / 2, 3, 3);
                        break;
                    }
                    else
                    {
                        currentTarget = new Rectangle(100, 300, 2, 2);
                        continue;
                    }

                }

                /*foreach (Wall w in wallList)
                {
                    Vector2 dis = new Vector2(w.position.X, w.position.Y) - position;
                    float length = (float)Math.Sqrt(dis.X + dis.Y);

                    if (length < 5000)
                    {
                        currentTarget = w.boundingBox;
                    }

                    if (boundingBox.Intersects(w.boundingBox))
                    {
                        w.hp++;
                    }

                    if (w.hp >= 100)
                    {
                        currentTarget = new Rectangle(1000, 300, 2, 2);
                    }
                }*/

                if (!currentTarget.Intersects(boundingBox))
                {
                    Vector2 Direction = new Vector2(currentTarget.X, currentTarget.Y) - position;
                    Direction.Normalize();
                    position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 400;
                }

                boundingBox = new Rectangle((int)position.X, (int)position.Y, Game1.gameWidth / 10, Game1.gameHeight / 10);
            }
            else
            {
                if (!dead)
                {
                    position = new Vector2(-1000, -1000);
                    OnDeath();
                }
            }
        }
    }
}
