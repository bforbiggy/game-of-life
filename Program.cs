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
		gd.DrawPixel(e, GameDisplay.WHITE);
	}

	public static void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		gd.ErasePixel((int)(e.GetPosition(img).X), (int)(e.GetPosition(img).Y));
	}

	public static void MouseMove(object sender, MouseEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
			gd.DrawPixel(e, GameDisplay.WHITE);
		else if (e.RightButton == MouseButtonState.Pressed)
			gd.ErasePixel((int)(e.GetPosition(gd.img).X), (int)(e.GetPosition(img).Y));
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