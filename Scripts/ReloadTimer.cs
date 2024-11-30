using Godot;
using System;

public partial class ReloadTimer : ProgressBar
{
	public override void _Ready()
	{
		Visible = false;
	}

	public override void _Process(double delta)
	{

	}

	public void MainUpdate(double ReloadTimer, double ReloadDuration)
	{
		Visible = true;
		Value = (ReloadTimer / ReloadDuration) * 100;
		if (Value >= 100) Visible = false;
	}
}
