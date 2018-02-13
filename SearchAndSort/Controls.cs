using System;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
    public class Controls
    {
        public Keys UP;
        public Keys LEFT;
        public Keys RIGHT;
        public Keys DOWN;
        public Keys FIRE;
        public Keys EXPLODE;
        public Keys MINE;
        public Keys BOOST;
        public Keys REVERSE;
        public Keys SEARCH;

        public Controls(Keys keyUp, Keys keyLeft, Keys keyRight, Keys keyDown, Keys keyFire, Keys keyExplode, Keys keyMine, Keys keyBoost, Keys keyReverse, Keys keySearch) {
            UP = keyUp;
            LEFT = keyLeft;
            RIGHT = keyRight;
            DOWN = keyDown;
            FIRE = keyFire;
            EXPLODE = keyExplode;
            MINE = keyMine;
            BOOST = keyBoost;
            REVERSE = keyReverse;
            SEARCH = keySearch;
        }
    }
}
