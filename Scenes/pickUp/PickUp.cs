using Godot;
using System;

public partial class PickUp : Area2D
{
	public enum Pickups
    {
        Gun,
        Health
    }
	[Signal] public delegate void GetPickUpEventHandler(Vector2 pos,Pickups pickupType);
	[Export] public Pickups PickupType { get; set; } = Pickups.Health;
	private AnimationPlayer animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		BodyEntered += onBodyEntered;
	}
	private async void onBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			if(body is Player player)
			{
				if(PickupType == Pickups.Gun)
				{
					player.UpgradeWeapon();
				}
				else if(PickupType == Pickups.Health)
				{
					player.UpgradeHealth();
				}
				animationPlayer.Play("pickup");
				await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
				QueueFree();
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
