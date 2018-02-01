using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SearchAndSort
{
    public class StaticTank : EnemyTank
    {
        public new bool enemy = true;

        public StaticTank() { }
        public StaticTank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed, float _rotation, int _player, int _strength, Texture2D _whiteRectangle)
        {
            tankTexture = _game.Content.Load<Texture2D>(_tankSpriteName);
            location = _location;
            startingLocation = _location;
            speed = _speed;
            rotation = _rotation;
            origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);
            game = _game;
            player = _player;
            scale = 1f;
            whiteRectangle = _whiteRectangle;
            alive = true;
            strength = _strength;
            lives = _strength;
            respawnParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Green, 0);
            deathParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Yellow, 0);
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
            targetDirection = DOWN;
        }

        public override void Update(KeyboardState state, GameTime gameTime) {
            SlowlyRotate(AimAt(game.playerTanks[0].location), gameTime);
            base.Update(state, gameTime);
        }

        public override void Move(KeyboardState state)
        {
            //AI CODE--This Tank does not move.
        }
    }
}
