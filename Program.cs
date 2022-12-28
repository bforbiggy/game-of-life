using System;
using System.IO;
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
		int x = (int)(e.GetPosition(img).X);
		int y = (int)(e.GetPosition(img).Y);
		gd.DrawPixel(x, y, GameDisplay.WHITE);
	}

	public static void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		int x = (int)(e.GetPosition(img).X);
		int y = (int)(e.GetPosition(img).Y);
		gd.ErasePixel(x, y);
	}

	public static void MouseMove(object sender, MouseEventArgs e)
	{
		int x = (int)(e.GetPosition(img).X);
		int y = (int)(e.GetPosition(img).Y);
		if (e.LeftButton == MouseButtonState.Pressed)
			gd.DrawPixel(x, y, GameDisplay.WHITE);
		else if (e.RightButton == MouseButtonState.Pressed)
			gd.ErasePixel(x, y);
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

		// Run program
		Application app = new Application();
		app.Startup += (a, b) =>
		{
			board.Load(@"board.save");
			board.StepNext();
			gd.Update(board);
		};
		app.Run();
	}
}