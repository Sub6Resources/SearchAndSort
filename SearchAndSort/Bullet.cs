using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{
    public class Bullet
    {
        public Rectangle bulletRect;
        public Game1 game;
        public Texture2D rectangleTexture;
        public Vector2 speed;

        public Bullet()
        {
        }

        public Bullet(Game1 _game, Rectangle _bulletRect, Vector2 _speed, Color _color, int _player, float _rotation)
        {
            game = _game;
            bulletRect = _bulletRect;
            speed = _speed;
            color = _color;
            player = _player;
            rotation = _rotation;
            rectangleTexture = game.texture2d;
            alive = true;
            pointsOnHit = 50;
            pointsOnKill = 200;
        }

        public Color color;
        public int player;
        public float rotation;
        public bool alive;
        public int pointsOnHit;
        public int pointsOnKill;

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive) spriteBatch.Draw(rectangleTexture, bulletRect, color);
        }

        public virtual void Update()
        {
            if (alive)
            {
                bulletRect.X += (int) speed.X;
                bulletRect.Y += (int) speed.Y;
                CheckCollision();
            }
        }

        public virtual void CheckCollision()
        {
            foreach (var et in game.enemyTanks)
                if (Rectangle.Intersect(bulletRect,
                        new Rectangle((int) et.location.X - et.tankTexture.Width / 2,
                            (int) et.location.Y - et.tankTexture.Height / 2, et.tankTexture.Width,
                            et.tankTexture.Height)).Width != 0 && et.alive && player != 100)
                {
                    try
                    {
                        et.Hit();
                        game.scoreManager.addScore(player, pointsOnHit);
                        if (!et.alive) game.scoreManager.addScore(player, pointsOnKill);
                        Die();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        //This catches the case where multiple tanks explode at the same time and the shrapnel hits the other exploding tank.
                    }
                }

            foreach (var tank in game.playerTanks)
            {
                if (tank.player != player && Rectangle.Intersect(bulletRect, tank.tankRect).Width != 0 && tank.alive)
                {
                    try
                    {
                        tank.Hit();
                        game.scoreManager.addScore(player, pointsOnHit);
                        if (!tank.alive) game.scoreManager.addScore(player, pointsOnKill);
                        Die();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        //This catches the case where multiple tanks explode at the same time and the shrapnel hits a player.
                    }
                }

                if (player == 100 && Rectangle.Intersect(bulletRect, tank.tankRect).Width != 0 && tank.alive)
                {
                    try
                    {
                        tank.Hit();
                        game.scoreManager.addScore(0, pointsOnHit);
                        if (!tank.alive) game.scoreManager.addScore(0, pointsOnKill);
                        Die();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        //This catches the case where multiple tanks explode at the same time and the shrapnel hits the other exploding tank.
                    }
                }
            }

            foreach (var tiles in game.map.map)
            foreach (var tile in tiles)
                if (Rectangle.Intersect(bulletRect, tile.collisionRect).Width > 0 && tile.type != Tile.AIR)
                    Die();
        }

        public virtual void Die()
        {
            alive = false;
            speed = Vector2.Zero;
            color = Color.Blue;
        }
    }
}