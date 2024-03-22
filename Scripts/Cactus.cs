using Godot;
using System;

public partial class Cactus : StaticBody2D
{
    public Sprite2D Sprite2D;
    public CollisionShape2D CollisionShape2D;

    private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

    public override void _Ready()
    {
        base._Ready();

        _visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
        CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

        _visibleOnScreenNotifier2D.ScreenExited += OnScreenExited;
    }

    private void OnScreenExited()
    {
        QueueFree();
    }

}
