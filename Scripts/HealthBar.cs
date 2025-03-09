using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	public void MainUpdate(double health)
	{
		if (health > 100) Value = (health - 100) / 200 * 75 + 25;
		else Value = health / 300 * 75;
	}
}
