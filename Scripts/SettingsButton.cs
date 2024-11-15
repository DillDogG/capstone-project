using Godot;
using System;

public partial class SettingsButton : Button
{
    [Export]
    public Game Game;

	public override void _Pressed()
	{
		base._Pressed();
		Game.Settings();
	}
}
