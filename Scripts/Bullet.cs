using Godot;
using System;

public class Bullet : Node2D
{
    [Export]
    public int direction;
    [Export]
    public int speed = 50;
    public Vector2 startingPoint = Vector2.Zero;
    private Vector2 _screenSize;
    private int _spriteWidth = 8;

    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
    }

    public override void _Process(float delta)
    {
        moveBullet(delta);
    }

    public void moveBullet(float delta) {
        Position += new Vector2(speed * direction * delta, 0);

        if (direction < 0 && (Position.x + _spriteWidth / 2) < 0) {
            Position = new Vector2(_screenSize.x - _spriteWidth / 2, Position.y);
        } else if (direction > 0 && (Position.x - _spriteWidth / 2) > _screenSize.x ) {
            Position = new Vector2(0 - _spriteWidth / 2, Position.y);
        }
    }
}
