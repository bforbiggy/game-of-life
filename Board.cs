using System.Text;

namespace GameOfLife;

public class Board
{
	public int width;
	public int height;
	public bool[,] grid;

	public Board(int width = 25, int height = 12)
	{
		this.width = width;
		this.height = height;
		grid = new bool[width, height];
	}

	public bool[,] NextState()
	{
		bool[,] grid = new bool[width, height];
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				if (this.grid[x, y])
				{
					if (GetNeighbors(x, y) < 2)
						grid[x, y] = false;
				}
			}
		}
		return grid;
	}

	public bool InBounds(int x, int y)
	{
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	public int GetNeighbors(int x, int y)
	{
		int total = 0;
		for (int xOffset = -1; xOffset <= 1; xOffset++)
		{
			for (int yOffset = -1; yOffset <= 1; yOffset++)
			{
				if (!InBounds(x + xOffset, y + yOffset) || (xOffset == 0 && yOffset == 0))
					continue;

				if (grid[x + xOffset, y + yOffset])
					total++;
			}
		}
		return total;
	}

	public bool this[int a, int b]
	{
		get { return grid[a, b]; }
		set { grid[a, b] = value; }
	}

	public override String ToString()
	{
		StringBuilder sb = new StringBuilder();
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				sb.Append(grid[x, y] ? "X" : "_");
			}
			sb.AppendLine();
		}
		return sb.ToString();
	}
}