using Godot;
using System;

public partial class ReturnToMenu : Button
{
    public override void _Pressed()
    {
        base._Pressed();
        GetTree().ChangeSceneToFile("res://Scenes/StartScreen.tscn");
    }
}