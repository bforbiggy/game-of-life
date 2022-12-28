
using GameOfLife;

public class Game
{

	public static void Main(string[] args)
	{
		Board board = new Board();
		board[5, 5] = true;
		board[5, 6] = true;
		board[5, 7] = true;

		board.grid = board.NextState();
		Console.WriteLine(board);
	}
}