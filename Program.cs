using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GameOfLife;

public class Program
{
	static GameDisplay gd = null!;
	static Board board = null!;
	static Image img = null!;

	public static void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		int[] pos = { (int)(e.GetPosition(img).X), (int)(e.GetPosition(img).Y) };
		gd.DrawPixel(pos[0], pos[1], GameDisplay.WHITE);
	}

	public static void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		int[] pos = { (int)(e.GetPosition(img).X), (int)(e.GetPosition(img).Y) };
		gd.ErasePixel(pos[0], pos[1]);
	}

	public static void MouseMove(object sender, MouseEventArgs e)
	{
		int[] pos = { (int)(e.GetPosition(img).X), (int)(e.GetPosition(img).Y) };
		if (e.LeftButton == MouseButtonState.Pressed)
			gd.DrawPixel(pos[0], pos[1], GameDisplay.WHITE);
		else if (e.RightButton == MouseButtonState.Pressed)
			gd.ErasePixel(pos[0], pos[1]);
	}

	public static void CellChanged(object? sender, EventArgs e)
	{
		CellChangArgs args = (CellChangArgs)e;
		if (args.state)
		{
			gd.DrawPixel(args.x, args.y, GameDisplay.WHITE);
		}
		else
		{
			gd.ErasePixel(args.x, args.y);
		}
	}

	public static async void loop()
	{
		while (true)
		{
			await Task.Delay(500);
			board.StepNext();
		}

	}

	[STAThread]
	public static void Main(string[] args)
	{
		// Initialize display and game
		gd = new GameDisplay();
		board = gd.board;
		img = gd.img;

		// Handle events
		img.MouseMove += MouseMove;
		img.MouseLeftButtonDown += MouseLeftButtonDown;
		img.MouseRightButtonDown += MouseRightButtonDown;
		board.cellChanged += CellChanged;

		// Run program
		Application app = new Application();
		app.Startup += (a, b) =>
		{
			board.Load(@"board.save");
			gd.Update(board);

			// Infinitely update board
			loop();
		};
		app.Run();
	}
}