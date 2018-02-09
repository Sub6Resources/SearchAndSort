using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
    public class Score
    {
        private Game1 game { get; set; }
        private int[] score;
        private int numOfPlayers { get; set; }
        //private SpriteFont spriteFont;
        public Score() { }
        public Score(Game1 _game, int _numOfPlayers)
        {
            game = _game;
            numOfPlayers = _numOfPlayers;
            score = new int[numOfPlayers];
            for(int i=0; i < score.Length; ++i)
            {
                score[i] = 0;
            }
            Load();
        }
        public void Load()
        {
            //spriteFont = game.Content.Load<SpriteFont>("Arial");
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
            //spriteBatch.DrawString(spriteFont, "Player 1: " + score[0]+"               Player 2: "+score[1], new Vector2(48,48), Color.Red);
            //spriteBatch.DrawString(spriteFont, "Lives P1: " + game.playerTanks[1].lives + "               Lives P2: " + game.playerTanks[2].lives, new Vector2(48, 63), Color.Red);
        }
    }
}
