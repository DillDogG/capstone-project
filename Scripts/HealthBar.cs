using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	public void MainUpdate(double health)
	{
		Value = health;
	}
}
