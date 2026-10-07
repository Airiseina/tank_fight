using Godot;
using System;

public partial class GameManager : Node
{
	[Signal] public delegate void EnemyKilledEventHandler(Vector2 pos);
	[Signal] public delegate void UpdateHealthUIEventHandler(int health);
	[Signal] public delegate void PlayerKilledEventHandler();
	[Signal] public delegate void PlayerWinEventHandler();
	public int Score { get; set; } = 0;
	public int EnemySize { get; set; } = 4;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EnemyKilled += OnEnemyKilled;
	}
	private void OnEnemyKilled(Vector2 pos)
	{
		Score += 1;
		if (Score == EnemySize)
		{
			EmitSignal(SignalName.PlayerWin);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
