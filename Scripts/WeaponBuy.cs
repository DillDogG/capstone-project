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

	bool Purchased = false;

    [Export]
    public Area3D area { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        if (area.HasOverlappingBodies())
        {
            foreach (var Overlaps in area.GetOverlappingBodies())
            {
                if (Overlaps is Player && Input.IsActionJustPressed("Interact"))
                {
                    Player player = (Player)Overlaps;
                    if (!Purchased && player.Credits >= MainPrice)
                    {
                        player.AddCredits(-MainPrice);
                        //Weapon gun = GunPrefab.Instantiate<Weapon>();
                        //gun.Initialize(Position, player, GetNode<HitMarker>("UserInterface/HitMarker"));
                        //player.Inventory.Add(gun);
                        //player.GetNode<Node3D>("Pivot").AddChild(gun);
                        Purchased = true;
                    }
                    else if (player.Credits >= AmmoPrice)
                    {
                        player.AddCredits(-AmmoPrice);
                    }
                    break;
                }
            }
        }
    }
}
