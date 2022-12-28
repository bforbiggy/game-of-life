
using GameOfLife;

Board board = new Board();
board[5, 5] = true;
for (int y = 0; y < board.height; y++)
{
	for (int x = 0; x < board.width; x++)
	{
		Console.Write(board[x, y] ? "X" : "_");
	}
	Console.WriteLine();
}