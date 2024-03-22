using Godot;
using System;

public partial class Pterodactyl : StaticBody2D
{
    [Export]
    private float _speed = 800.0f;

    private AnimatedSprite2D _animatedSprite;
    private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");

        _visibleOnScreenNotifier2D.ScreenExited += OnScreenExited;
    }

    public override void _Process(double delta)
    {
        Position += new Vector2(-_speed * (float)delta, 0);
    }

    public void Stop()
    {
        _speed = 0;
        _animatedSprite.Stop();
    }

    private void OnScreenExited() => QueueFree();

}
