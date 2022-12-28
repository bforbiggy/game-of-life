using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using GameOfLife;

namespace GameOfLife;
class GameDisplay
{
	public Window window;
	public Image img;
	public WriteableBitmap bmp;
	public Board board;

	public static byte[] BLACK = { 0, 0, 0 };
	public static byte[] WHITE = { 255, 255, 255 };


	public GameDisplay()
	{
		// Create window and board
		window = new Window();
		window.Width = 250;
		window.Height = 250;
		window.Show();
		board = new Board((int)window.ActualWidth, (int)window.ActualHeight);
		bmp = new WriteableBitmap(board.width, board.height, 96, 96, PixelFormats.Bgr24, null);

		// Create image render from bitmap
		img = new Image();
		RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
		RenderOptions.SetEdgeMode(img, EdgeMode.Aliased);
		img.Source = bmp;
		img.Stretch = Stretch.None;

		window.Content = img;
	}

	public void Update(Board board)
	{
		for (int x = 0; x < board.width; x++)
		{
			for (int y = 0; y < board.height; y++)
			{
				DrawPixel(x, y, board[x, y] ? WHITE : BLACK);
			}
		}
	}

	// Draws a pixel at the target location
	public void DrawPixel(MouseEventArgs e, byte[] color)
	{
		int x = (int)e.GetPosition(img).X;
		int y = (int)e.GetPosition(img).Y;
		DrawPixel(x, y, color);
	}

	public void DrawPixel(int x, int y, byte[] color)
	{
		Int32Rect rect = new Int32Rect(x, y, 1, 1);
		bmp.WritePixels(rect, color, 4, 0);
	}

	public void ErasePixel(int x, int y)
	{
		Int32Rect rect = new Int32Rect(x, y, 1, 1);
		bmp.WritePixels(rect, BLACK, 4, 0);
	}

	// static void w_MouseWheel(object sender, MouseWheelEventArgs e)
	// {
	// 	System.Windows.Media.Matrix m = i.RenderTransform.Value;

	// 	if (e.Delta > 0)
	// 	{
	// 		m.ScaleAt(
	// 				1.5,
	// 				1.5,
	// 				e.GetPosition(w).X,
	// 				e.GetPosition(w).Y);
	// 	}
	// 	else
	// 	{
	// 		m.ScaleAt(
	// 				1.0 / 1.5,
	// 				1.0 / 1.5,
	// 				e.GetPosition(w).X,
	// 				e.GetPosition(w).Y);
	// 	}

	// 	i.RenderTransform = new MatrixTransform(m);
	// }
}