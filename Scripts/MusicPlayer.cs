using Godot;
using System;

public class MusicPlayer : Node2D
{
    AudioStreamPlayer _asp;

    public override void _Ready()
    {
        _asp = (AudioStreamPlayer)GetNode("AudioStreamPlayer");
        _asp.Play();
    }
}
