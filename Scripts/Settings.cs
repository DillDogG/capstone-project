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

    [Export]
    public TabBar Section;

    [Export]
    public Control Visuals;

    [Export]
    public Control Audio;

    [Export]
    public Control Controls;

    [Export]
    public Control Controller;

    private int currTab = -1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // make a load from a file
	}

    public void SaveSettings()
    {
        // make it save the settings
        Game.ReadFile.SaveSettings(FOV_Slider.Value, MouseSens_Slider.Value);
        currTab = 0;
        Game.ExitSettings();
    }

    public void ResetSettings()
    {
        FOV_Slider.Value = 75;
        MouseSens_Slider.Value = 1;
    }

    private double prevFOV = 75;
    private double prevMouseSens = 1;
    public override void _Process(double delta)
    {
        if (Section.CurrentTab != currTab) SetVisibleSection();
        if (FOV_Slider.Value != prevFOV)
        {
            FOV_Label.Text = FOV_Slider.Value.ToString();
            Player.BaseFOV = FOV_Slider.Value;
            Player.SmoothFOV(FOV_Slider.Value, 0);
            prevFOV = FOV_Slider.Value;
        }

        if (MouseSens_Slider.Value != prevMouseSens)
        {
            MouseSens_Slider.Value = Math.Round(MouseSens_Slider.Value, 2);
            if (MouseSens_Slider.Value <= 0)
            {
                MouseSens_Slider.Value = 0.1;
            }
            MouseSens_Label.Text = Math.Round(MouseSens_Slider.Value, 2).ToString();
            Player.MouseSensitivity = (float)MouseSens_Slider.Value * 0.01f;
            prevMouseSens = MouseSens_Slider.Value;
        }
    }

    public void SetVisibleSection()
    {
        currTab = Section.CurrentTab;
        switch (currTab)
        {
            case 0:
                {
                    Visuals.Visible = true;
                    Audio.Visible = false;
                    Controls.Visible = false;
                    Controller.Visible = false;
                    break;
                }
            case 1:
                {
                    Visuals.Visible = false;
                    Audio.Visible = true;
                    Controls.Visible = false;
                    Controller.Visible = false;
                    break;
                }
            case 2:
                {
                    Visuals.Visible = false;
                    Audio.Visible = false;
                    Controls.Visible = true;
                    Controller.Visible = false;
                    break;
                }
            case 3:
                {
                    Visuals.Visible = false;
                    Audio.Visible = false;
                    Controls.Visible = false;
                    Controller.Visible = true;
                    break;
                }
        }
    }
}
