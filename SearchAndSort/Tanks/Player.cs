using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SearchAndSort.Tanks
{
    public class Player: Tank
    {
        //data members
        public Controls controls;
        private const float FIRE_DELAY = 0.5f;
        private const float EXPLOSION_DELAY = 5f;
        private const float BACK_ALIVE_DELAY = 2f;
        private const float MINE_DELAY = 20f;

        //generic constructor
        public Player() {}

            //overloaded constructor(s)
        public Player(Game1 _game, Texture2D _tankTexture, Vector2 _location, Vector2 _speed, float _rotation, int _player, Controls _controls): base(_game, _tankTexture, _location, _speed, _rotation, _player)
        {
            controls = _controls;
        }
        public override void Update(KeyboardState state, GameTime gameTime) 
        {
            updateDelays(gameTime);
            base.Update(state, gameTime);
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

            if (state.IsKeyDown(controls.SEARCH))
            {
                int toAimAt = SearchAndDestroy(ref game.enemyTanks);
                if (toAimAt > -1)
                {
                    SlowlyRotate(AimAt(game.enemyTanks[toAimAt].location), gameTime);
                }
            }
        }
        public override void Move(KeyboardState state)
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
            }
            else if (isPressedRight && DOWN_down && LEFT_down)
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
            }
            else if (isPressedLeft && DOWN_down)
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
        public void updateDelays(GameTime gameTime)
        {
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
