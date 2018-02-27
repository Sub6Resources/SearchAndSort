using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{
    public class ParticleSpray
    {
        public static int MAX_PARTICLES = 15;
        private readonly Color color;
        private readonly int num = 15;
        private readonly Particle[] particles = new Particle[MAX_PARTICLES];

        public ParticleSpray()
        {
        }

        public ParticleSpray(Vector2 location, Game1 game, int player, Texture2D whiteRectangle, Color _color,
            int maxSpeed)
        {
            color = _color;
            var randy = new Random();
            for (var i = 0; i < MAX_PARTICLES; ++i)
            {
                var speed = new Vector2();
                var a = randy.Next(-maxSpeed, maxSpeed);
                var b = randy.Next(-maxSpeed, maxSpeed);
                /*if (a == 0 || a == 1 || a == 2)
                {
                    a = 3;
                }
                if (b == 0 || b == 1 || b == 2)
                {
                    b = 3;
                }*/
                speed = new Vector2(a, b);
                /*int r = randy.Next(0, 456);
                int g = randy.Next(0, 456);
                int c = randy.Next(0, 456);
                color = Color.FromNonPremultiplied(255, r, g, c);*/
                //Create a new shrapnel bullet
                particles[i] = new Particle(game,
                    new Rectangle(new Point((int) location.X, (int) location.Y),
                        new Point(randy.Next(1, 20), randy.Next(1, 20))), speed, color, player,
                    (float) randy.NextDouble(), whiteRectangle, 1, true);
            }
        }

        public ParticleSpray(Vector2 location, Game1 game, int player, Texture2D whiteRectangle, Color _color,
            int maxSpeed, int _num)
        {
            num = _num;
            particles = new Particle[num];
            color = _color;
            var randy = new Random();
            for (var i = 0; i < num; ++i)
            {
                var speed = new Vector2();
                var a = randy.Next(-maxSpeed, maxSpeed);
                var b = randy.Next(-maxSpeed, maxSpeed);
                /*if (a == 0 || a == 1 || a == 2)
                {
                    a = 3;
                }
                if (b == 0 || b == 1 || b == 2)
                {
                    b = 3;
                }*/
                speed = new Vector2(a, b);
                /*int r = randy.Next(0, 456);
                int g = randy.Next(0, 456);
                int c = randy.Next(0, 456);
                color = Color.FromNonPremultiplied(255, r, g, c);*/
                //Create a new shrapnel bullet
                particles[i] = new Particle(game,
                    new Rectangle(new Point((int) location.X, (int) location.Y),
                        new Point(randy.Next(1, 20), randy.Next(1, 20))), speed, color, player,
                    (float) randy.NextDouble(), whiteRectangle, 1, true);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < num; ++i) particles[i].Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < num; ++i) particles[i].Update(gameTime);
        }
    }
}