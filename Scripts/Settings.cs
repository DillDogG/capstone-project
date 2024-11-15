using Godot;
using System;

public partial class Settings : Control
{
    [Export]
    public Game Game;

    [Export]
    public Player Player;

    [Export]
    public Slider FOV_Slider;

    [Export]
    public Slider MouseSens_Slider;

    [Export]
    public Label FOV_Label;

    [Export]
    public Label MouseSens_Label;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // make a load from a file
	}

    public void SaveSettings()
    {
        // make it save the settings
        Game.ExitSettings();
    }

    private double prevFOV;
    private double prevMouseSens;
    public override void _Process(double delta)
    {
        if (FOV_Slider.Value != prevFOV)
        {
            FOV_Label.Text = FOV_Slider.Value.ToString();
            Player.BaseFOV = FOV_Slider.Value;
            Player.SmoothFOV(FOV_Slider.Value, 0);
        }

        if (MouseSens_Slider.Value != prevMouseSens)
        {
            MouseSens_Label.Text = MouseSens_Slider.Value.ToString();
            Player.MouseSensitivity = (float)MouseSens_Slider.Value * 0.01f;
        }
    }
}
