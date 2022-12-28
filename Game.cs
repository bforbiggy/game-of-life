using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using GameOfLife;

namespace WriteableBitmapDemo;
class Game
{
	static Window window = null!;
	static Image img = null!;
	static WriteableBitmap bmp = null!;
	static Board board = null!;

	static int WHITE = 255 << 16 | 255 << 8 | 255 << 0;
	static byte[] BLACK = { 0, 0, 0, 0 };


	[STAThread]
	static void Main(string[] args)
	{
		// Create window and board
		window = new Window();
		window.Show();
		board = new Board((int)window.ActualWidth, (int)window.ActualHeight);
		bmp = new WriteableBitmap(board.width, board.height, 96, 96, PixelFormats.Bgr32, null);

		// Create image render from bitmap
		img = new Image();
		RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
		RenderOptions.SetEdgeMode(img, EdgeMode.Aliased);
		img.Source = bmp;
		img.Stretch = Stretch.None;
		img.MouseMove += new MouseEventHandler(MouseMove);
		img.MouseLeftButtonDown += new MouseButtonEventHandler(MouseLeftButtonDown);
		img.MouseRightButtonDown += new MouseButtonEventHandler(MouseRightButtonDown);
		window.Content = img;

		// Run application
		Application app = new Application();
		app.Startup += (a, b) =>
		{
			board[5, 5] = true;
			board[5, 6] = true;
			board[5, 7] = true;
			board.StepNext();
			Update(board);
		};
		app.Run();
	}

	static void Update(Board board)
	{
		for (int x = 0; x < board.width; x++)
		{
			for (int y = 0; y < board.height; y++)
			{
				DrawPixel(x, y, board[x, y] ? WHITE : 0);
			}
		}
	}

	// Draws a pixel at the target location
	static void DrawPixel(MouseEventArgs e, int color)
	{
		int x = (int)e.GetPosition(img).X;
		int y = (int)e.GetPosition(img).Y;
		DrawPixel(x, y, color);
	}

	static void DrawPixel(int x, int y, int color)
	{
		try
		{
			// Lock buffer to write to it
			bmp.Lock();
			unsafe
			{
				// Get target pixel
				IntPtr bufferPtr = bmp.BackBuffer;
				bufferPtr += y * bmp.BackBufferStride;
				bufferPtr += x * 4;

				// Set target pixel color
				*((int*)bufferPtr) = color;
			}

			// Specify the area of the bitmap that changed.
			bmp.AddDirtyRect(new Int32Rect(x, y, 1, 1));
		}
		finally
		{
			// Release buffer
			bmp.Unlock();
		}
	}

	static void ErasePixel(int x, int y)
	{
		Int32Rect rect = new Int32Rect(x, y, 1, 1);
		bmp.WritePixels(rect, BLACK, 4, 0);
	}

	static void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		DrawPixel(e, WHITE);
	}

	static void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		ErasePixel((int)(e.GetPosition(img).X), (int)(e.GetPosition(img).Y));
	}

	static void MouseMove(object sender, MouseEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
			DrawPixel(e, WHITE);
		else if (e.RightButton == MouseButtonState.Pressed)
			ErasePixel((int)(e.GetPosition(img).X), (int)(e.GetPosition(img).Y));
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