using Godot;
using System;

public partial class ReadFile : Node
{
    //string FilePath { get; set; } = "user://SaveData.cfg";
    string Settings { get; set; } = "user://Settings.cfg";
    string SaveData { get; set; } = "user://SaveData.cfg";

    //public void LoadFiles()
    //{
    //    if (!FileAccess.FileExists(Settings))
    //    {
    //        FileAccess.Open(Settings, FileAccess.ModeFlags.Write);
    //    }

    //    if (!FileAccess.FileExists(SaveData))
    //    {
    //        FileAccess.Open(SaveData, FileAccess.ModeFlags.Write);
    //    }
    //}

    public void ConfirmFiles()
    {
        var file = new ConfigFile();
        Error err = file.Load(Settings);

        if (err != Error.Ok)
        {
            file = new ConfigFile();
            file.SetValue("Visual", "FOV", 75);
            file.SetValue("Controls", "MouseSens", 1);
        }
        file.Save(Settings);

        file = new ConfigFile();
        err = file.Load(SaveData);

        if (err != Error.Ok)
        {
            file = new ConfigFile();
            //file.SetValue("Visual", "FOV", 75);
            //file.SetValue("Controls", "MouseSens", 1);
        }
        file.Save(SaveData);
    }

    public bool SaveSettings(/*resolution save, */double FOV, double MouseSens)
    {
        var file = new ConfigFile();
        Error err = file.Load(Settings);

        if (err != Error.Ok)
        {
            return false;
        }
        file.SetValue("Visual", "FOV", FOV);
        file.SetValue("Controls", "MouseSens", MouseSens);
        file.Save(Settings);
        //while (file.GetPosition() < file.GetLength())
        //{
        //    /*if (Settings.GetLine().StartsWith("Resolution:"))
        //    {
                
        //    }
        //    else */if (file.GetLine().StartsWith("FOV:"))
        //    {

        //    }
        //    else if (file.GetLine().StartsWith("MouseSens:"))
        //    {

        //    }
        //}
        return true;
    }

    public bool LoadSettings(Settings settings)
    {
        var file = new ConfigFile();
        Error err = file.Load(Settings);

        if (err != Error.Ok)
        {
            return false;
        }

        double FOV = (double)file.GetValue("Visual", "FOV");
        double MouseSens = (double)file.GetValue("Controls", "MouseSens");
        settings.FOV_Slider.Value = FOV;
        settings.MouseSens_Slider.Value = MouseSens;
        settings._Process(0);
        return true;
    }
}
