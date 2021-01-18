using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Common
{
    public readonly struct Position
    {
        public Position(Vector3 point, Quaternion rotation)
        {
            Point = point;
            Rotation = rotation;
        }

        public Vector3 Point { get; }

        public Quaternion Rotation { get; }

        public bool Equals(Position other)
        {
            return Point.Equals(other.Point);
        }

        public override bool Equals(object obj)
        {
            return obj is Position other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Point.GetHashCode();
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }
    }
}