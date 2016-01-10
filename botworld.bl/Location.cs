using System;

namespace botworld.bl
{
	public class Location
	{
		public Location(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; private set; }

		public int Y { get; private set; }

		public Location GetNeighborLocationInDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.North:
					return new Location(X, Y - 1);
				case Direction.East:
					return new Location(X + 1, Y);
				case Direction.South:
					return new Location(X, Y + 1);
				case Direction.West:
					return new Location(X - 1, Y);
				default:
					throw new ArgumentOutOfRangeException("direction");
			}
		}

		public override bool Equals(object other)
		{
			return Equals(other as Location);
		}

		public bool Equals(Location other)
		{
			if ((object)other == null) 
				return false;

			if (ReferenceEquals(this, other)) 
				return true;

			return X == other.X && Y == other.Y;
		}

		public override int GetHashCode()
		{
			return X ^ Y;
		}

		public static bool operator ==(Location location1, Location location2)
		{
			if (ReferenceEquals(location1, location2)) 
				return true;

			if ((object)location1 == null || (object)location2 == null) 
				return false;

			return location1.X == location2.X && location1.Y == location2.Y;
		}

		public static bool operator !=(Location location1, Location location2)
		{
			return !(location1 == location2);
		}
	}
}