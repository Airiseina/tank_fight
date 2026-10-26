using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private Vector2 direction = Vector2.Zero;
	private float speed = 0;
	private bool canShake = false;
	private bool gameEnded = false;
	[Export] public float MaxSpeed { get; set; } = 300f;
	private AudioStreamPlayer engineSound;
	private WeaponComponent weaponComponent;
	private TrailComponent trailComponent;
	private HealthComponent healthComponent;
	private HurtBoxComponent hurtBoxComponent;
	private Camera2D camera2D;
	private Timer timer;
	private AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		engineSound = GetNode<AudioStreamPlayer>("EngineSound");
		weaponComponent = GetNode<WeaponComponent>("WeaponComponent");
		trailComponent = GetNode<TrailComponent>("TrailComponent");
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		hurtBoxComponent = GetNode<HurtBoxComponent>("HurtBoxComponent");
		camera2D = GetNode<Camera2D>("Camera2D");
		timer = GetNode<Timer>("Timer");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		hurtBoxComponent.GetDamage += healthComponent.GetDamage;
		hurtBoxComponent.GetDamage += onGetDamage;
		healthComponent.HealthChanged+=onHealthChanged;
		healthComponent.Died+=onDied;
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.PlayerWin += onPlayerWin;
		timer.Timeout += onTimeOut;
	}
	private void onHealthChanged(float healthPercent)
	{
		int percent = Mathf.RoundToInt(
        Mathf.Clamp(healthPercent, 0f, 1f) * 100f
    );
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.EmitSignal(GameManager.SignalName.UpdateHealthUI, percent);
	}
	private void onDied()
	{
		if (gameEnded) return;
		StopControl();
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.EmitSignal(GameManager.SignalName.EntityDied, GlobalPosition, GetGroups());
		gm.EmitSignal(GameManager.SignalName.PlayerKilled);
		Hide();
	}
	private void onPlayerWin()
	{
		if (gameEnded) return;
		StopControl();
	}
	private void StopControl()
	{
		gameEnded = true;
		direction = Vector2.Zero;
		speed = 0f;
		Velocity = Vector2.Zero;
		trailComponent.Stop();
		timer.Stop();
		engineSound.Stop();
		canShake = false;
		camera2D.Offset = Vector2.Zero;
		SetPhysicsProcess(false);
		SetProcess(false);
		SetProcessUnhandledInput(false);
		hurtBoxComponent.SetDeferred(Area2D.PropertyName.Monitorable, false);
	}
	private void shake()
	{
		camera2D.Offset = new Vector2((float)GD.RandRange(-3.0, 3.0),(float)GD.RandRange(-3.0, 3.0));
	}
	private void Move(double delta)
{
    float dt = (float)delta;
    direction = Input.GetVector("left", "right", "up", "down");

    if (direction != Vector2.Zero)
    {
        float angleRad = direction.Angle();
        Rotation = Mathf.RotateToward(Rotation, angleRad, 2 * Mathf.Pi * dt);
    speed = Mathf.MoveToward(speed, MaxSpeed, MaxSpeed * dt);
        trailComponent.Start();
    }
    else
    {
        speed = Mathf.MoveToward(speed, 0f, 2 * MaxSpeed * dt);
        trailComponent.Stop();
    }
    Velocity = Transform.X * speed;
    MoveAndSlide();
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (gameEnded) return;
		Vector2 targetPos = GetGlobalMousePosition();
		weaponComponent.Target(targetPos);
		if(@event.IsActionPressed("shoot"))
		{
			weaponComponent.Shoot(targetPos);
		}
	}
	private void onGetDamage(float value)
	{
		if (gameEnded) return;
		animationPlayer.Play("flash");
		canShake = true;
		timer.Start();
	} 
	private void onTimeOut()
	{
		canShake = false;
		camera2D.Offset = Vector2.Zero;
	}
	public void UpgradeHealth()
	{
		healthComponent.Upgrade(1);
	}
	public void UpgradeWeapon()
	{
		weaponComponent.Upgrade(0.1f);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (gameEnded) return;
		Move(delta);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (gameEnded) return;
    if (canShake)
    {
        shake();
    }
	}
	
	
}
