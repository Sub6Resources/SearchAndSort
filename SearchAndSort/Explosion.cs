using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{
    public class Explosion
    {
        public static int MAX_PROJECTILES = 60;
        private readonly Color color;
        private readonly Bullet[] shrapnel = new Bullet[MAX_PROJECTILES];

        public Explosion()
        {
        }

        public Explosion(Vector2 location, Game1 game, int player, Color _color)
        {
            color = _color;
            var randy = new Random();
            for (var i = 0; i < MAX_PROJECTILES; ++i)
            {
                var speed = new Vector2();
                var a = randy.Next(-20, 20);
                var b = randy.Next(-20, 20);
                if (a == 0 || a == 1 || a == 2) a = 3;
                if (b == 0 || b == 1 || b == 2) b = 3;
                speed = new Vector2(a, b);
                var r = randy.Next(0, 456);
                var g = randy.Next(0, 456);
                var c = randy.Next(0, 456);
                color = Color.FromNonPremultiplied(255, r, g, c);
                //Create a new shrapnel bullet
                shrapnel[i] = new Bullet(game,
                    new Rectangle(new Point((int) location.X, (int) location.Y),
                        new Point(randy.Next(1, 20), randy.Next(1, 20))), speed, color, player,
                    randy.Next(-(int) MathHelper.Pi, (int) MathHelper.Pi));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < MAX_PROJECTILES; ++i) shrapnel[i].Draw(spriteBatch);
        }

        public void Update()
        {
            for (var i = 0; i < MAX_PROJECTILES; ++i) shrapnel[i].Update();
        }
    }
}