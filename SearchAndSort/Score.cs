using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{
    public class Score
    {
        private readonly Game1 game;
        private int numPlayers;
        private List<int> score;
        private SpriteFont spriteFont;

        public Score(Game1 _game)
        {
            game = _game;
            score = new List<int>();
            for (var i = 0; i < 100; ++i) score.Add(0);
            Load();
        }

        public void Load()
        {
            spriteFont = game.Content.Load<SpriteFont>("score");
        }

        public int getScore(int playerIndex)
        {
            return score[playerIndex];
        }

        public void addScore(int playerIndex, int pointsToAdd)
        {
            score[playerIndex] += pointsToAdd;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var player in game.playerTanks)
                spriteBatch.DrawString(
                    spriteFont,
                    "Player " + player.player + ": " + score[player.player] + " points. Lives: "+player.lives,
                    new Vector2(48, 48 + 16 * player.player),
                    player.alive ? player.color : Color.DimGray);
        }

        public void Reset()
        {
            score = new List<int>();
            for (var i = 0; i < 100; ++i) score.Add(0);
        }
    }
}