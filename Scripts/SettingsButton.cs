using Godot;
using System;

public partial class SettingsButton : Button
{
    public Game Game;

    public override void _Ready()
    {
        Game = GetTree().Root.GetNode<Game>("Node3D");
    }
    
    public override void _Pressed()
	{
		base._Pressed();
		Game.Settings();
	}
}
