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
                if (boundingBox.Intersects(currentTarget) && oxygenMining)
                {
                    position.Y = -boundingBox.Height;
                    position.X = random.Next(0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2);
                    Game1.oxygenPoints += 10;
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
    }
}
