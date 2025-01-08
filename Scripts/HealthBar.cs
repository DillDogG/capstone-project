using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	public void MainUpdate(double health)
	{
		if (health > 50) Value = (health - 50) / 100 * 75 + 25;
		else Value = health / 150 * 75;
		GD.Print(health);
		GD.Print(Value);
	}
}
