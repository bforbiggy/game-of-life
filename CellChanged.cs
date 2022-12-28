using System;

public class CellChanged : EventArgs
{
	public int x;
	public int y;
	public bool state;

	public CellChanged(int[] data) : base()
	{
		x = data[0];
		y = data[1];
		state = data[2] != 0;
	}
}