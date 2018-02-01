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
        public float rotation;
        public Texture2D tankTexture;
        public Vector2 origin;
        public Game1 game;
        public int player;
        public int lives;
        public float scale;
        public Controls controls;
        public bool alive = true;
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
        public Tank(Game1 _game, Texture2D _tankTexture, Vector2 _location, Vector2 _speed, float _rotation, int _player, Controls _controls)
        {
            tankTexture = _tankTexture;
            location = _location;
            startingLocation = _location;
            speed = _speed;
            rotation = _rotation;
            origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);
            game = _game;
            player = _player;
            scale = 1f;
            controls = _controls;
            alive = true;
            lives = 3;
            respawnParticles = new ParticleSpray(location, game, player, tankTexture, Color.Firebrick, 0);
            deathParticles = new ParticleSpray(location, game, player, tankTexture, Color.Green, 0);
            hitParticles = new ParticleSpray(location, game, player, tankTexture, Color.Silver, 0);
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(tankTexture, location, null, null, origin, rotation);
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
            if (!(this is EnemyTank))
            {
                updateDelays(gameTime);
            }
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

            if (!(this is EnemyTank))
            {
                if (state.IsKeyDown(controls.FIRE) && fireDelay <= 0)
                {
                    fireDelay = FIRE_DELAY;
                    game.bullets.Add(Fire());
                }
                if (state.IsKeyDown(controls.EXPLODE) && explosionDelay <= 0)
                {
                    explosionDelay = EXPLOSION_DELAY;
                    Explode();
                }
                if (state.IsKeyDown(controls.MINE) && mineDelay <= 0)
                {
                    mineDelay = MINE_DELAY;
                    game.landmines.Add(new Landmine(game, new Rectangle((int)location.X, (int)location.Y, 20, 20), Vector2.Zero, Color.Orange, player, 0, tankTexture));
                }
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
            bool RIGHT_down = state.IsKeyDown(controls.RIGHT);
            bool DOWN_down = state.IsKeyDown(controls.DOWN);
            bool LEFT_down = state.IsKeyDown(controls.LEFT);
            bool UP_down = state.IsKeyDown(controls.UP);
            bool BOOST_down = state.IsKeyDown(controls.BOOST);
            bool REVERSE_down = state.IsKeyDown(controls.REVERSE);
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
        public float AimAt(Vector2 target) {
            if (target.X > location.X)
            {
                return (float)(Math.Atan((location.Y - target.Y) / (location.X - target.X)));
            }
            else
            {
                return (float)(Math.Atan((location.Y - target.Y) / (location.X - target.X)) + Math.PI);
            }
        }
        public void SlowlyRotate(float targetRotation, GameTime gameTime) {
            Rotate(MathHelper.Lerp(rotation, targetRotation, (float)gameTime.ElapsedGameTime.TotalSeconds));
        }
        public Bullet Fire()
        {
            if (alive)
            {
                game.sound.PlaySound(Sound.Sounds.LASERSHOOT);
                return new Bullet(game, new Rectangle((int) location.X, (int) location.Y, 5, 5), new Vector2((float) (15 * Math.Cos(rotation)), (float) (15 * Math.Sin(rotation))), Color.Red, player, rotation, tankTexture);
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
                hitParticles = new ParticleSpray(location, game, player, tankTexture, Color.Firebrick, 2, 5);
            }
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
			if(alive)
			{
				explosion = new Explosion(location, game, player, tankTexture, Color.Firebrick);
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
