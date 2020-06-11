using Godot;
using System;

public class BulletContainer : Node2D
{
    private PackedScene bulletScene;
    private int[] _startPositions = {0, 1024};
    private int _speed = 175;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bulletScene = (PackedScene)ResourceLoader.Load("res://Scenes/Bullet.tscn");
        GD.Randomize();
        InstanceBullets();
    }

    public void InstanceBullets() {
        int startX = _startPositions[0];
        int startPosY = 10;
        for (int i = 0; i < 50; i++) {
            Bullet bullet = (Bullet)bulletScene.Instance();
            bullet.speed = _speed;
            int shuffle = (int)Godot.GD.RandRange(50, 1024);
            if (i % 2 == 0) {
                bullet.Position = new Vector2(_startPositions[0] + shuffle, startPosY * i + 10);
                bullet.direction = 1;
            } else {
                bullet.Position = new Vector2(_startPositions[1] - shuffle, startPosY * i + 10);
                bullet.direction = -1;
            }
            AddChild(bullet);
        }
    }
}
