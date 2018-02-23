using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace BloodSim
{
    public class Eritro : Cell
    {
        public Rectangle currentTarget;
        public Random random;
        public bool oxygenMining = true;

        public SoundEffect death_soundEffect;
        bool dead = false;

        public event Action OnDeath;

        public Eritro(Random random)
        {
            texture = null;
            this.random = random;
            position = new Vector2(random.Next(0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 4), -100);
            currentTarget = new Rectangle(100, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + 100, 2, 2);

            OnDeath += Die;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/eritro");
            death_soundEffect = content.Load<SoundEffect>("Sounds/kill");
        }

        public void Update(GameTime gameTime, Rectangle currentTarget, List<Wall> list)
        {
            if (hp > 0)
            {
                oxygenMining = !IsWallBroken(list);

                if (!oxygenMining)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].hp <= 0)
                        {
                            currentTarget = new Rectangle(list[i].boundingBox.X + 50, list[i].boundingBox.Y, list[i].boundingBox.Width, list[i].boundingBox.Height);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                if (position.Y >= currentTarget.Y && oxygenMining)
                {
                    position.Y = -boundingBox.Height;
                    position.X = random.Next(0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2);
                    Game1.oxygenPoints += 10;
                }
                
                if(boundingBox.Intersects(currentTarget) && oxygenMining == false)
                {
                    hp -= 20;
                }

                Vector2 Direction = new Vector2(currentTarget.X, currentTarget.Y) - position;
                Direction.Normalize();
                position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 200;

                boundingBox = new Rectangle((int)position.X, (int)position.Y, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 10, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 15);
            }
            else if (!dead)
            {
                position = new Vector2(-1000, -1000);
                OnDeath();
            }
            
        }

        public void Die()
        {
            SoundEffect.MasterVolume = 0.5f;
            death_soundEffect.Play();

            dead = true;
        }

        public bool IsWallBroken(List<Wall> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].hp >= 100)
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
