using Godot;
using Godot.Collections;

public partial class PickupManager : Node
{
	[Export] public PackedScene GunPickupScene { get; set; }
	[Export] public PackedScene HealthPickupScene { get; set; }
	private Array<PackedScene> pickupArray;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pickupArray = new Array<PackedScene> { GunPickupScene, HealthPickupScene };
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.EntityDied += onEntityDied;
	}
	private void onEntityDied(Vector2 pos, Array groups)
	{
		if (groups.Contains("Enemy"))
		{
			if (GD.RandRange(1, 10) > 5)
			{
				CallDeferred(nameof(spawnPickup),pos);
			}
		}
	}
	private void spawnPickup(Vector2 pos)
	{
		var pickupScene = pickupArray.PickRandom();
		if(pickupScene == null)
		{
			return;
		}
		var pickup = pickupScene.Instantiate() as Node2D;
		if(pickup == null)
		{
			return;
		}
		
			pickup.GlobalPosition = pos;
			AddChild(pickup);
		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
