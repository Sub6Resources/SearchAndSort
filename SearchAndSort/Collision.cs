namespace SearchAndSort
{
    public class Collision
    {
        public enum Side
        {
            LEFT,
            RIGHT,
            TOP,
            BOTTOM
        }

        public float depth;
        public Side side;

        public Collision()
        {
        }

        public Collision(Side _side, float _depth)
        {
            side = _side;
            depth = _depth;
        }
    }
}