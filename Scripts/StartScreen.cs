using Godot;
using System;

public partial class StartScreen : Node
{
    [Export]
    public Control PauseMenu { get; set; }

    [Export]
    public Control SettingsMenu { get; set; }

    public ReadFile ReadFile { get; set; } = new ReadFile();
    public override void _Ready()
	{
        Input.MouseMode = Input.MouseModeEnum.Visible;
        PauseMenu.Show();
        SettingsMenu.Hide();
        ReadFile.ConfirmFiles();
        if (SettingsMenu is StartSettings)
        {
            StartSettings menu = (StartSettings)SettingsMenu;
            ReadFile.LoadSettings(menu);
        }
    }
    public void LoadGame()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/" + ReadFile.LoadGame() + ".tscn");
    }

    public void NewGame()
    {
        GetTree().Paused = false;
        ReadFile.ResetGame();
        GetTree().ChangeSceneToFile("res://Scenes/FirstLevel.tscn");
    }

    public void Endless()
    {
        GetTree().Paused = false;
        //ReadFile.ResetGame();
        GetTree().ChangeSceneToFile("res://Scenes/Test Level.tscn");
    }

    public void Settings()
    {
        PauseMenu.Hide();
        SettingsMenu.Show();
    }

    public void ExitSettings()
    {
        PauseMenu.Show();
        SettingsMenu.Hide();
    }

}
