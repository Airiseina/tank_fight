using Godot;
using Godot.Collections;

public partial class GameManager : Node
{
	[Signal] public delegate void UpdateHealthUIEventHandler(int health);
	[Signal] public delegate void PlayerKilledEventHandler();
	[Signal] public delegate void PlayerWinEventHandler();
	[Signal] public delegate void BulleytHitEventHandler(Vector2 pos);
	[Signal] public delegate void EntityDiedEventHandler(Vector2 pos, Array groups);
	[Signal] public delegate void ScoreUpdateEventHandler();
	[Signal] public delegate void UpdateScoreUIEventHandler(int score, int total);
	public int Score { get; set; } = 0;
	public int TotalEnemySize { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScoreUpdate += OnScoreUpdate;
	}
	public void Restart()
	{
		Score = 0;
		GetTree().ReloadCurrentScene();
	}
	public void SetTotalEnemySize(int value)
	{
		TotalEnemySize = value;
	}
	private void OnScoreUpdate()
	{
		Score += 1;
		EmitSignal(SignalName.UpdateScoreUI, Score,TotalEnemySize);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
