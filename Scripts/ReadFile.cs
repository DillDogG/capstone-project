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
            file.SetValue("General", "LoadedFromSave", false);
            file.SetValue("Level", "CurrentLevel", "Test Level");
            file.SetValue("Level", "Time", 0);
            file.SetValue("Player", "Health", 150);
            file.SetValue("Player", "Credits", 0);
            //file.SetValue("Player", "Inventory", 2);
            file.SetValue("Weapons", "SniperAmmo", 7);
            file.SetValue("Weapons", "SniperReserves", 35);
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

    public bool LoadSettings(StartSettings settings)
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

    public bool SaveGame(string CurrentLevel, int Checkpoint, double Time, double Health, int Credits, Weapon[] Inventory)
    {
        var file = new ConfigFile();
        Error err = file.Load(SaveData);

        if (err != Error.Ok)
        {
            return false;
        }
        file.SetValue("Level", "CurrentLevel", CurrentLevel);
        file.SetValue("Level", "CurrentCheckpoint", Checkpoint);
        file.SetValue("Level", "Time", Time);
        file.SetValue("Player", "Health", Health);
        file.SetValue("Player", "Credits", Credits);
        foreach (var item in Inventory)
        {
            file.SetValue("Inventory", item.Name, "");
            if (item is RangedWeapon)
            {
                RangedWeapon gun = (RangedWeapon)item;
                file.SetValue("Ammo", item.Name, gun.AmmoCount);
                file.SetValue("Reserves", item.Name, gun.AmmoReserves);
            }
        }
        file.Save(SaveData);
        return true;
    }

    public bool WasGameLoaded()
    {
        var file = new ConfigFile();
        Error err = file.Load(SaveData);

        if (err != Error.Ok)
        {
            return false;
        }
        bool returnValue = (bool)file.GetValue("General", "LoadedFromSave");
        file.SetValue("General", "LoadedFromSave", false);
        file.Save(SaveData);
        return returnValue;
    }

    public string LoadGame()
    {
        var file = new ConfigFile();
        Error err = file.Load(SaveData);

        if (err != Error.Ok)
        {
            return "";
        }
        string CurrentLevel = (string)file.GetValue("Level", "CurrentLevel");
        file.SetValue("General", "LoadedFromSave", true);
        file.Save(SaveData);
        return CurrentLevel;
    }

    public int LoadGame(Player player)
    {
        var file = new ConfigFile();
        Error err = file.Load(SaveData);

        if (err != Error.Ok)
        {
            return 0;
        }
        int Checkpoint = (int)file.GetValue("Level", "CurrentCheckpoint");
        double Time = (double)file.GetValue("Level", "Time");
        double Health = (double)file.GetValue("Player", "Health");
        int Credits = (int)file.GetValue("Player", "Credits");
        string[] Inventory = file.GetSectionKeys("Inventory");
        string[] Ammo = file.GetSectionKeys("Ammo");
        string[] Reserves = file.GetSectionKeys("Reserves");
        player.GlobalTimer._timer = Time;
        player.Health = Health;
        player.Credits = Credits;
        AddGuns(Inventory, player);
        foreach (var Weapon in player.Inventory)
        {
            if (Weapon is RangedWeapon)
            {
                RangedWeapon gun = (RangedWeapon)Weapon;
                foreach (var item in Ammo)
                {
                    if (item == Weapon.Name) { gun.AmmoCount = (int)file.GetValue("Ammo", item); break; }
                }
                foreach (var item in Reserves)
                {
                    if (item == Weapon.Name) { gun.AmmoReserves = (int)file.GetValue("Reserves", item); break; }
                }
                
            }
        }
        return Checkpoint;
    }

    private bool AddGuns(string[] Inventory, Player player)
    {
        foreach (var Weapon in Inventory)
        {
            bool ifBreak = false;
            foreach (var item in player.Inventory)
            {
                if (item.Name == Weapon) { ifBreak = true; break; }
            }
            if (ifBreak) { ifBreak = false; continue; }
            PackedScene scene = ResourceLoader.Load<PackedScene>("res://Prefabs/" + Weapon + ".tscn");
            player.AddGun("SniperBase", scene);
            if (scene.Instantiate() is MeleeWeapon) QueueFree();
        }
        return true;
    }

    public bool LoadStats(Player player)
    {
        var file = new ConfigFile();
        Error err = file.Load(SaveData);

        if (err != Error.Ok)
        {
            return false;
        }
        double Time = (double)file.GetValue("Level", "Time");
        double Health = (double)file.GetValue("Player", "Health");
        int Credits = (int)file.GetValue("Player", "Credits");
        string[] Inventory = file.GetSectionKeys("Inventory");
        string[] Ammo = file.GetSectionKeys("Ammo");
        string[] Reserves = file.GetSectionKeys("Reserves");
        player.GlobalTimer._timer = Time;
        player.Health = Health;
        player.Credits = Credits;
        AddGuns(Inventory, player);
        foreach (var Weapon in player.Inventory)
        {
            if (Weapon is RangedWeapon)
            {
                RangedWeapon gun = (RangedWeapon)Weapon;
                foreach (var item in Ammo)
                {
                    if (item == Weapon.Name) gun.AmmoCount = (int)file.GetValue("Ammo", item);
                    break;
                }
                foreach (var item in Reserves)
                {
                    if (item == Weapon.Name) gun.AmmoReserves = (int)file.GetValue("Reserves", item);
                    break;
                }

            }
        }
        return true;
    }

    public void ResetGame()
    {
        var file = new ConfigFile();
        Error err = file.Load(SaveData);

        if (err != Error.Ok)
        {
            return;
        }
        file.EraseSection("Inventory");
        file.SetValue("Level", "CurrentLevel", "");
        file.SetValue("Level", "CurrentCheckpoint", 0);
        file.SetValue("Level", "Time", 0);
        file.SetValue("Player", "Health", 150);
        file.SetValue("Player", "Credits", 0);
        file.SetValue("Inventory", "Sniper_Rifle", "");
        file.SetValue("Ammo", "Sniper_Rifle", 7);
        file.SetValue("Reserves", "Sniper_Rifle", 14);
        file.SetValue("Inventory", "Sword", "");
        file.Save(SaveData);
    }
}
