using Godot;
using Godot.Collections;
using System.Threading.Tasks;

public partial class EnemyManager : Node
{
	[Signal] public delegate void WaveDiedEventHandler();
	[Export] public PackedScene EnemyTankScene { get; set; }
	private  int diedTank;
	private int diedEnemy;
	private  int totalEnemySize;
	[Export] public Node2D PathHolder { get; set; }
	[Export] public int WaveNumber { get; set; } = 3;
	[Export] public int EnemyInWave { get; set; } = 8;
	[Export] public int EnemySpawnDelay { get; set; } = 2;
	[Export] public Node2D EnemyHolder { get; set; }
	private Array<Node> paths;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.EntityDied += onEntityDied;
		checkEnemySize();
		if (PathHolder != null)
        {
            paths = PathHolder.GetChildren();
        }
		spawnWaves();
	}
	private void checkEnemySize()
	{
		totalEnemySize = 0;
    if (EnemyHolder != null)
    {
        foreach (Node enemy in EnemyHolder.GetChildren())
        {
            if (enemy.IsInGroup("EnemyTower"))
            {
                totalEnemySize += 1;
            }
        }
    }
    totalEnemySize += WaveNumber * EnemyInWave;
    var gm = GetNode<GameManager>("/root/GameManager");
    gm.SetTotalEnemySize(totalEnemySize);
	}
	private async void spawnWaves()
{
    for (int i = 0; i < WaveNumber; i++)
    {
        await spawnWave();

        while (diedTank < EnemyInWave)
        {
            await ToSignal(this, SignalName.WaveDied);
        }
    }
}	private async Task spawnWave()
	{
		diedTank = 0;
		for (int i = 0; i < EnemyInWave; i++)
		{
			CallDeferred(nameof(spawnEnemy));
			var timer= GetTree().CreateTimer(EnemySpawnDelay);
			await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
		}
	}
	private void spawnEnemy()
	{
		var enemyTank = EnemyTankScene.Instantiate()as EnemyTank;
		if(enemyTank == null)
		{
			return;
		}
		var path = paths.PickRandom();
		if(path == null)
		{
			return;
		}
		enemyTank.Speed = GD.RandRange(100, 150);
		path.AddChild(enemyTank);

	}
	private void onEntityDied(Vector2 pos, Array groups)
	{
		checkWaveDied(groups);
		checkKillEnmey(groups);
	}
	private void checkWaveDied(Array groups)
	{
		if (!groups.Contains("EnemyTank"))
		{
			return;
		}
		
			diedTank += 1;
			if (diedTank == EnemyInWave)
			{
				EmitSignal(SignalName.WaveDied);
			
		}
	}
	private void checkKillEnmey(Array groups)
	{
		if (groups.Contains("Enemy"))
		{
			diedEnemy += 1;
			var gm = GetNode<GameManager>("/root/GameManager");
			gm.EmitSignal(GameManager.SignalName.ScoreUpdate);
			if (diedEnemy == totalEnemySize)
			{
				gm.EmitSignal(GameManager.SignalName.PlayerWin);
			}
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
