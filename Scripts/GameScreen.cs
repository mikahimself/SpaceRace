using Godot;
using System;

public class GameScreen : Node2D
{
    SceneChanger _sceneChanger;
    private Vector2 _screenSize;
    private PackedScene _spaceShip;

    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        SetupShips();
        SetupSignals();
        _sceneChanger = (SceneChanger)GetNode("/root/SceneChanger");
        _sceneChanger.revealScreen();
    }

    public void SetupShips()
    {
        _spaceShip = (PackedScene)ResourceLoader.Load("res://Scenes/SpaceShip.tscn");

        for (int i = 1; i < 3; i++)
        {
            var ship = (SpaceShip)_spaceShip.Instance();
            ship.Name = "SpaceShip" + i;
            ship.myId = i;
            ship.SetupShip(_screenSize);
            AddChild(ship);
        }
    }

    // Setup signals only after ships have been created/added
    private void SetupSignals()
    {
        var score = (ScoreMinder)GetNode("ScoreMinder");
        score.SetupSignals();

        var timer = (TimerRectangle)GetNode("TimerRectangle");
        timer.SetupSignals();
    }
}
