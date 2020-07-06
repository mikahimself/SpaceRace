using Godot;
using System;

public class SceneChanger : CanvasLayer
{
    private AnimationPlayer _animationPlayer;
    private ColorRect _colorRect;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        _colorRect = (ColorRect)GetNode("Control/FadeRect");
    }

    async public void changeScene(string path, float delay)
    {
        await ToSignal(GetTree().CreateTimer(delay), "timeout");
        _animationPlayer.Play("FadeInOut");
        await ToSignal(_animationPlayer, "animation_finished");
        GetTree().ChangeScene(path);
    }

    public void revealScreen()
    {
        _animationPlayer.PlayBackwards("FadeInOut");
    }
}
