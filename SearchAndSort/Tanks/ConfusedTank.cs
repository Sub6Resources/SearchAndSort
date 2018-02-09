using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SearchAndSort.Tanks;

namespace SearchAndSort
{
    public class ConfusedTank : Tank
    {
        private bool fired;
        public ConfusedTank()
        {
            
        }

        public ConfusedTank(Game1 _game, Texture2D _tankTexture, Vector2 _location, Vector2 _speed, float _rotation, int _player) : base(_game, _tankTexture, _location, _speed, _rotation, _player)
        {
            
        }

        public override void Move(KeyboardState state)
        {
            
        }

        public override void Update(KeyboardState state, GameTime gameTime)
        {
            base.Update(state, gameTime);
            int toDestroy = SearchAndDestroy();
            if (toDestroy > -1)
            {
                Tank tankToRotateTo = game.enemyTanks[toDestroy];
                Console.WriteLine(AimAt(new Vector2(tankToRotateTo.location.X, tankToRotateTo.location.Y)));
                SlowlyRotate(AimAt(new Vector2(tankToRotateTo.location.X, tankToRotateTo.location.Y)), gameTime);
                if (Math.Abs(rotation - AimAt(new Vector2(tankToRotateTo.location.X, tankToRotateTo.location.Y))) < Math.PI/180 && fired == false)
                {
                    fired = true;
                    game.bullets.Add(Fire());
                }
                else
                {
                    fired = false;
                }
            }
        }

        public int SearchAndDestroy()
        {
            //Unordered List Sort
            int min = 100;
            int mindex = -1;
            for (int i = 0; i < game.enemyTanks.Count; i++)
            {
                if (game.enemyTanks[i].strength < min && game.enemyTanks[i].alive)
                {
                    min = game.enemyTanks[i].strength;
                    mindex = i;
                }
            }

            return mindex;
        }
    }
}