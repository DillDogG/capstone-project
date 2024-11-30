using Godot;
using System;

public partial class SaveGameButton : Button
{
    [Export]
    public Game game;

    public override void _Pressed()
    {
        base._Pressed();
        game.SaveGame();
    }
}
