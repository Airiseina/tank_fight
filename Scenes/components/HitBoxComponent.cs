using Godot;
using System;

public partial class HitBoxComponent : Area2D
{
	[Signal] public delegate void HitEventHandler();
	[Export] public int Damage { get; set; } = 1;
	[Export] public bool HitMultiple { get; set; } = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		BodyEntered += OnBodyEntered;
	}
	public void ApplyHit(Area2D hurtBox)
	{
		if (hurtBox.HasMethod("GetHurt"))
		{
			hurtBox.Call("GetHurt", Damage);
		}
		SetDeferred(Area2D.PropertyName.Monitoring, HitMultiple);
	}
	public void OnAreaEntered(Area2D area)
	{
		EmitSignal(SignalName.Hit);
		ApplyHit(area);
	}
	public void OnBodyEntered(Node body)
	{
		EmitSignal(SignalName.Hit);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
