using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace SearchAndSort
{
    public class Tank
    {
        //data members
        public Vector2 location;
        public Vector2 startingLocation;
		public Vector2 speed;
        public float rotation { get; set; }
        public Texture2D tankTexture { get; set; }
        public Vector2 origin { get; set; }
        public Game1 game { get; set; }
        public int player { get; set; }
        public int lives { get; set; }
        public float scale { get; set; }
        public Keys keyUp;
        public Keys keyLeft;
        public Keys keyDown;
        public Keys keyRight;
        public Keys keyBoost;
        public Keys keyReverse;
        public Keys fireKey;
        public Keys explodeKey;
        public Keys mineKey;
        public bool alive;
        public Rectangle tankRect;
        public ParticleSpray deathParticles;
        public ParticleSpray respawnParticles;
        public ParticleSpray hitParticles;
        public const float UP = -MathHelper.PiOver2;
        public const float UP_RIGHT = -MathHelper.PiOver4;
        public const float RIGHT = 0;
        public const float DOWN_RIGHT = MathHelper.PiOver4;
        public const float DOWN = MathHelper.PiOver2;
        public const float DOWN_LEFT = MathHelper.Pi - MathHelper.PiOver4;
        public const float LEFT = MathHelper.Pi;
        public const float UP_LEFT = -(MathHelper.Pi - MathHelper.PiOver4);
		public bool colliding = false;
        public Texture2D whiteRectangle;
		public bool enemy = false;
		public Explosion explosion;
        public float fireDelay = 0f;
        public float explosionDelay = 0f;
        public float respawnDelay = 0f;
        public float mineDelay = 0f;

        private const float FIRE_DELAY = 0.5f;
        private const float EXPLOSION_DELAY = 5f;
        private const float BACK_ALIVE_DELAY = 2f;
        private const float MINE_DELAY = 20f;

        //generic constructor
        public Tank()
        {

        }

        //overloaded constructor(s)
        public Tank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed, float _rotation, int _player, float _scale, Texture2D _whiteRectangle, Keys _keyUp, Keys _keyLeft, Keys _keyDown, Keys _keyRight, Keys _keyBoost, Keys _keyReverse, Keys _keyFire, Keys _keyExplode, Keys _keyMine)
        {
            tankTexture = _game.Content.Load<Texture2D>(_tankSpriteName);
            location = _location;
            startingLocation = _location;
            speed = _speed;
            rotation = _rotation;
            origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);
            game = _game;
            player = _player;
            scale = _scale;
            whiteRectangle = _whiteRectangle;
            keyUp = _keyUp;
            keyLeft = _keyLeft;
            keyDown = _keyDown;
            keyRight = _keyRight;
            keyBoost = _keyBoost;
            keyReverse = _keyReverse;
            fireKey = _keyFire;
            explodeKey = _keyExplode;
            mineKey = _keyMine;
            alive = true;
            lives = 3;
            respawnParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Firebrick, 0);
            deathParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Green, 0);
            hitParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Silver, 0);
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(tankTexture, location, null, null, origin, rotation, null, null);
            } else
			{
                if(explosion != null)
                {
                    explosion.Draw(spriteBatch);
                }
				
			}
            respawnParticles.Draw(spriteBatch);
            deathParticles.Draw(spriteBatch);
            if (hitParticles != null)
            {
                hitParticles.Draw(spriteBatch);
            }
        }
        public virtual void Update(KeyboardState state, GameTime gameTime)
        {
            if (alive)
            {
                Move(state);
                updateRectangleLocation();
				//Check collisions
				colliding = false;

                foreach (Tank tank in game.enemyTanks)
                {
                    Collision collision = tank.isColliding(tankRect);
                    if (collision.depth > 0)
                    {
                        cancelOutCollisionOverlap(collision);
                    }
                }
                foreach (Tank tank in game.playerTanks) {
                    Collision collision = tank.isColliding(tankRect);
                    if (tank != this && collision.depth > 0) {
                        cancelOutCollisionOverlap(collision);
                    }
                }
                foreach (Tile[] tiles in game.map.map)
                {
                    foreach (Tile tile in tiles)
                    {
                        Collision collision = tile.isColliding(tankRect);
                        if (collision.depth > 0) //If collision is not an empty collision
                        {
                            cancelOutCollisionOverlap(collision);
                        }
                    }
                }

            } else //if not alive
			{
                if (explosion != null)
                {
                    explosion.Update();
                }
			}
            respawnParticles.Update(gameTime);
            deathParticles.Update(gameTime);
            if (hitParticles != null)
            {
                hitParticles.Update(gameTime);
            }

            if (state.IsKeyDown(fireKey) && fireDelay <= 0)
            {
                fireDelay = FIRE_DELAY;
                game.bullets.Add(Fire());
            }
            if (state.IsKeyDown(explodeKey) && explosionDelay <= 0)
            {
                explosionDelay = EXPLOSION_DELAY;
                Explode();
            }
            if (state.IsKeyDown(mineKey) && mineDelay <= 0)
            {
                mineDelay = MINE_DELAY;
                game.landmines.Add(new Landmine(game, new Rectangle((int)location.X, (int)location.Y, 20, 20), Vector2.Zero, Color.Orange, player, 0, whiteRectangle));
            }


        }
        private void cancelOutCollisionOverlap(Collision collision) {
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
        private void updateRectangleLocation() {
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
        }
        public virtual void Move(KeyboardState state)
        {
            //Declare the variables used to determine the direction and speed of the tank.
            bool RIGHT_down = state.IsKeyDown(keyRight);
            bool DOWN_down = state.IsKeyDown(keyDown);
            bool LEFT_down = state.IsKeyDown(keyLeft);
            bool UP_down = state.IsKeyDown(keyUp);
            bool BOOST_down = state.IsKeyDown(keyBoost);
            bool REVERSE_down = state.IsKeyDown(keyReverse);
            bool isPressedLeft = false;
            bool isPressedRight = false;
            bool isDrifting = false;

            //isPressed keyUp statements
            if (!RIGHT_down)
            {
                isPressedRight = false;
            }

            if (!LEFT_down)
            {
                isPressedLeft = false;
            }

            if (!LEFT_down || !RIGHT_down)
            {
                isDrifting = false;
            }

            //Corner drifting statements
            if (isPressedRight && UP_down && LEFT_down)
            {
                isDrifting = true;
                Rotate(UP_RIGHT);
                MoveUp(BOOST_down, REVERSE_down);

            } else if (isPressedRight && DOWN_down && LEFT_down)
            {
                isDrifting = true;
                Rotate(DOWN_RIGHT);
                MoveDown(BOOST_down, REVERSE_down);
            }

            if (isPressedLeft && UP_down)
            {
                isDrifting = true;
                Rotate(UP_LEFT);
                MoveUp(BOOST_down, REVERSE_down && RIGHT_down);

            } else if (isPressedLeft && DOWN_down)
            {
                isDrifting = true;
                Rotate(DOWN_LEFT);
                MoveDown(BOOST_down, REVERSE_down && RIGHT_down);
            }

            //Basic movement statements
            if (UP_down && !isDrifting)
            {
                Rotate(UP);
                MoveUp(BOOST_down, REVERSE_down);
                if (RIGHT_down && !BOOST_down)
                {
                    Rotate(UP_RIGHT);
                    MoveRight(BOOST_down, REVERSE_down);
                }
                if (LEFT_down && !BOOST_down)
                {
                    Rotate(UP_LEFT);
                    MoveLeft(BOOST_down, REVERSE_down);
                }
            }
            else if (DOWN_down && !isDrifting)
            {
                Rotate(DOWN);
                MoveDown(BOOST_down, REVERSE_down);
                if (RIGHT_down && !BOOST_down)
                {
                    Rotate(DOWN_RIGHT);
                    MoveRight(BOOST_down, REVERSE_down);
                }
                if (LEFT_down && !BOOST_down)
                {
                    Rotate(DOWN_LEFT);
                    MoveLeft(BOOST_down, REVERSE_down);
                }
            }
            else if (RIGHT_down && !isDrifting)
            {
                Rotate(RIGHT);
                MoveRight(BOOST_down, REVERSE_down);
                    if (!isPressedLeft)
                    {
                        isPressedRight = true;
                    }
            }
            else if (LEFT_down && !isDrifting)
            {
                Rotate(LEFT);
                MoveLeft(BOOST_down, REVERSE_down);
                if (!isPressedRight)
                {
                        isPressedLeft = true;
                }
            }
            
        }
        public void MoveLeft(bool isBoostPressed, bool isReversedPressed)
        {
            if (isBoostPressed)
            {
                this.location.X -= (2) + this.speed.X;
            }
            else if (isReversedPressed)
            {
                this.location.X += this.speed.Y;
            } 
            else
            {
                this.location.X -= this.speed.X;
            }
        }
        public void MoveRight(bool isBoostPressed, bool isReversedPressed)
        {
            if (isBoostPressed)
            {
                this.location.X += (2) + this.speed.X;
            }
            else if (isReversedPressed)
            {
                this.location.X -= this.speed.X;
            }
            else
            {
                this.location.X += this.speed.X;
            }
        }
        public void MoveUp(bool isBoostPressed, bool isReversedPressed)
        {
            if (isBoostPressed)
            {
                this.location.Y -= (2) + this.speed.Y;
            }
            else if (isReversedPressed)
            {
                this.location.Y += this.speed.Y;
            }
            else
            {
                this.location.Y -= this.speed.Y;
            }
        }
        public void MoveDown(bool isBoostPressed, bool isReversedPressed)
        {
            if (isBoostPressed)
            {
                this.location.Y += (2) + this.speed.Y;
            }
            else if (isReversedPressed)
            {
                this.location.Y -= this.speed.Y;
            }
            else
            {
                this.location.Y += this.speed.Y;
            }
        }
        public void Rotate(float angle)
        {
            this.rotation = angle;
        }
        public Bullet Fire()
        {
            if (alive)
            {
                game.sound.PlaySound(Sound.Sounds.LASERSHOOT);
                if (rotation == UP)
                {
                    return new Bullet(game, new Rectangle((int)location.X-2, (int)location.Y, 5, 5), new Vector2(0, -20), Color.Red, player, UP, whiteRectangle);
                }
                else if (rotation == UP_RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X-2, (int)location.Y-2, 5, 5), new Vector2(10, -10), Color.Red, player, UP_RIGHT, whiteRectangle);
                }
                else if (rotation == RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X-5, (int)location.Y-2, 5, 5), new Vector2(20, 0), Color.Red, player, RIGHT, whiteRectangle);
                }
                else if (rotation == DOWN_RIGHT)
                {
                    return new Bullet(game, new Rectangle((int)location.X, (int)location.Y, 5, 5), new Vector2(10, 10), Color.Red, player, DOWN_RIGHT, whiteRectangle);
                }
                else if (rotation == DOWN)
                {
                    return new Bullet(game, new Rectangle((int)location.X-2, (int)location.Y-5, 5, 5), new Vector2(0, 20), Color.Red, player, DOWN, whiteRectangle);
                }
                else if (rotation == DOWN_LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X-2, (int)location.Y-2, 5, 5), new Vector2(-10, 10), Color.Red, player, DOWN_LEFT, whiteRectangle);
                }
                else if (rotation == LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X, (int)location.Y-2, 5, 5), new Vector2(-20, 0), Color.Red, player, LEFT, whiteRectangle);
                }
                else if (rotation == UP_LEFT)
                {
                    return new Bullet(game, new Rectangle((int)location.X-3, (int)location.Y-3, 5, 5), new Vector2(-10, -10), Color.Red, player, UP, whiteRectangle);
                }
                else
                {
                    return null;
                }
            }
            return null;
            
        }
        public virtual void Hit()
        {
            lives -= 1;
            if(lives <  1)
            {
                Die();
            } else {
                hitParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Firebrick, 2, 5);
            }
        }
        public virtual void Die()
        {
            if (alive)
            {
                deathParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Green, 2);
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
                respawnParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Blue, 2);
                alive = true;
            }
        }
		public void Explode()
		{
			if(alive)
			{
				explosion = new Explosion(location, game, player, whiteRectangle, Color.Firebrick);
                game.sound.PlaySound(Sound.Sounds.EXPLOSION);
				Die();
			}
		}
        public Collision isColliding(Rectangle possibleCollisionRect)
        {
            Rectangle intersect = Rectangle.Intersect(possibleCollisionRect, tankRect);
            if (intersect.Width > 0 || intersect.Height > 0)
            {
                if (possibleCollisionRect.Top < tankRect.Bottom && Math.Abs(intersect.Width) > Math.Abs(intersect.Height) && possibleCollisionRect.Y > tankRect.Y)
                {
                    float depth = intersect.Height;
                    return new Collision(Collision.Side.TOP, depth);
                }
                if (possibleCollisionRect.Bottom > tankRect.Top && Math.Abs(intersect.Width) > Math.Abs(intersect.Height))
                {
                    float depth = intersect.Height;
                    return new Collision(Collision.Side.BOTTOM, depth);
                }
                if (possibleCollisionRect.Left < tankRect.Right && Math.Abs(intersect.Width) < Math.Abs(intersect.Height) && possibleCollisionRect.Right > tankRect.Right)
                {
                    float depth = intersect.Width;
                    return new Collision(Collision.Side.LEFT, depth);
                }
                if (possibleCollisionRect.Right > tankRect.Right - tankRect.Width && possibleCollisionRect.Right > tankRect.Left && Math.Abs(intersect.Width) < Math.Abs(intersect.Height))
                {
                    float depth = intersect.Width;
                    return new Collision(Collision.Side.RIGHT, depth);
                }
            }
            return new Collision();
        }
        public void updateDelays(GameTime gameTime) {
            float timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            fireDelay -= timer;
            explosionDelay -= timer;
            mineDelay -= timer;

            //if tanks are dead, decrease their time until they respawn
            if (!alive)
            {
                respawnDelay -= timer;
                if (respawnDelay < 0)
                {
                    Respawn(startingLocation);
                    respawnDelay = BACK_ALIVE_DELAY;
                }
            }
        }
    }
}
