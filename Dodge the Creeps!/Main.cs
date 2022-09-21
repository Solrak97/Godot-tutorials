using Godot;
using System;

public class Main : Node
{

#pragma warning disable 649
	[Export]
	public PackedScene MobScene;
#pragma warning restore 649

	public int Score;


	public override void _Ready()
	{
		GD.Randomize();
		//NewGame();
	}


	private void GameOver()
	{
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGamever();
	}


	public void NewGame()
	{
		Score = 0;

		GetNode<AudioStreamPlayer>("Music").Play();

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Position2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();

		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(Score);
		hud.ShowMessage("Get Ready!");
		
		GetTree().CallGroup("mobs", "queue_free");
	}

	public void OnScoreTimerTimeout()
	{
		Score++;
		GetNode<HUD>("HUD").UpdateScore(Score);
	}

	public void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void OnMobTimerTimeout()
	{
		var mob = (Mob)MobScene.Instance();

		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.Offset = GD.Randi();

		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		mob.Position = mobSpawnLocation.Position;

		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		var velocity = new Vector2((float)GD.RandRange(150f, 250f), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		AddChild(mob);
	}
}
