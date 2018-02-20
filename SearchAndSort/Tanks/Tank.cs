using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
    public class Tank
    {
        public const float UP = -MathHelper.PiOver2;
        public const float UP_RIGHT = -MathHelper.PiOver4;
        public const float RIGHT = 0;
        public const float DOWN_RIGHT = MathHelper.PiOver4;
        public const float DOWN = MathHelper.PiOver2;
        public const float DOWN_LEFT = MathHelper.Pi - MathHelper.PiOver4;
        public const float LEFT = MathHelper.Pi;
        public const float UP_LEFT = -(MathHelper.Pi - MathHelper.PiOver4);

        private const float FIRE_DELAY = 0.5f;
        private const float EXPLOSION_DELAY = 5f;
        private const float BACK_ALIVE_DELAY = 2f;
        private const float MINE_DELAY = 20f;
        public bool alive = true;
        public bool colliding;
        public ParticleSpray deathParticles;
        public bool enemy = false;
        public Explosion explosion;
        public float explosionDelay = 0f;
        public float fireDelay = 0f;
        public Game1 game;
        public ParticleSpray hitParticles;

        public int lives;

        //data members
        public Vector2 location;
        public float mineDelay = 0f;
        public Vector2 origin;
        public int player;
        public float respawnDelay = 0f;
        public ParticleSpray respawnParticles;
        public float rotation;
        public float scale;
        public Vector2 speed;
        public Vector2 startingLocation;
        public Rectangle tankRect;
        public Texture2D tankTexture;

        public Color color;
        
        //generic constructor
        public Tank()
        {
        }

        //overloaded constructor(s)
        public Tank(Game1 _game, Texture2D _tankTexture, Vector2 _location, Vector2 _speed, float _rotation,
            int _player, Color _color)
        {
            tankTexture = _tankTexture;
            location = _location;
            startingLocation = _location;
            speed = _speed;
            rotation = _rotation;
            origin = new Vector2(tankTexture.Width / 2f, tankTexture.Height / 2f);
            game = _game;
            player = _player;
            scale = 1f;
            alive = true;
            lives = 3;
            color = _color;
            respawnParticles = new ParticleSpray(location, game, player, tankTexture, Color.Firebrick, 0);
            deathParticles = new ParticleSpray(location, game, player, tankTexture, Color.Green, 0);
            hitParticles = new ParticleSpray(location, game, player, tankTexture, Color.Silver, 0);
            tankRect = new Rectangle((int) location.X - tankTexture.Width / 2,
                (int) location.Y - tankTexture.Height / 2, tankTexture.Width, tankTexture.Height);
        }

        public Tank(Game1 _game, Texture2D _tankTexture, Vector2 _location, Vector2 _speed, float _rotation,
            int _player, Color _color, int _lives)
        {
            tankTexture = _tankTexture;
            location = _location;
            startingLocation = _location;
            speed = _speed;
            rotation = _rotation;
            origin = new Vector2(tankTexture.Width / 2f, tankTexture.Height / 2f);
            game = _game;
            player = _player;
            scale = 1f;
            alive = true;
            lives = _lives;
            color = _color;
            respawnParticles = new ParticleSpray(location, game, player, tankTexture, Color.Firebrick, 0);
            deathParticles = new ParticleSpray(location, game, player, tankTexture, Color.Green, 0);
            hitParticles = new ParticleSpray(location, game, player, tankTexture, Color.Silver, 0);
            tankRect = new Rectangle((int) location.X - tankTexture.Width / 2,
                (int) location.Y - tankTexture.Height / 2, tankTexture.Width, tankTexture.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(tankTexture, location, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 1);
            }
            else
            {
                if (explosion != null) explosion.Draw(spriteBatch);
            }

            respawnParticles.Draw(spriteBatch);
            deathParticles.Draw(spriteBatch);
            if (hitParticles != null) hitParticles.Draw(spriteBatch);
        }

        public virtual void Update(KeyboardState state, GameTime gameTime)
        {
            if (alive)
            {
                Move(state);
                Move(state, gameTime);
                updateRectangleLocation();
                //Check collisions
                colliding = false;

                foreach (Tank tank in game.enemyTanks)
                {
                    var collision = tank.isColliding(tankRect);
                    if (collision.depth > 0) cancelOutCollisionOverlap(collision);
                }

                foreach (var tank in game.playerTanks)
                {
                    var collision = tank.isColliding(tankRect);
                    if (tank != this && collision.depth > 0) cancelOutCollisionOverlap(collision);
                }

                foreach (var tiles in game.map.map)
                foreach (var tile in tiles)
                {
                    var collision = tile.isColliding(tankRect);
                    if (collision.depth > 0) //If collision is not an empty collision
                        cancelOutCollisionOverlap(collision);
                }
            }
            else //if not alive
            {
                if (explosion != null) explosion.Update();
            }

            respawnParticles.Update(gameTime);
            deathParticles.Update(gameTime);
            if (hitParticles != null) hitParticles.Update(gameTime);
        }

        private void cancelOutCollisionOverlap(Collision collision)
        {
            colliding = true;
            switch (collision.side)
            {
                case Collision.Side.TOP:
                    location.Y += collision.depth;
                    break;
                case Collision.Side.BOTTOM:
                    location.Y -= collision.depth;
                    break;
                case Collision.Side.LEFT:
                    location.X += collision.depth;
                    break;
                case Collision.Side.RIGHT:
                    location.X -= collision.depth;
                    break;
            }

            updateRectangleLocation();
        }

        private void updateRectangleLocation()
        {
            tankRect = new Rectangle((int) location.X - tankTexture.Width / 2,
                (int) location.Y - tankTexture.Height / 2, tankTexture.Width, tankTexture.Height);
        }

        public virtual void Move(KeyboardState state)
        {
        }

        public virtual void Move(KeyboardState state, GameTime gameTime)
        {
        }

        public void MoveLeft(bool isBoostPressed, bool isReversedPressed)
        {
            if (isBoostPressed)
                location.X -= 2 + speed.X;
            else if (isReversedPressed)
                location.X += speed.Y;
            else
                location.X -= speed.X;
        }

        public void MoveRight(bool isBoostPressed, bool isReversedPressed)
        {
            if (isBoostPressed)
                location.X += 2 + speed.X;
            else if (isReversedPressed)
                location.X -= speed.X;
            else
                location.X += speed.X;
        }

        public void MoveUp(bool isBoostPressed, bool isReversedPressed)
        {
            if (isBoostPressed)
                location.Y -= 2 + speed.Y;
            else if (isReversedPressed)
                location.Y += speed.Y;
            else
                location.Y -= speed.Y;
        }

        public void MoveDown(bool isBoostPressed, bool isReversedPressed)
        {
            if (isBoostPressed)
                location.Y += 2 + speed.Y;
            else if (isReversedPressed)
                location.Y -= speed.Y;
            else
                location.Y += speed.Y;
        }

        public void Rotate(float angle)
        {
            while (angle > MathHelper.TwoPi) angle -= MathHelper.TwoPi;
            while (angle < 0) angle += MathHelper.TwoPi;
            rotation = angle;
        }

        public float AimAt(Vector2 target)
        {
            Vector2 targetVector = location - target;
            if (target.Y > location.Y)
            {
                return (float) Math.Atan2(-targetVector.Y, -targetVector.X);
            }
            else
            {
                return (float) (Math.Atan2(targetVector.Y, targetVector.X) + Math.PI);
            }
            
        }

        public void SlowlyRotate(float targetRotation, GameTime gameTime)
        {
            Rotate(MathHelper.Lerp(rotation, targetRotation, (float) gameTime.ElapsedGameTime.TotalSeconds * 4));
        }

        public Bullet Fire()
        {
            if (alive)
            {
                game.sound.PlaySound(Sound.Sounds.LASERSHOOT);
                return new Bullet(game, new Rectangle((int) location.X, (int) location.Y, 5, 5),
                    new Vector2((float) (15 * Math.Cos(rotation)), (float) (15 * Math.Sin(rotation))), Color.Red,
                    player, rotation);
            }

            return null;
        }

        public virtual void Hit()
        {
            lives -= 1;
            if (lives < 1)
                Die();
            else
                hitParticles = new ParticleSpray(location, game, player, tankTexture, Color.Firebrick, 2, 5);
        }

        public virtual void Die()
        {
            if (alive)
            {
                deathParticles = new ParticleSpray(location, game, player, tankTexture, Color.Green, 2);
                alive = false;
                location = new Vector2(-100, -100);
                updateRectangleLocation();
            }
        }

        public void Respawn(Vector2 _location)
        {
            if (!alive)
            {
                location = _location;
                lives = 3;
                respawnParticles = new ParticleSpray(location, game, player, tankTexture, Color.Blue, 2);
                alive = true;
            }
        }

        public void Explode()
        {
            if (alive)
            {
                explosion = new Explosion(location, game, player, Color.Firebrick);
                game.sound.PlaySound(Sound.Sounds.EXPLOSION);
                Die();
            }
        }

        public int SearchAndDestroy(ref List<EnemyTank> tanks)
        {
            //Unordered List Sort
            var min = 100;
            var mindex = -1;
            for (var i = 0; i < tanks.Count; i++)
                if (tanks[i].strength <= min && tanks[i].alive)
                {
                    min = tanks[i].strength;
                    mindex = i;
                }

            return mindex;
        }

        public Collision isColliding(Rectangle possibleCollisionRect)
        {
            var intersect = Rectangle.Intersect(possibleCollisionRect, tankRect);
            if (intersect.Width > 0 || intersect.Height > 0)
            {
                if (possibleCollisionRect.Top < tankRect.Bottom &&
                    Math.Abs(intersect.Width) > Math.Abs(intersect.Height) && possibleCollisionRect.Y > tankRect.Y)
                {
                    float depth = intersect.Height;
                    return new Collision(Collision.Side.TOP, depth);
                }

                if (possibleCollisionRect.Bottom > tankRect.Top &&
                    Math.Abs(intersect.Width) > Math.Abs(intersect.Height))
                {
                    float depth = intersect.Height;
                    return new Collision(Collision.Side.BOTTOM, depth);
                }

                if (possibleCollisionRect.Left < tankRect.Right &&
                    Math.Abs(intersect.Width) < Math.Abs(intersect.Height) &&
                    possibleCollisionRect.Right > tankRect.Right)
                {
                    float depth = intersect.Width;
                    return new Collision(Collision.Side.LEFT, depth);
                }

                if (possibleCollisionRect.Right > tankRect.Right - tankRect.Width &&
                    possibleCollisionRect.Right > tankRect.Left &&
                    Math.Abs(intersect.Width) < Math.Abs(intersect.Height))
                {
                    float depth = intersect.Width;
                    return new Collision(Collision.Side.RIGHT, depth);
                }
            }

            return new Collision();
        }
    }
}