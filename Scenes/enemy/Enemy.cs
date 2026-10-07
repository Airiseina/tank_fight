using Godot;
using System;

public partial class Enemy : Area2D
{
	private Node2D player;
	[Export] public PackedScene BulletScene{ get; set; }
	[Export] public int health{get;set;}=3;
	private Sprite2D gun;
	private Timer timer;
	private Marker2D marker2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gun=GetNode<Sprite2D>("Gun");
		timer=GetNode<Timer>("Timer");
		marker2D=GetNode<Marker2D>("Gun/Marker2D");
		player=GetTree().GetFirstNodeInGroup("Player")as Node2D;
		timer.Timeout +=OnTimeOut;
		timer.Start(1);
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.PlayerKilled += onPlayerKilled;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		findPlayer();
	}
	private void onPlayerKilled()
	{
		timer.Stop();
	}
	public void ReduceHealth()
	{
		if (health > 0){
			health--;
		}
		if (health <= 0)
		{
			var gm = GetNode<GameManager>("/root/GameManager");
			gm.EmitSignal(GameManager.SignalName.EnemyKilled,GlobalPosition);
			QueueFree();
		}
		
	}
	private void findPlayer()
	{
		var playerPos = player.GlobalPosition;
		gun.LookAt(playerPos);
	}
	private void OnTimeOut()
	{
		Shoot();
		timer.Start(GD.RandRange(1.0, 3.0));
	}
	private void Shoot()
	{
		var bullet = BulletScene.Instantiate()as Node2D;
		if (bullet == null)
		{
			return;
		}
		bullet.GlobalPosition = marker2D.GlobalPosition;
		bullet.Rotation=gun.Rotation;
		bullet.TopLevel = true;
		AddChild(bullet);
	}
}
