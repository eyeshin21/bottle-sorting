namespace Anvil
{
	public enum Direction4
	{
	    Left,
		Up,
		Right,
		Down
	}

	public static partial class ExtensionMethods
    {
		public static Direction4 Reverse(this Direction4 direction)
        {
			if (direction == Direction4.Left) return Direction4.Right;
			if (direction == Direction4.Right) return Direction4.Left;
			if (direction == Direction4.Up) return Direction4.Down;
			if (direction == Direction4.Down) return Direction4.Up;

			return direction;
		}

		public static void GetNextRowColumn(this Direction4 direction, int row, int column, out int nextRow, out int nextColumn)
        {
			nextRow = row;
			nextColumn = column;

			if (direction == Direction4.Left)
            {
				nextColumn--;
            }
			else if (direction == Direction4.Right)
			{
				nextColumn++;
			}
			else if (direction == Direction4.Up)
            {
				nextRow--;
            }
			else if (direction == Direction4.Down)
			{
				nextRow++;
			}
			else
            {
            }
		}
    }
}