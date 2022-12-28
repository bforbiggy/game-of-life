﻿using System;
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


	[STAThread]
	static void Main(string[] args)
	{
		img = new Image();
		window = new Window();
		window.Content = img;
		window.Show();
		board = new Board((int)window.ActualWidth, (int)window.ActualHeight);

		RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
		RenderOptions.SetEdgeMode(img, EdgeMode.Aliased);
		bmp = new WriteableBitmap(board.width, board.height, 96, 96, PixelFormats.Bgr32, null);
		img.Source = bmp;
		img.Stretch = Stretch.None;
		img.HorizontalAlignment = HorizontalAlignment.Left;
		img.VerticalAlignment = VerticalAlignment.Top;
		img.MouseMove += new MouseEventHandler(MouseMove);
		img.MouseLeftButtonDown += new MouseButtonEventHandler(MouseLeftButtonDown);
		img.MouseRightButtonDown += new MouseButtonEventHandler(MouseRightButtonDown);

		Application app = new Application();
		app.Run();
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
		byte[] BLACK = { 0, 0, 0, 0 };
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