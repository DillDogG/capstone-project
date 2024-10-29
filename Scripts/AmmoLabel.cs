using Godot;
using System;

public partial class AmmoLabel : Label
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void MainUpdate(RangedWeapon weapon)
	{
		Text = $"{weapon.AmmoCount} / {weapon.MaxAmmoCount}";
	}
}
