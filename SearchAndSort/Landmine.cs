using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{
    public class Landmine : Bullet
    {
        public Explosion theExplosion;

        public Landmine()
        {
        }

        public Landmine(Game1 _game, Rectangle _bulletRect, Vector2 _speed, Color _color, int _player, float _rotation,
            Texture2D _rectangleTexture)
        {
            game = _game;
            bulletRect = _bulletRect;
            speed = _speed;
            color = _color;
            player = _player;
            rotation = _rotation;
            rectangleTexture = _rectangleTexture;
            alive = true;
            pointsOnHit = 50;
            pointsOnKill = 200;
        }

        public override void Die()
        {
            base.Die();

            game.scoreManager.addScore(player, 1000);
            theExplosion = new Explosion(new Vector2(bulletRect.X, bulletRect.Y), game, player, Color.Orange);
        }

        public override void Update()
        {
            base.Update();
            if (!alive) theExplosion.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!alive) theExplosion.Draw(spriteBatch);
        }

        public override void CheckCollision()
        {
            foreach (var et in game.enemyTanks)
                if (Rectangle.Intersect(bulletRect,
                        new Rectangle((int) et.location.X - et.tankTexture.Width / 2,
                            (int) et.location.Y - et.tankTexture.Height / 2, et.tankTexture.Width,
                            et.tankTexture.Height)).Width != 0 && et.alive)
                    Die();
            foreach (var tank in game.playerTanks)
                if (tank.player != player && Rectangle.Intersect(bulletRect, tank.tankRect).Width != 0 && tank.alive)
                    Die();
        }
    }
}