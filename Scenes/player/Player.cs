using Godot;
using System;

public partial class Player : Area2D
{
	private Vector2 direction = Vector2.Zero;
	private Vector2 targetPosition;
	private float speed = 0;
	private int health = 10;
	private AudioStreamPlayer shootSound;
	[Export] public float MaxSpeed { get; set;}= 300;
	[Export] public PackedScene BulletScene{ get; set;}
	private Marker2D marker2D;
	private Sprite2D gun;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		marker2D = GetNode<Marker2D>("Gun/Marker2D");
		gun = GetNode<Sprite2D>("Gun");
		shootSound = GetNode<AudioStreamPlayer>("ShootSound");
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.PlayerKilled+=onPlayerKilled;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		move(delta);
		target();
		shoot();
	}
	// private void simpleMove(double delta)
	// {
	// 	direction = Input.GetVector("left", "right", "up", "down");
	// 	if (direction != Vector2.Zero)
	// 	{
	// 		var angleRad = direction.Angle();
	// 		Rotation = angleRad;
	// 		speed = MaxSpeed;
	// 	}
	// 	else
	// 	{
	// 		speed = 0;
	// 	}
	// 	Position += Transform.X * speed * (float)delta;
	// }
	private void onPlayerKilled()
	{
		SetProcess(false);
	}
	public void ReduceHealth()
	{
		if (health > 0){
			health--;
			var gm = GetNode<GameManager>("/root/GameManager");
			gm.EmitSignal(GameManager.SignalName.UpdateHealthUI,health);
		}
		if (health <= 0)
		{
			var gm = GetNode<GameManager>("/root/GameManager");
			gm.EmitSignal(GameManager.SignalName.PlayerKilled);
		}
	}
	private void move(double delta)
	{
		direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			var angleRad = direction.Angle();
			Rotation = Mathf.RotateToward(Rotation,angleRad,2*Mathf.Pi*(float)delta);
			speed = Mathf.MoveToward(speed, MaxSpeed, MaxSpeed * (float)delta);
		}
		else
		{
			speed = Mathf.MoveToward(speed, 0, 2*MaxSpeed * (float)delta);
		}
		Position += Transform.X * speed * (float)delta;
		checkBorder();
	}
	private void checkBorder()
	{
		var size = GetViewportRect().Size;
		Position = Position.Clamp(Vector2.Zero, size);
	}
	private void target()
	{
		targetPosition = GetGlobalMousePosition();
		gun.LookAt(targetPosition);
	}
	private void shoot()
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			shootSound.Play();
			var bullet = BulletScene.Instantiate()as Node2D;
			if (bullet == null)
			{
				return;
			}
			bullet.GlobalPosition = marker2D.GlobalPosition;
			bullet.LookAt(targetPosition);
			bullet.TopLevel = true;
			AddChild(bullet);
		}
	}
}
