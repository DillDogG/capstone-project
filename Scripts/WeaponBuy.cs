using Godot;
using System;

public partial class WeaponBuy : Node3D
{
	[Export]
	public int MainPrice { get; set; }

	[Export]
	public int AmmoPrice { get; set; }

	[Export]
	public PackedScene GunPrefab { get; set; }

    [Export]
    public Area3D area { get; set; }

    [Export]
    public string WeaponName { get; set; }

    [Export]
    public Label BuyDisplay { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        if (area.HasOverlappingBodies())
        {
            foreach (var Overlaps in area.GetOverlappingBodies())
            {
                if (Overlaps is Player)
                {
                    Player player = (Player)Overlaps;
                    Weapon playerSave = null;
                    foreach (Weapon weapon in player.Inventory)
                    {
                        if (weapon.Name == WeaponName)
                        {
                            playerSave = weapon;
                            break;
                        }
                    }
                    if (playerSave == null) BuyDisplay.Text = "Purchase " + WeaponName + ": Price " + MainPrice + " (Press E)";
                    else BuyDisplay.Text = "Purchase " + WeaponName + " Ammo: Price " + AmmoPrice + " (Press E)";
                    if (Input.IsActionJustPressed("Interact"))
                    {
                        if (playerSave == null && player.Credits >= MainPrice)
                        {
                            player.AddCredits(-MainPrice);
                            Weapon gun = GunPrefab.Instantiate<Weapon>();
                            if (gun is MeleeWeapon)
                            {
                                player.AddGun("SwordBase", GunPrefab);
                                QueueFree();
                            }
                            else player.AddGun("SniperBase", GunPrefab);
                        }
                        else if (player.Credits >= AmmoPrice)
                        {
                            if (playerSave is RangedWeapon)
                            {
                                player.AddCredits(-AmmoPrice);
                                RangedWeapon gun = (RangedWeapon)playerSave;
                                gun.AmmoReserves = gun.MaxAmmoReserves + (gun.MaxAmmoCount - gun.AmmoCount);
                            }
                            else QueueFree();
                        }
                    }
                    break;
                }
                if (BuyDisplay.Text == "Purchase " + WeaponName + ": Price " + MainPrice + " (Press E)" || BuyDisplay.Text == "Purchase " + WeaponName + " Ammo: Price " + AmmoPrice + " (Press E)") BuyDisplay.Text = "";
            }
        }
    }
}
