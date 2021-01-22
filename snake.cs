using System;
using System.Linq;
namespace SnakeExtensions
{
    struct Block
    {
        private int _x, _y;
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }
        public Block(int x, int y)
        {
            _x = x;
            _y = y;
        }
        private bool EqualsBlock(Block obj)
        {
            return (this.X == obj.X) && (this.Y == obj.Y);
        }
        public override bool Equals(object obj)
        {
            return obj is Block block &&
                   X == block.X &&
                   Y == block.Y;
        }
        public override int GetHashCode()
        {
            var hashCode = 1;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Block a, Block b)
        {
            return a.EqualsBlock(b);
        }
        public static bool operator !=(Block a, Block b)
        {
            return !a.EqualsBlock(b);
        }
    }
    enum Direction
    {
        Up, Down, Right, Left
    }
    static class Field
    {
        public static string[][] GetField()
        {
            return new string[][]{
                ToArray("▐▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐                ▌"),
                ToArray("▐▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▌"),
            };
        }
        private static string[] ToArray(string text)
        {
            return text.ToCharArray().Select(c => c.ToString()).ToArray();
        }
    }
}