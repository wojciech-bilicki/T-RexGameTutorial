using Godot;
using System;

public partial class ObstacleSpawner : Node
{
    [Export]
    private float _decreaseSpawnTimeOnDifficultyIncrease = 0.2f;

    [Export]
    private float _chanceToSpawnPterodactyl = 0.1f;

    private PackedScene _cactusScene = GD.Load<PackedScene>("res://Scenes/cactus.tscn");
    private PackedScene _pterodactylScene = GD.Load<PackedScene>("res://Scenes/pterodactyl.tscn");


    RectangleShape2D[] collisionsShapes = new RectangleShape2D[6];
    Texture2D[] texture2Ds = new Texture2D[6];

    private StaticBody2D _ground1;
    private StaticBody2D _ground2;

    private Timer _obstacleSpawnTimer;
    private Node2D _spawnPoint;
    private Node main;

    private float[] _obstacleSpawnTimeRange = { 1f, 2f };

    public override void _Ready()
    {
        main = GetNode<Node>("/root/main");
        _spawnPoint = GetNode<Node2D>("SpawnPoint");
        _obstacleSpawnTimer = GetNode<Timer>("Timer");
        _ground1 = GetParent().GetNode<StaticBody2D>("Ground_1");
        _ground2 = GetParent().GetNode<StaticBody2D>("Ground_2");

        collisionsShapes[0] = GD.Load<RectangleShape2D>("res://CollisionShapes/cactus/cactus_large_double.tres");
        collisionsShapes[1] = GD.Load<RectangleShape2D>("res://CollisionShapes/cactus/cactus_large_single.tres");
        collisionsShapes[2] = GD.Load<RectangleShape2D>("res://CollisionShapes/cactus/cactus_large_triple.tres");

        collisionsShapes[3] = GD.Load<RectangleShape2D>("res://CollisionShapes/cactus/cactus_small_double.tres");
        collisionsShapes[4] = GD.Load<RectangleShape2D>("res://CollisionShapes/cactus/cactus_small_single.tres");
        collisionsShapes[5] = GD.Load<RectangleShape2D>("res://CollisionShapes/cactus/cactus_small_triple.tres");

        texture2Ds[0] = GD.Load<Texture2D>("res://Assets/Sprites/Cactus_Large_Double.png");
        texture2Ds[1] = GD.Load<Texture2D>("res://Assets/Sprites/Cactus_Large_Single.png");
        texture2Ds[2] = GD.Load<Texture2D>("res://Assets/Sprites/Cactus_Large_Triple.png");
        texture2Ds[3] = GD.Load<Texture2D>("res://Assets/Sprites/Cactus_Small_Double.png");
        texture2Ds[4] = GD.Load<Texture2D>("res://Assets/Sprites/Cactus_Small_Single.png");
        texture2Ds[5] = GD.Load<Texture2D>("res://Assets/Sprites/Cactus_Small_Triple.png");

        _obstacleSpawnTimer.Timeout += SpawnObstacle;
    }

    private void SpawnObstacle()
    {
        Random random = new Random();
        float randomValue = (float)random.NextDouble();

        if (randomValue <= _chanceToSpawnPterodactyl)
        {
            SpawnPterodactyl();
        }
        else
        {

            SpawnCactus();
        }

        _obstacleSpawnTimer.Stop();
        _obstacleSpawnTimer.WaitTime = (float)GD.RandRange(_obstacleSpawnTimeRange[0], _obstacleSpawnTimeRange[1]);
        _obstacleSpawnTimer.Start();
    }


    private void SpawnPterodactyl()
    {
        var pterodactyl = _pterodactylScene.Instantiate<Pterodactyl>();

        main.AddChild(pterodactyl);
        var positionY = GetViewport().GetVisibleRect().Size.Y - (float)GD.RandRange(128.0, 192.0);
        pterodactyl.Position = new Vector2(_spawnPoint.Position.X, positionY);
    }


    private void SpawnCactus()
    {
        var cactus = _cactusScene.Instantiate<Cactus>();

        var parentGround = _ground1.GlobalPosition.X > _ground2.GlobalPosition.X ? _ground1 : _ground2;

        parentGround.AddChild(cactus);


        Random random = new Random();
        int randomIndex = random.Next(0, 6);
        cactus.Sprite2D.Texture = texture2Ds[randomIndex];
        cactus.CollisionShape2D.Shape = collisionsShapes[randomIndex];

        cactus.GlobalPosition = new Vector2(_spawnPoint.Position.X, parentGround.GlobalPosition.Y);

    }

    public void IncreaseDifficulty()
    {
        _chanceToSpawnPterodactyl += 0.05f;

        if (_obstacleSpawnTimeRange[0] > 0.5f)
        {
            _obstacleSpawnTimeRange[0] -= _decreaseSpawnTimeOnDifficultyIncrease;
            _obstacleSpawnTimeRange[1] -= _decreaseSpawnTimeOnDifficultyIncrease;
        }

    }

}
