using Godot;
using System;

public partial class HurtBoxComponent : Area2D
{
	[Signal] public delegate void GetDamageEventHandler(float damage);
	[Export] public float Armor { get; set; } = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public void GetHurt(float damage)
	{
		var finalDamage = Math.Max(damage-Armor, 0);
		EmitSignal(SignalName.GetDamage, finalDamage);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
