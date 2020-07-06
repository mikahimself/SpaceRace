using Godot;
using System;

public class CometContainer : Node2D
{
    private PackedScene cometScene;
    private int[] _startPositions = {0, 1024};
    private int _speed = 200;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cometScene = (PackedScene)ResourceLoader.Load("res://Scenes/Comet.tscn");
        GD.Randomize();
        InstanceComets();
    }

    public void InstanceComets() {
        int startX = _startPositions[0];
        int startPosY = 10;
        for (int i = 0; i < 50; i++) {
            Comet comet = (Comet)cometScene.Instance();
            comet.speed = _speed;
            int shuffle = (int)Godot.GD.RandRange(50, 1024);
            if (i % 2 == 0) {
                comet.Position = new Vector2(_startPositions[0] + shuffle, startPosY * i + 14);
                comet.direction = 1;
            } else {
                comet.Position = new Vector2(_startPositions[1] - shuffle, startPosY * i + 14);
                comet.direction = -1;
                comet.RotationDegrees = 180;
            }
            AddChild(comet);
        }
    }
}
