using Godot;
using System;

public partial class SpeedDisplay : Label
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void MainUpdate(double speed)
	{
		Text = $"{(speed / 2).ToString("0.00")} mph";
	}
}
