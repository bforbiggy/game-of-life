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

	public bool[,] getNextState()
	{
		bool[,] grid = new bool[width, height];
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				if (this.grid[x, y])
				{
					if (getLiveNeighbors(x, y) < 2)
						grid[x, y] = false;
				}
			}
		}
		return grid;
	}

	public bool inBounds(int x, int y)
	{
		return true;
	}

	public int getLiveNeighbors(int x, int y)
	{
		int total = 0;

		return 0;
	}

	public bool this[int a, int b]
	{
		get { return grid[a, b]; }
		set { grid[a, b] = value; }
	}
}