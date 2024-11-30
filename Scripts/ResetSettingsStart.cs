using Godot;
using System;

public partial class ResetSettingsStart : Button
{
    [Export]
    public StartSettings Settings;


    public override void _Pressed()
    {
        base._Pressed();
        Settings.ResetSettings();
    }
}
