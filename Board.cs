using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameOfLife;

public class Board
{
	public int width;
	public int height;
	public bool[,] grid;
	public event EventHandler? cellChanged;

	public Board(int width = 25, int height = 12)
	{
		this.width = width;
		this.height = height;
		grid = new bool[width, height];
	}

	public void StepNext()
	{
		Stack<int[]> stack = new Stack<int[]>();
		ArrayPool<int> shared = ArrayPool<int>.Shared;
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				int neighbors = GetNeighbors(x, y);
				if (this.grid[x, y])
				{
					// Too few living neighbors
					if (neighbors < 2)
					{
						int[] arr = shared.Rent(3);
						(arr[0], arr[1], arr[2]) = (x, y, 0);
						stack.Push(arr);
					}
					// Too many living neighbors
					else if (neighbors > 3)
					{
						int[] arr = shared.Rent(3);
						(arr[0], arr[1], arr[2]) = (x, y, 0);
						stack.Push(arr);
					}
					// Just enough living neighbors
					else
					{
						int[] arr = shared.Rent(3);
						(arr[0], arr[1], arr[2]) = (x, y, 1);
						stack.Push(arr);
					}
				}
				else
				{
					// Just enough living neighbors
					if (neighbors == 3)
					{
						int[] arr = shared.Rent(3);
						(arr[0], arr[1], arr[2]) = (x, y, 1);
						stack.Push(arr);
					}
				}
			}
		}

		while (stack.Count != 0)
		{
			int[] data = stack.Pop();
			CellChanged cc = new CellChanged(data);
			grid[cc.x, cc.y] = cc.state;
			cellChanged?.Invoke(this, cc);
			shared.Return(data);
		}
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

	public void Load(string path)
	{
		foreach (string line in File.ReadLines(path))
		{
			string[] tokens = line.Split(",");
			int x = Int32.Parse(tokens[0]);
			int y = Int32.Parse(tokens[1]);
			if (InBounds(x, y))
				grid[x, y] = true;
		}
	}

	public override string ToString()
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