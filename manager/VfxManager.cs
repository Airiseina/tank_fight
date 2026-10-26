using Godot;
using Godot.Collections;

public partial class VfxManager : Node
{
	[Export] public PackedScene ExpAnimScene { get; set; }
	[Export] public PackedScene ExpParticleScene { get; set; }
	private AudioStreamPlayer2D hitSound;
	private AudioStreamPlayer2D expSound;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hitSound = GetNode<AudioStreamPlayer2D>("HitSound");
		expSound = GetNode<AudioStreamPlayer2D>("ExpSound");
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.BulleytHit += onBulletHit;
		gm.EntityDied += onEntityDied;
	}
	private void onBulletHit(Vector2 pos)
	{
		spawnExpParticle(pos);
		hitSound.GlobalPosition = pos;
		hitSound.Play();
	}
	private void onEntityDied(Vector2 pos, Array groups)
	{
		if (groups.Contains("Enemy"))
		{
			explode(pos,"regular_exp");

		}
		else if (groups.Contains("Player"))
		{
			explode(pos,"sonic_exp");
		}
	}
	private async void explode(Vector2 pos,string name)
	{
		var expAnim = ExpAnimScene.Instantiate() as AnimatedSprite2D;
		if(expAnim == null)
		{
			return;
		}
		expAnim.GlobalPosition = pos;
		AddChild(expAnim);
		expAnim.Play(name);
		expSound.GlobalPosition = pos;
		expSound.Play();
		await ToSignal(expAnim, AnimatedSprite2D.SignalName.AnimationFinished);
		expAnim.QueueFree();
	}
	private async void spawnExpParticle(Vector2 pos)
	{
		var expParticle = ExpParticleScene.Instantiate() as CpuParticles2D;
		if(expParticle == null)
		{
			return;
		}
		expParticle.GlobalPosition = pos;
		AddChild(expParticle);
		expParticle.Emitting = true;
		await ToSignal(expParticle, CpuParticles2D.SignalName.Finished);
		expParticle.QueueFree();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
