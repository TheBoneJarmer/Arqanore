using System;

namespace Seanuts.Logic.PathFinding
{
    /// <summary>
    /// A 2D position structure
    /// </summary>
    public struct SNPosition : IEquatable<SNPosition>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">the x-position</param>
        /// <param name="y">the y-position</param>
        public SNPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// X-position
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y-position
        /// </summary>
        public int Y { get; }

        public override string ToString() => $"Position: ({this.X}, {this.Y})";

        public bool Equals(SNPosition other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is SNPosition && Equals((SNPosition) obj);
        }
        
        public static bool operator ==(SNPosition a, SNPosition b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SNPosition a, SNPosition b)
        {
            return !a.Equals(b);
        }

        public static SNPosition operator +(SNPosition a, SNPosition b)
        {
            return new SNPosition(a.X + b.X, a.Y + b.Y);
        }

        public static SNPosition operator -(SNPosition a, SNPosition b)
        {
            return new SNPosition(a.X - b.X, a.Y - b.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X * 397) ^ this.Y;
            }
        }
    }
}
