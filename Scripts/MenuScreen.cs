using Godot;
using System;

public class MenuScreen : Control
{
    SceneChanger _sceneChanger;

    public override void _Ready()
    {
        _sceneChanger = (SceneChanger)GetNode("/root/SceneChanger");
        _sceneChanger.changeScene("res://Screens/GameScreen.tscn", 1f);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
