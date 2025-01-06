using Godot;
using System;

public partial class ReloadTimer : TextureProgressBar
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
		if (Value <= 0) Visible = false;
	}
}
